using EgePakErp.Concrete;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.Enums;
using EgePakErp.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Data.Entity;


namespace EgePakErp.Controllers
{
    public class UretimEmirController : BaseController
    {
        public UretimEmirRepository repo { get; set; }
        public SiparisKalipRepository siparisKalipRepo { get; set; }
        public YaldizRepository yaldizRepo { get; set; }
        public BoyaKodRepository boyaKodRepo { get; set; }
        public CariRepository cariRepo { get; set; }
        public KalipRepository kalipRepo { get; set; }
        public UretimEmirController()
        {
            repo = new UretimEmirRepository();
            siparisKalipRepo = new SiparisKalipRepository();
            yaldizRepo = new YaldizRepository();
            boyaKodRepo = new BoyaKodRepository();
            cariRepo = new CariRepository();
            kalipRepo = new KalipRepository();

        }

        [Menu("Üretim Emirleri", "flaticon2-menu-2 icon-xl", "Üretim", 5, 1)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Üretim Emir Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UretimEmirId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.UretimEmirId.ToString() == searchQuery.ToLower()
                );
            }
            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[urunId]"]))
            {
                var urunId = Convert.ToInt32(Request.Form["query[urunId]"].ToString());
                if(urunId > 0)
                {
                    model = model.Where(i => i.Siparis.UrunId == urunId);
                }
                
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[SiparisAdi]"]))
            {
                var siparisAdi = Request.Form["query[SiparisAdi]"].ToString();
                model = model.Where(i => i.Siparis.SiparisIsim.Contains(siparisAdi));
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[ParcaAdi]"]))
            {
                var ParcaAdi = Request.Form["query[ParcaAdi]"].ToString();
                model = model.Where(i => i.SiparisKalip.EnjeksiyonRenk.Contains(ParcaAdi));
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[Adet]"]))
            {
                var Adet = Convert.ToInt32(Request.Form["query[Adet]"].ToString());
                model = model.Where(i => i.SiparisAdet== Adet);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("UretimEmirId Desc");
                dtMeta.field = "UretimEmirId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            var dto = model.AsEnumerable().Select(x => new
            {
                UretimEmirId = x.UretimEmirId,
                SiparisKalip = x.SiparisKalip.KalipKod,
                Makine = x.Makine.MakineAd,
                MakineId = x.Makine.MakineId,
                Baslangic = x.Baslangic?.ToString("dd MMMM yyyy HH:mm"),
                Bitis = x.Bitis?.ToString("dd MMMM yyyy HH:mm"),
                Parca = x.SiparisKalip.EnjeksiyonRenk,
                Siparis = x.Siparis.SiparisIsim,
                Urun = x.Siparis.Urun.UrunCinsi.Kisaltmasi + " " + x.Siparis.Urun.UrunNo,
                SiparisAdet = x.SiparisAdet,
                //ClassList = x.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi ? "bg-success" : "bg-warning",
                ClassList = "bg-warning",

            }).ToList();


            dtModel.meta = dtMeta;
            dtModel.data = dto.ToList<dynamic>();
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);
            return PartialView(model);
        }
        public PartialViewResult UretimEmirAksiyonForm(int UretimEmirId, int UretimEmirAksiyonTypeId)
        {
            ViewBag.UretimEmirId = UretimEmirId;
            ViewBag.UretimEmirAksiyonTypeId = UretimEmirAksiyonTypeId;
            return PartialView();
        }

        public PartialViewResult UretimEmirAksiyonDuzenle(int id)
        {
            var model = Db.UretimEmirAksiyon.Find(id);
            return PartialView(model);
        }

        public PartialViewResult UretimEmirAksiyonListe(int UretimEmirId, int UretimEmirAksiyonTypeId)
        {
            var model = Db.UretimEmirAksiyon
                .Include(x => x.Calisan)
                .Include(x => x.UretimEmirAksiyonType)
                .Where(x => x.UretimEmirId == UretimEmirId && x.UretimEmirAksiyonTypeId == UretimEmirAksiyonTypeId).ToList();
            return PartialView(model);
        }



        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Üretim Emir Kaydet", "Üretim")]
        public JsonResult Kaydet(UretimEmir form)
        {
            var response = new Response();

            if (form.UretimEmirId == 0)
            {
                if (form.SicakBaskiYapilacak)
                {
                    form.isSicakBaskiBitti = false;
                }
                if (form.SpreyYapilacak)
                {
                    form.isSpreyBoyaBitti = false;
                }
                if (form.MetalizeYapilacak)
                {
                    form.isMetalizeBitti = false;
                }
                if (form.MontajYapilacak)
                {
                    form.isMontajBitti = false;
                }
                if (form.EvMontajYapilacak)
                {
                    form.isEvMontajBitti = false;
                }

                //tahmini bitiş zamanı hesaplama 
                //var _siparisKalip = siparisKalipRepo.Get(form.SiparisKalipId);
                //var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
                //var _gozSayisi = _kalip.KalipGozSayisi;

                //var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
                //var fireliAdet = form.SiparisAdet + (form.SiparisAdet * fireOrani / 100);

                //var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
                //form.Bitis = form.Baslangic.AddMinutes(bitisZaman / 60);


                repo.Insert(form);

                var sk = siparisKalipRepo.Get(form.SiparisKalipId);
                if (sk != null)
                {
                    sk.UretimBasladiMi = true;
                    siparisKalipRepo.Update(sk);
                }

                response.Success = true;
                response.Description = "Kayıt edildi.";
            }
            else
            {
                var entity = repo.Get(form.UretimEmirId);
                if (entity != null)
                {
                    //tahmini bitiş zamanı hesaplama 
                    //var _siparisKalip = siparisKalipRepo.Get(form.SiparisKalipId);
                    //var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
                    //var _gozSayisi = _kalip.KalipGozSayisi;

                    //var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
                    //var fireliAdet = form.SiparisAdet + (form.SiparisAdet * fireOrani / 100);

                    //var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
                    //form.Bitis = form.Baslangic.AddMinutes(bitisZaman / 60);
                    //entity.Bitis = form.Bitis;
                    //tamamlanma tarihi kaydetme
                    //if (form.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi)
                    //{
                    //    entity.TamamlanmaTarih = DateTime.Now;
                    //}

                    //alanları güncelle
                    var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                    foreach (var prop in propList)
                    {
                        if (form.Include.Contains(prop.Name))
                        {
                            prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                        }
                    }
                    repo.Update(entity);
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }

            return Json(response);

        }



        public JsonResult UretimEmirAksiyonKaydet(UretimEmirAksiyon form)
        {
            var response = new Response();
            var uretimEmirAksiyonRepo = new UretimEmirAksiyonRepository();
            if (form.UretimEmirAksiyonId == 0)
            {
                form.KayitTarih = DateTime.Now;
                uretimEmirAksiyonRepo.Insert(form);
                response.Success = true;
                response.Description = "Kayıt edildi.";
            }
            else
            {
                var entity = uretimEmirAksiyonRepo.Get(form.UretimEmirAksiyonId);
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
                    uretimEmirAksiyonRepo.Update(entity);
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }
            return Json(response);

        }

        public PartialViewResult SiparisKalipBySiparis(int siparisId)
        {
            SiparisKalipRepository siparisKalipRepo = new SiparisKalipRepository();
            var kaliplar = siparisKalipRepo.GetAll(x => x.SiparisId == siparisId);
            return PartialView(kaliplar.ToList());
        }

        public string BitisTarihHesapla(int siparisAdet, int siparisKalipId)
        {
            //tahmini bitiş zamanı hesaplama 
            var _siparisKalip = siparisKalipRepo.Get(siparisKalipId);
            var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
            var _gozSayisi = _kalip.KalipGozSayisi;

            var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
            var fireliAdet = siparisAdet + (siparisAdet * fireOrani / 100);

            var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
            double bitis = bitisZaman / 60 / 60; //dakika cinsinden bitiş süresi
            return "bitiş süresi " + bitis.ToString("n1") + " saat";
        }

        public JsonResult GetAll(string type)
        {
            var yaldizList = yaldizRepo.GetAll();
            var boyaKodList = boyaKodRepo.GetAll();
            var siparisKaliplar = siparisKalipRepo.GetAll(x => x.YaldizKodList != null || x.TozBoyaKodId != null || x.MetalizeKodId != null || x.TozBoyaKodList != null);
            var response = new Response<dynamic>();
            try
            {
                var kaliprepo = new KalipRepository();
                var kaliplar = kaliprepo.GetAll();
                var cariList = cariRepo.GetAll();
                var model = repo.GetAll(x => x.Siparis.SiparisDurumId == (int)ESiparisType.Uretimde);

                var dto = model.AsEnumerable().Select(x =>
                {
                    var cariId = x.SiparisKalip.Siparis.CariId;
                    var cari = cariList.FirstOrDefault(a => a.CariId == cariId).Unvan;
                    var fixSiparisKaliplar = siparisKaliplar.Where(a => a.SiparisId == x.SiparisKalip.SiparisId);
                    dynamic ret = new
                    {
                        UretimEmirId = x.UretimEmirId,
                        SiparisKalip = x.SiparisKalip.KalipKod,
                        KalipAd = "<b>" + x.Siparis.Urun.UrunCinsi.Kisaltmasi + x.Siparis.Urun.UrunNo + " " + x.SiparisKalip.EnjeksiyonRenk
                        + "</b>" + " ( " + cari.Substring(0, 10) + " ) "
                        + " _ (" + x.SiparisKalip.Siparis.SiparisKod + ")",
                        Makine = x.Makine.MakineAd,
                        MakineId = x.Makine.MakineId,
                        Baslangic = x.Baslangic,
                        Bitis = x.Bitis,
                        //Durum = x.UretimEmirDurum?.Durum,
                        SiparisAdet = x.SiparisAdet,
                        SiparisId = x.SiparisKalip.SiparisId,
                        SiparisKod = x.SiparisKalip.Siparis?.SiparisKod,
                        //ClassList = x.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi ? "bg-success" : "bg-secondary",
                        ClassList = "bg-secondary",
                    };
                    return ret;

                }).OrderBy(x => x.UretimEmirId).ToList();

                response.Success = true;
                response.Description = "veriler alındı";
                response.Data = dto;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
            }

            return Json(response);

        }

        [HttpPost]
        [Yetki("Üretim Emri Silme", "Uretim Emir")]
        public JsonResult Sil(int id)
        {
            var response = new Response();
            var emir = repo.Get(id);
            repo.Delete(emir);
            response.Success = true;
            response.Description = "Emir Silindi";
            return Json(response);

        }


        [HttpPost]
        [Yetki("Üretim Emri Aksiyon Silme", "Uretim Emir")]
        public JsonResult UretimEmirAksiyonSil(int id)
        {
            var response = new Response();
            var model = Db.UretimEmirAksiyon.Find(id);
            Db.UretimEmirAksiyon.Remove(model);
            Db.SaveChanges();
            response.Success = true;
            response.Description = "Kayıt Silindi";
            return Json(response);
        }
    }
}