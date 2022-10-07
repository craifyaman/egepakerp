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
            var model2 = model.ToList();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.UretimEmirId.ToString() == searchQuery.ToLower()
                );
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
                Baslangic = x.Baslangic.ToString("dd MMMM yyyy HH:mm"),
                Bitis = x.Bitis.ToString("dd MMMM yyyy HH:mm"),
                Durum = x.UretimEmirDurum.Durum,
                //UretilenAdet = x.UretilenAdet,
                SiparisAdet = x.SiparisAdet,
                //KalanAdet = x.KalanAdet,
                ClassList = x.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi ? "bg-success" : "bg-warning",

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
                var _siparisKalip = siparisKalipRepo.Get(form.SiparisKalipId);
                var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
                var _gozSayisi = _kalip.KalipGozSayisi;

                var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
                var fireliAdet = form.SiparisAdet + (form.SiparisAdet * fireOrani / 100);

                var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
                form.Bitis = form.Baslangic.AddMinutes(bitisZaman / 60);
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
                    var _siparisKalip = siparisKalipRepo.Get(form.SiparisKalipId);
                    var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
                    var _gozSayisi = _kalip.KalipGozSayisi;

                    var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
                    var fireliAdet = form.SiparisAdet + (form.SiparisAdet * fireOrani / 100);

                    var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
                    form.Bitis = form.Baslangic.AddMinutes(bitisZaman / 60);
                    entity.Bitis = form.Bitis;
                    //tamamlanma tarihi kaydetme
                    if (form.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi)
                    {
                        entity.TamamlanmaTarih = DateTime.Now;
                    }

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

        public PartialViewResult SiparisKalipBySiparis(int siparisId)
        {
            SiparisKalipRepository siparisKalipRepo = new SiparisKalipRepository();
            var kaliplar = siparisKalipRepo.GetAll(x => x.SiparisId == siparisId);
            return PartialView(kaliplar.ToList());
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
                var model = repo.GetAll();

                if (type == "enjeksiyon")
                {
                    //EUretimEmirDurum.Uretimde
                    model = model.Where(x => x.UretimEmirDurumList.Contains("1"));
                }

                else if (type == "sicakbaski")
                {
                    //EUretimEmirDurum.SicakBaski
                    model = model.Where(x => x.UretimEmirDurumList.Contains("2"));
                }
                else if (type == "sprey")
                {
                    //EUretimEmirDurum.Sprey
                    model = model.Where(x => x.UretimEmirDurumList.Contains("3"));
                }
                else if (type == "montaj")
                {
                    //EUretimEmirDurum.Montaj
                    model = model.Where(x => x.UretimEmirDurumList.Contains("4"));
                }
                else if (type == "metalize")
                {
                    //EUretimEmirDurum.Metalize
                    model = model.Where(x => x.UretimEmirDurumList.Contains("5"));
                }

                //var yaldiz = BaseSiparisKalipListeQ().FirstOrDefault(x => x.KalipKod == Model.SiparisKalip.KalipKod && x.YaldizId != null);
                //var boyaKod = BaseSiparisKalipListeQ().FirstOrDefault(x => x.KalipKod == Model.SiparisKalip.KalipKod && x.BoyaKodId != null);

                var dto = model.AsEnumerable().Select(x =>
                {
                    var cariId = x.SiparisKalip.Siparis.CariId;
                    var cari = cariList.FirstOrDefault(a => a.CariId == cariId).Unvan;
                    var fixSiparisKaliplar = siparisKaliplar.Where(a => a.SiparisId == x.SiparisKalip.SiparisId);
                    dynamic ret = new
                    {
                        UretimEmirId = x.UretimEmirId,
                        SiparisKalip = x.SiparisKalip.KalipKod,
                        KalipAd = "<b>"+x.SiparisKalip.EnjeksiyonRenk +" "+ kaliplar.FirstOrDefault(c => c.ParcaKodu == x.SiparisKalip.KalipKod).Adi
                        + "</b>" + " ( " + cari.Substring(0,10) + " ) "
                        + " _ (" + x.SiparisKalip.Siparis.SiparisAdi + ")"
                        
                        //+ fixSiparisKaliplar.FirstOrDefault(m => m.KalipKod == x.SiparisKalip.KalipKod && m.YaldizKodList != null)?.YaldizKodList
                        //+ " _ "
                        //+ fixSiparisKaliplar.FirstOrDefault(m => m.KalipKod == x.SiparisKalip.KalipKod && m.SpreyBoyaKodId != null)?.SpreyBoyaKod.Aciklama
                        //+ "_"
                        //+ fixSiparisKaliplar.FirstOrDefault(m => m.KalipKod == x.SiparisKalip.KalipKod && m.MetalizeKodId != null)?.MetalizeKod.Aciklama
                        ,
                        Makine = x.Makine.MakineAd,
                        MakineId = x.Makine.MakineId,
                        Baslangic = x.Baslangic,
                        Bitis = x.Bitis,
                        Durum = x.UretimEmirDurum?.Durum,
                        //UretilenAdet = x.UretilenAdet,
                        SiparisAdet = x.SiparisAdet,
                        //KalanAdet = x.KalanAdet,
                        SiparisId = x.SiparisKalip.SiparisId,
                        SiparisAdi = x.SiparisKalip.Siparis?.SiparisAdi,
                        ClassList = x.UretimEmirDurumId == (int)EUretimEmirDurum.Tamamlandi ? "bg-success" : "bg-secondary",
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

    }
}