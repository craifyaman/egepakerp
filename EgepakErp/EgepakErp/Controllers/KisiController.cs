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

    public class KisiController : BaseController
    {
        // GET: Cari
        [Menu("Kişi Listesi", "flaticon-users icon-xl", "Müşteri", 2, 2)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Kişi Listesi", "Kişi")]
        public JsonResult Liste(int? cariId)
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "KisiId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Kisi
                .Include("Cari")
                .Include("Brans")
                .AsQueryable();

            if (cariId != null)
            {
                model = model.Where(i => i.CariId == cariId);
            }

            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;



            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.AdSoyad.ToLower().Contains(searchQuery.ToLower()) ||
                i.Cari.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
                i.Görev.ToLower().Contains(searchQuery.ToLower()) ||
                i.Eposta.ToLower().Contains(searchQuery.ToLower()) ||
                i.Eposta2.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon2.ToLower().Contains(searchQuery.ToLower())

                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[durum]"]))
            {
                var durum = Request.Form["query[durum]"] == "Aktif";
                model = model.Where(i => i.Aktif == durum);
            }

            

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("KisiId Desc");
                dtMeta.field = "KisiId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.Select(i => new
            {
                KisiId = i.KisiId,
                CariUnvan = i.Cari.Unvan,
                AdSoyad = i.AdSoyad,
                Gorev = i.Görev,
                Eposta = i.Eposta,
                Eposta2 = i.Eposta2,
                Telefon = i.Telefon,
                Telefon2 = i.Telefon2,
                Aktif = i.Aktif ? "Aktif" : "Pasif",
                Birincil = i.Birincil ? "Biricil" : "-",
            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }

        public JsonResult ListeSelect2(string q)
        {
            //kabasını aldır
            var model = Db.Kisi
                .Include("Cari")
                .Include("Brans")
                .AsQueryable();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(i =>
                i.AdSoyad.ToLower().Contains(searchQuery.ToLower()) ||
                i.Cari.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
                i.Görev.ToLower().Contains(searchQuery.ToLower()) ||
                i.Eposta.ToLower().Contains(searchQuery.ToLower()) ||
                i.Eposta2.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon.ToLower().Contains(searchQuery.ToLower()) ||
                i.Telefon2.ToLower().Contains(searchQuery.ToLower())

                );
            }

            model = model.OrderBy("KisiId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.KisiId,
                    text = i.AdSoyad,
                    cari = i.Cari.Unvan,
                    markup = "kisi"

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

        [Yetki("Kişi Durum Güncelle", "Kişi")]
        public JsonResult DurumGuncelle(int id)
        {
            var response = new Response();
            var Kisi = Db.Kisi.Find(id);

            Kisi.Aktif = !Kisi.Aktif;
            Db.SaveChanges(CurrentUser.PersonelId);

            response.Success = true;
            response.Description = "Kişi Durum Güncellendi";
            return Json(response);
        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Kisi.FirstOrDefault(i => i.KisiId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Kişi Kaydet", "Kişi")]
        public JsonResult Kaydet(Kisi kisi)
        {
            var response = new Response();

            try
            {
                if (kisi.KisiId == 0)
                {
                    kisi.PersonelId = CurrentUser.PersonelId;
                    kisi.KayitTarihi = DateTime.Now;
                    Db.Kisi.Add(kisi);
                }
                else
                {
                    var entity = Db.Kisi.Find(kisi.KisiId);
                    if (entity != null)
                    {
                        foreach (var prop in entity.GetType().GetProperties())
                        {
                            if (kisi.Include.Contains(prop.Name))
                            {
                                prop.SetValue(entity, kisi.GetType().GetProperty(prop.Name).GetValue(kisi, null));
                            }
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

    }
}