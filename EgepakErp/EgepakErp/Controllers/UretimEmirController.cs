using EgePakErp.Concrete;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class UretimEmirController : BaseController
    {
        public UretimEmirRepository repo { get; set; }
        public UretimEmirController()
        {
            repo = new UretimEmirRepository();
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
                i.Durum.ToLower().Contains(searchQuery.ToLower())
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
                Durum = x.Durum,
                UretilenAdet = x.UretilenAdet,
                SiparisAdet = x.SiparisAdet,
                KalanAdet = x.KalanAdet

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

            try
            {
                if (form.UretimEmirId == 0)
                {
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = repo.Get(form.UretimEmirId);
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
                        repo.Update(entity);
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

        public PartialViewResult SiparisKalipBySiparis(int siparisId)
        {
            SiparisKalipRepository siparisKalipRepo = new SiparisKalipRepository();
            var kaliplar = siparisKalipRepo.GetAll(x => x.SiparisId == siparisId);
            return PartialView(kaliplar.ToList());
        }


        public JsonResult GetAll()
        {
            var response = new Response<dynamic>();
            try
            {
                var kaliprepo = new KalipRepository();
                var kaliplar = kaliprepo.GetAll();

                var model = repo.GetAll();

                var dto = model.AsEnumerable().Select(x => new
                {
                    UretimEmirId = x.UretimEmirId,
                    SiparisKalip = x.SiparisKalip.KalipKod,
                    KalipAd = kaliplar.FirstOrDefault(c=>c.ParcaKodu == x.SiparisKalip.KalipKod).Adi,
                    Makine = x.Makine.MakineAd,
                    MakineId = x.Makine.MakineId,
                    Baslangic = x.Baslangic,
                    Bitis = x.Bitis,
                    Durum = x.Durum,
                    UretilenAdet = x.UretilenAdet,
                    SiparisAdet = x.SiparisAdet,
                    KalanAdet = x.KalanAdet

                }).ToList();

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