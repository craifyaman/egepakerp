﻿using System;
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

    public class UrunController : BaseController
    {
        // GET: Cari
        [Menu("Ürünler", "flaticon2-list-2 icon-xl", "Üretim", 0, 4)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Ürün Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UrunId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Urun
                .Include("UrunCinsi")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Kalip")
                .AsQueryable();


            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.UrunCinsi.Aciklamasi.ToLower().Contains(searchQuery.ToLower()) ||
                i.UrunCinsi.Kisaltmasi.ToLower().Contains(searchQuery.ToLower()) ||
                i.UrunCinsi.Aciklamasi.ToLower().Contains(searchQuery.ToLower()) ||
                string.Concat(i.UrunCinsi.Kisaltmasi.ToLower() + i.UrunNo).Contains(searchQuery.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[urunCinsiId]"]))
            {
                var urunCinsiId = Convert.ToInt32(Request.Form["query[urunCinsiId]"]);
                model = model.Where(i => i.UrunCinsiId == urunCinsiId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("UrunId Desc");
                dtMeta.field = "UrunId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                UrunId = i.UrunId,
                UrunKodu = string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo),
                UrunCinsi = i.UrunCinsi.Adi,
                Kalip = i.KalipUrunRelation.Select(s => s.Kalip).Select(s => s.KalipNo + " " + s.KalipOzellik)

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Urun
                .Include("UrunCinsi")
                .FirstOrDefault(i => i.UrunId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Ürün Kaydet", "Üretim")]
        public JsonResult Kaydet(Urun form)
        {
            var response = new Response();

            try
            {
                if (form.UrunId == 0)
                {
                    form.KalipUrunRelation = form.KalipList.Select(s => new KalipUrunRelation { KalipId = s }).ToList();
                    Db.Urun.Add(form);
                }
                else
                {
                    var entity = Db.Urun.Find(form.UrunId);
                    if (entity != null)
                    {
                        var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                        foreach (var prop in propList)
                        {
                            if (form.Include.Contains(prop.Name))
                            {
                                prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                            }
                        }


                        Db.KalipUrunRelation.RemoveRange(Db.KalipUrunRelation.Where(i => i.UrunId == form.UrunId));
                        if (form.KalipList != null)
                        {
                            Db.KalipUrunRelation.AddRange(form.KalipList.Select(s => new KalipUrunRelation { KalipId = s, UrunId = entity.UrunId }));
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
            var model = Db.Urun
                .Include("UrunCinsi")
                .AsQueryable();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(i =>
                string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo).Contains(searchQuery.ToLower()) ||
                i.UrunCinsi.Kisaltmasi.ToLower().Contains(searchQuery.ToLower())
                );
            }

            model = model.OrderBy("UrunId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.UrunId,
                    text = string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo),
                    markup = "urun"

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

    }
}