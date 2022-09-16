using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Helper;
using EgePakErp.Custom;
using EgePakErp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data.Entity;
using EgePakErp.Concrete;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using SelectPdf;
using System.IO;
using System.Web.Hosting;
using System.Configuration;

namespace EgePakErp.Controllers
{
    public class SiparisController : BaseController
    {
        //public List<Kalip> Kaliplar { get; set; }
        public KalipRepository kalipRepo { get; set; }
        public SiparisRepository siparisRepo { get; set; }
        public SiparisKalipRepository siparisKalipRepo { get; set; }
        public UrunRepository urunRepo { get; set; }

        public SiparisController()
        {
            kalipRepo = new KalipRepository();
            siparisRepo = new SiparisRepository();
            siparisKalipRepo = new SiparisKalipRepository();
            urunRepo = new UrunRepository();
        }

        [Menu("Sipariş Listesi", "flaticon2-cart icon-xl", "Sipariş", 0, 5)]
        [Yetki("Sipariş Listesi", "Sipariş")]
        public ActionResult Index()
        {
            var model = siparisRepo.GetAll().ToList();
            return View(model);
        }

        [Menu("Üretimdeki Siparişler", "flaticon2-cart icon-xl", "Sipariş", 0, 5)]
        [Yetki("Üretimdeki Siparişler", "Sipariş")]
        public ActionResult UretimdekiSiparisler()
        {
            var model = siparisRepo.GetAll(x => x.SiparisDurumId == (int)ESiparisType.Uretimde).ToList();
            return View(model);
        }

        [Menu("Sipariş Formu", "flaticon2-cart icon-xl", "Sipariş", 0, 5)]
        public ActionResult SiparisFormu(int siparisId = 0)
        {
            if (siparisId != 0)
            {
                var siparis = siparisRepo.Get(siparisId);
                return View(siparis);
            }
            return View(new Siparis());
        }

