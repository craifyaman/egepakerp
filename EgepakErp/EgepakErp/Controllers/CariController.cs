using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class CariController : BaseController
    {
        // GET: Cari
        [Menu("Müşteriler", "flaticon-customer icon-xl", "Müşteri", 1, 2)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Müşteri Listesi", "Müşteri")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "CariId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Cari
                .Include("CariGrup")
                .Include("Kisi")
                .Include("Il")
                .Include("Il.Ilce")
                .AsQueryable();

            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i => 
                i.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
                i.Il.Adi.ToLower().Contains(searchQuery)||
                i.Eposta.ToLower().Contains(searchQuery)||
                i.WebSitesi.ToLower().Contains(searchQuery)||
                i.Telefon.ToLower().Contains(searchQuery)
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[durum]"]))
            {
                var durum = Request.Form["query[durum]"] == "Aktif";
                model = model.Where(i => i.Aktif == durum);
            }

            if (!string.IsNullOrEmpty(Request.Form["query[cariGrupId]"]))
            {
                var cariGrupId = Convert.ToInt32(Request.Form["query[cariGrupId]"]);
                model = model.Where(i => i.CariGrupId == cariGrupId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("CariId Desc");
                dtMeta.field = "CariId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.Select(i => new
            {
                CariId = i.CariId,
                Unvan = i.Unvan,
                BirincilKisi = i.Kisi.Count >= 1 ? i.Kisi.FirstOrDefault(f => f.Birincil).AdSoyad : "",
                BirincilKisiEposta = i.Kisi.Count >= 1 ? i.Kisi.FirstOrDefault(f => f.Birincil).Eposta : "",
                Ilce = i.IlceId != null ? i.Ilce.Adi : "",
                Il = i.IlId != null ? i.Il.Adi : "",
                Durum = i.Aktif ? "Aktif" : "Pasif"
            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public JsonResult ListeSelect2(string q)
        {
            //kabasını aldır
            var model = Db.Cari
                .AsQueryable();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(i =>
                i.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon.ToLower().Contains(searchQuery.ToLower()) ||
                i.Eposta.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon.ToLower().Contains(searchQuery.ToLower())  
                );
            }

            model = model.OrderBy("CariId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.CariId,
                    text = i.Unvan,
                    markup = ""

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

        [Yetki("Müşteri Durum Güncelle", "Müşteri")]
        public JsonResult DurumGuncelle(int id)
        {
            var response = new Response();
            var cari = Db.Cari.Find(id);

            cari.Aktif = !cari.Aktif;
            Db.SaveChanges();

            response.Success = true;
            response.Description = "Cari Durum Güncellendi";
            return Json(response);
        }
        public JsonResult Il()
        {
            return Json(Db.Il.Select(i=> new { Adi=i.Adi}),JsonRequestBehavior.AllowGet);
        }
        public JsonResult CariList(string q, int page = 1)
        {
            IQueryable<Cari> model = null;
            model = Db.Cari.AsQueryable();
            
            var totalCount = model.Count();
            model = model.Where(i => i.Unvan.ToLower().Contains(q.ToLower()));
            //model = model.Skip((page - 1) * 10).Take(10);

            var ret = new
            {
                total_count = totalCount,
                items = model.Select(s => new { id = s.CariId, Value = s.Unvan })
            };

            return Json(ret, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Cari.FirstOrDefault(i => i.CariId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            
            return PartialView();
        }

        [Yetki("Müşteri Kaydet", "Müşteri")]
        public JsonResult Kaydet(Cari cari)
        {
            var response = new Response();

            if (cari.CariId==0)
            {
                cari.Aktif = true;
                Db.Cari.Add(cari);
            }
            else
            {
                var entity = Db.Cari.Find(cari.CariId);
                if (entity != null)
                {
                    foreach (var prop in entity.GetType().GetProperties())
                    {
                        if (cari.Include.Contains(prop.Name))
                        {
                            prop.SetValue(entity, cari.GetType().GetProperty(prop.Name).GetValue(cari, null));
                        }
                    }

                }
            }
            Db.SaveChanges(CurrentUser.PersonelId);

            response.Success = true;
            response.Description = "İşlem Başarılı";

            return Json(response);

        }
        public ActionResult Detay(int id)
        {
            return View(Db.Cari.Find(id));
        }
    }
}