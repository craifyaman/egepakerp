using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{

    public class KalipController : BaseController
    {
        // GET: Cari
        [Menu("Kalıplar", "flaticon2-download icon-xl", "Üretim", 0, 4)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Kalıp Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "KalıpId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Kalip
                .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .AsQueryable();


            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.KalipNo.ToLower().Contains(searchQuery.ToLower())||
                i.KalipOzellik.ToLower().Contains(searchQuery.ToLower()) ||
                i.Adi.ToLower().Contains(searchQuery.ToLower()) ||
                string.Concat(i.KalipNo.ToLower()+i.KalipOzellik).Contains(searchQuery.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[uretimTeminSekliId]"]))
            {
                var uretimTeminSekliId = Convert.ToInt32(Request.Form["query[uretimTeminSekliId]"]);
                model = model.Where(i => i.UretimTeminSekliId == uretimTeminSekliId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("KalipId Desc");
                dtMeta.field = "UrunId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                KalipId=i.KalipId,
                KalipKodu = string.Concat(i.KalipNo+"-"+ i.KalipOzellik),
                Adi = i.Adi,
                UretimTeminSekli=i.UretimTeminSekli?.Adi,
                Agirlik=i.ParcaAgirlik,
                GozSayisi=i.KalipGozSayisi,
                UretimZamani=i.UretimZamani,
                Hammadde=i.KalipHammaddeRelation.Select(s=>s.HammaddeCinsi).Select(s=>s.Adi),
                Urun=i.KalipUrunRelation.Select(s=>s.Urun).Select(s=>s.UrunCinsi.Kisaltmasi+s.UrunNo)

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Kalip
                 .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .FirstOrDefault(i => i.KalipId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Kalıp Kaydet", "Üretim")]
        public JsonResult Kaydet(Kalip form)
        {
            var response = new Response();

            try
            {
                if (form.KalipId == 0)
                {

                    form.KalipHammaddeRelation= form.HammaddeList.Select(s => new KalipHammaddeRelation { HammaddeCinsiId = s }).ToList();
                    form.KalipUrunRelation=form.UrunList.Select(s => new KalipUrunRelation { UrunId = s }).ToList();
                    Db.Kalip.Add(form);
                }
                else
                {
                    var entity = Db.Kalip
                        .Include("KalipHammaddeRelation")
                        .Include("KalipUrunRelation")
                        .FirstOrDefault(i=>i.KalipId ==form.KalipId);
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

                        //relationları güncelle
                        Db.KalipHammaddeRelation.RemoveRange(Db.KalipHammaddeRelation.Where(i=>i.KalipId==form.KalipId));
                        Db.KalipUrunRelation.RemoveRange(Db.KalipUrunRelation.Where(i => i.KalipId == form.KalipId));
                        if (form.HammaddeList!=null)
                        {
                            Db.KalipHammaddeRelation.AddRange(form.HammaddeList.Select(s => new KalipHammaddeRelation { KalipId = entity.KalipId, HammaddeCinsiId = s }));
                        }
                        if (form.UrunList!=null)
                        {
                            Db.KalipUrunRelation.AddRange(form.UrunList.Select(s => new KalipUrunRelation { KalipId = entity.KalipId, UrunId = s }));
                        }
                        
                        
                    }

                    
                }
                Db.SaveChanges(CurrentUser.PersonelId);
                response.Success = true;
                response.Description = "İşlem Başarılı";
            }
            catch (Exception ex)
            {
                response.Success = true;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }



            return Json(response);

        }

        public JsonResult ListeSelect2(string q)
        {
            //kabasını aldır
            var model = Db.Kalip
                .AsQueryable();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(
                    x => x.KalipNo.Contains(q) ||
                    x.Adi.Contains(q) ||
                    x.Aciklama.Contains(q)
                    );
            }
            model = model.OrderBy("KalipId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.KalipId,
                    text = string.Concat(i.KalipNo +" "+i.Adi),
                    markup = "urun"

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Sil(int KalipId)
        {
            Response response = new Response();
            var kalip = Db.Kalip.Find(KalipId);
            try
            {
                Db.Kalip.Remove(kalip);
                Db.SaveChanges();
                response.Success = true;
                response.Description = "kalıp silindi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        public void KalipAgirlikDuzelt()
        {
            var list = Db.Kalip.ToList();
            foreach(var kalip in list)
            {
                kalip.ParcaAgirlik *= 10;
            }
            Db.SaveChanges();
        }

    }
}