        [Yetki("Sipariş Listesi", "Sipariş")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "SiparisId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = siparisRepo.GetAll();

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("SiparisId Desc");
                dtMeta.field = "SiparisId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                SiparisId = i.SiparisId,
                SiparisAdi = i.SiparisAdi,
                Cari = i.Cari.Unvan,
                Urun = i.Urun.UrunCinsi.Kisaltmasi + i.Urun.UrunNo,
                UrunId = i.UrunId,
                Durum = i.SiparisDurum.Durum,
                DurumId = i.SiparisDurumId
            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = siparisRepo.Get((int)id);
            return PartialView(model);
        }
        public JsonResult Kaydet(Siparis siparis)
        {
            Response response = new Response();
            try
            {
                var cariSiparisleri = siparisRepo.GetAll(x => x.CariId == siparis.CariId && x.UrunId == siparis.UrunId).OrderByDescending(x => x.KayitTarihi).ToList();
                var sipNo = 0;

                var boyaKodlar = siparis.SiparisKalip.Where(x => x.TozBoyaKodId != null).Select(x => x.TozBoyaKodId + "_" + x.KalipKod).ToList();
                var yaldizIdList = siparis.SiparisKalip.Where(x => x.YaldizId != null).Select(x => x.YaldizId + "_" + x.KalipKod).ToList();

                List<string> yaldizlar = yaldizIdList.ConvertAll<string>(x => x.ToString());
                List<string> boyaKodList = boyaKodlar.ConvertAll<string>(x => x.ToString());
                string yaldizListToString = String.Join(", ", yaldizlar);
                string boyaKodListToString = String.Join(", ", boyaKodList);


                var urun = urunRepo.Get(siparis.UrunId);
                var cari = Db.Cari.Include(x => x.BaglantiTipi).FirstOrDefault(x => x.CariId == siparis.CariId);
                var urunKod = urun.UrunCinsi.Kisaltmasi + urun.UrunNo;



                foreach (var sip in cariSiparisleri)
                {
                    var _boyaKodlar = sip.SiparisKalip.Where(x => x.TozBoyaKodId != null).Select(x => x.TozBoyaKod + "_" + x.KalipKod).ToList();
                    var _yaldizIdList = sip.SiparisKalip.Where(x => x.YaldizId != null).Select(x => x.YaldizId + "_" + x.KalipKod).ToList();

                    List<string> _yaldizlar = _yaldizIdList.ConvertAll<string>(x => x.ToString());
                    List<string> _boyaKodList = _boyaKodlar.ConvertAll<string>(x => x.ToString());

                    string _yaldizListToString = String.Join(", ", _yaldizlar);
                    string _boyaKodListToString = String.Join(", ", _boyaKodList);

                    if (yaldizListToString != _yaldizListToString || boyaKodListToString != _boyaKodListToString)
                    {
                        var splitSiparisNo = sip.SiparisAdi.Split('-');
                        var siparisNo = Convert.ToInt32(splitSiparisNo[2]);
                        sipNo = siparisNo += 1;
                        break;
                    }
                }

                //siparis.SiparisAdi = cari.MusteriNo + urunCinsi + urunNo + String.Join(", ", yaldizlar) + String.Join(", ", boyaKodList);

                siparis.SiparisAdi = cari.MusteriNo + "-" + urunKod + "-" + sipNo;
                siparis.KayitTarihi = DateTime.Now;
                siparis.SiparisDurumId = (int)ESiparisType.SiparisAlindi;
                siparisRepo.Insert(siparis);

                response.Success = true;
                response.Description = "Sipariş başarı ile kaydedildi";
                return Json(response);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response);
            }

        }

        
        public JsonResult KisitliKaydet(Siparis form)
        {
            var response = new Response();

            try
            {
                if (form.SiparisId == 0)
                {
                    siparisRepo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = siparisRepo.Get(form.SiparisId);
                    if (entity != null)
                    {
                        //alanları güncelle
                        var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                        foreach (var prop in propList)
                        {
                            if (form.Include.Contains(prop.Name))
                            {
                                prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                            }
                        }
                        siparisRepo.Update(entity);
                        response.Success = true;
                        response.Description = "Güncellendi.";
                    }

                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }

            return Json(response);

        }
        public JsonResult SiparisKalipGuncelle(int siparisKalipId, decimal maliyet)
        {
            Response response = new Response();
            try
            {
                var siparisKalip = siparisKalipRepo.Get(siparisKalipId);
                siparisKalip.Maliyet = maliyet;
                siparisKalipRepo.Update(siparisKalip);

                response.Success = true;
                response.Description = "Sipariş başarı ile güncellendi";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response);
            }
        }
        public JsonResult TopluSiparisKalipGuncelle(List<SiparisKalip> liste, decimal toplam, decimal toplamUsd, decimal toplamEur, int siparisId,double nakitKatsayi,double vadeliKatsayi)
        {
            Response response = new Response();
            try
            {
                if (liste != null)
                {
                    foreach (var item in liste)
                    {
                        var siparisKalip = siparisKalipRepo.Get(item.SiparisKalipId);
                        if (siparisKalip != null)
                        {
                            siparisKalip.Maliyet = item.Maliyet;
                            siparisKalip.isEnable = item.isEnable;
                            siparisKalip.TozBoyaKodId = item.TozBoyaKodId;
                            if (item.YaldizId != null && item.YaldizId != 0)
                            {
                                siparisKalip.YaldizId = item.YaldizId;
                            }

                            siparisKalip.Aciklama = item.Aciklama;
                            siparisKalip.Formul = item.Formul;
                            siparisKalipRepo.Update(siparisKalip);
                        }
                    }
                }

                var siparis = siparisRepo.Get(siparisId);

                if (siparis != null)
                {
                    siparis.TeklifFiyat = toplam;
                    siparis.TeklifFiyatUsd = toplamUsd;
                    siparis.TeklifFiyatEur = toplamEur;
                    siparis.NakitKatsayi = nakitKatsayi;
                    siparis.VadeliKatsayi = vadeliKatsayi;
                    siparisRepo.Update(siparis);
                    response.Success = true;
                    response.Description = "Sipariş başarı ile güncellendi";
                    return Json(response);
                }

                response.Success = false;
                response.Description = "sipariş bulunamadı.";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response);
            }
        }

        public JsonResult Guncelle(Siparis siparis)
        {
            Response response = new Response();
            try
            {
                var _siparis = siparisRepo.Get(siparis.SiparisId);
                _siparis.Aciklama = siparis.Aciklama;
                _siparis.SiparisDurumId = siparis.SiparisDurumId;
                siparisRepo.Update(_siparis);

                response.Success = true;
                response.Description = "sipariş güncellendi.";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response);
            }
        }

        [HttpPost]
        public PartialViewResult UrunKaliplari(int urunId, List<int> exclude, List<int> includes)
        {
            var KalipListe = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .ToList();

            //exclude = çıkarılan kalıpların id listesi.
            var model = new List<Kalip>();
            if (includes.Count() > 0)
            {
                foreach (var item in includes)
                {
                    //var kalip = Db.Kalip.FirstOrDefault(x => x.KalipId == item);
                    var kalip = kalipRepo.Get(item);
                    if (kalip != null)
                    {
                        model.Add(kalip);
                    }
                }
            }

            if (exclude == null)
            {
                var list = Db.KalipUrunRelation.Include("Kalip").Where(x => x.UrunId == urunId).Select(x => x.Kalip).ToList();
                model.AddRange(list);

            }

            else
            {

                var _list = Db.KalipUrunRelation
                    .Include("Kalip")
                    .Where(x => x.UrunId == urunId && x.Kalip.isAktive == true && !exclude.Contains(x.KalipId)).Select(x => x.Kalip.KalipId)
                    .ToList();
                foreach (var kId in _list)
                {
                    var kalip = KalipListe.FirstOrDefault(x => x.KalipId == kId);
                    model.Add(kalip);
                }
            }

            ViewBag.urun = Db.Urun.Include(x => x.UrunCinsi).FirstOrDefault(x => x.UrunId == urunId);
            return PartialView(model.OrderBy(x => x.ParcaKodu).ToList());
        }

        [HttpGet]
        public PartialViewResult UrunKaliplari(int siparisId)
        {
            var KalipListe = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .ToList();
            var siparis = siparisRepo.Get(siparisId);

            var model = new List<Kalip>();
            var kalipKodList = siparis.SiparisKalip.GroupBy(x => new { x.KalipKod }, (key, group) => new
            {
                Kod = key.KalipKod,
            }).Select(x => x.Kod).ToList();

            foreach (var _kod in kalipKodList)
            {
                try
                {
                    var kalip = kalipRepo.Get(x => x.ParcaKodu == _kod);
                    if (kalip != null)
                    {
                        model.Add(kalip);
                    }

                }
                catch { }
            }

            ViewBag.urun = Db.Urun.Include(x => x.UrunCinsi).FirstOrDefault(x => x.UrunId == siparis.UrunId);
            return PartialView(model.OrderBy(x => x.ParcaKodu).ToList());
        }

        [HttpPost]
        public PartialViewResult MaliyetForm(List<int> idList, int urunId)
        {
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.ToLower().Contains("poşet")).BirimFiyat;
            ViewBag.urunId = urunId;
            var kaliplar = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Where(i => idList.Contains(i.KalipId)).ToList();
            return PartialView(kaliplar.OrderBy(x => x.ParcaKodu).ToList());
        }

        public PartialViewResult MaliyetFormSiparis(int siparisId)
        {
            var siparis = siparisRepo.Get(siparisId);
            ViewBag.siparisId = siparisId;
            ViewBag.urunId = siparis.UrunId;

            var kaliplar = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .ToList();
            List<Kalip> fixKalipList = new List<Kalip>();
            var idList = new List<int>();

            var kalipKodList = siparis.SiparisKalip.GroupBy(x => new { x.KalipKod }, (key, group) => new
            {
                Kod = key.KalipKod,
            }).Select(x => x.Kod).ToList();

            foreach (var _kod in kalipKodList)
            {
                try
                {
                    var kalip = kalipRepo.Get(x => x.ParcaKodu == _kod);
                    if (kalip != null)
                    {
                        fixKalipList.Add(kalip);
                    }

                }
                catch { }
            }

            return PartialView(fixKalipList.OrderBy(x => x.ParcaKodu).ToList());
        }


        public PartialViewResult MaliyetHesap(List<MaliyetDto> liste, double nakitKatsayi=1.25, double vadeliKatsayi=1.4)
        {
            var doviz = Db.DovizKur.FirstOrDefault();
            ViewBag.dolarKur = doviz.UsdKur;
            ViewBag.euroKur = doviz.EurKur;
            ViewBag.nakitKatsayi = nakitKatsayi;
            ViewBag.vadeliKatsayi = vadeliKatsayi;

            var FixListe = liste.Where(x => x.KalipId != 0).ToList();
            return PartialView(FixListe);
        }

        public PartialViewResult MaliyetHesapTeklifTutar(List<MaliyetDto> liste)
        {
            var doviz = Db.DovizKur.FirstOrDefault();
            ViewBag.dolarKur = doviz.UsdKur;
            ViewBag.euroKur = doviz.EurKur;

            var FixListe = liste.Where(x => x.KalipId != 0).ToList();
            return PartialView(FixListe);
        }

        [HttpGet]
        public JsonResult KurGetir(string kisaltma)
        {
            Response<decimal> response = new Response<decimal>();
            try
            {
                var doviz = Db.DovizKur.FirstOrDefault();
                decimal Kur = 0;
                if (kisaltma.ToLower() == "usd")
                {
                    Kur = doviz.UsdKur;
                }

                if (kisaltma.ToLower() == "eur")
                {
                    Kur = doviz.EurKur;
                }               
                
                response.Success = true;
                response.Data = Kur;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult MaliyetDetay(string MaliyetType, int KalipId, int urunId, string PosetParametre = "0.060")
        {
            var Kalip = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .FirstOrDefault(x => x.KalipId == KalipId);

            ViewBag.MaliyetType = MaliyetType;
            //ViewBag.TozBoyaSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("toz")).BirimFiyat;
            //ViewBag.BaskiMalzemeler = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).Where(x => x.HammaddeCinsiId == 6632).ToList();
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("poşet")).BirimFiyat;
            ViewBag.KoliSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı")).BirimFiyat;
            ViewBag.HamMaddeHareket = Db.HammaddeHareket.ToList();
            ViewBag.KoliSonHareket = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            ViewBag.PosetParametre = PosetParametre;
            ViewBag.urunId = urunId;
            return PartialView(Kalip);

        }

        public PartialViewResult FormTdKart(dynamic ModelItem)
        {
            return PartialView(ModelItem);
        }
        public JsonResult GetById(int siparisId)
        {
            var siparis = siparisRepo.Get(siparisId);
            if (siparis != null)
            {
                var json = JsonConvert.SerializeObject(siparis, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Json(json);
            }
            return Json(new { Aciklama = "Veriler yüklenemedi!!" });
        }

        public JsonResult AcKapat(int siparisId)
        {
            Response response = new Response();
            try
            {
                var siparis = siparisRepo.Get(siparisId);
                if (siparis.SiparisDurumId == (int)ESiparisType.Tamamlandi || siparis.SiparisDurumId == (int)ESiparisType.SiparisAlindi)
                {
                    siparis.SiparisDurumId = (int)ESiparisType.Uretimde;
                    response.Description = "Sipariş üretimde olarak işaretlendi";
                }
                else
                {
                    siparis.SiparisDurumId = (int)ESiparisType.Tamamlandi;
                    response.Description = "Sipariş tamamlandı olarak işaretlendi";
                }

                siparisRepo.Update(siparis);
                response.Success = true;

                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response);
            }
        }


        public FileResult SiparisDetayPdf(int siparisId)
        {
            var siparis = siparisRepo.Get(siparisId);
            var bId = siparis.Cari.Unvan + "_detay_" + siparisId + ".pdf";
            var vPath = "~/Content/SiparisPdf2/" + bId;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
            string url = baseUrl + "/Pdf/SiparisDetay?siparisId=" + siparisId;
            //string url = "https://localhost:44381/Pdf/SiparisDetay?siparisId=" + siparisId;

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            doc.Save(HostingEnvironment.MapPath(vPath));

            // close pdf document
            doc.Close();

            return File(vPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(bId));

        }

        public FileResult SiparisUretimDetayPdf(int siparisId)
        {
            var siparis = siparisRepo.Get(siparisId);
            var bId = siparis.Cari.Unvan + "_uretim_" + siparisId + ".pdf";
            var vPath = "~/Content/UretimTakip/" + bId;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
            string url = baseUrl + "/Pdf/SiparisUretimDetay?siparisId=" + siparisId;
            //string url = "https://localhost:44381/Pdf/SiparisUretimDetay?siparisId=" + siparisId;

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            doc.Save(HostingEnvironment.MapPath(vPath));

            // close pdf document
            doc.Close();

            return File(vPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(bId));

        }

        private List<Kalip> FindKalipInParcaKodList(List<string> liste)
        {
            var kaliplar = kalipRepo.GetAll();
            List<Kalip> KalipList = new List<Kalip>();
            foreach (var parcaKodu in liste)
            {
                var kalip = kaliplar.FirstOrDefault(x => x.ParcaKodu == parcaKodu);
                if (kalip != null)
                {
                    KalipList.Add(kalip);
                }
            }
            return KalipList;
        }
    }
}