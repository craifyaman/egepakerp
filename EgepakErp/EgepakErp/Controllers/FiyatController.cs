﻿using EgePakErp.Concrete;
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
    public class FiyatController : BaseController
    {
        public FiyatRepository repo { get; set; }
        public FiyatController()
        {
            repo = new FiyatRepository();
        }
        // GET: Fiyat
        [Menu("Fiyatlar", "fas fa-money-bill icon-xl", "Fiyatlar", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }
        [Yetki("Fiyat Listesi", "Fiyatlar")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "FiyatId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.Aciklama.ToLower().Contains(searchQuery.ToLower()) ||
                i.Kod.ToLower().Contains(searchQuery.ToLower())
                );
            }


            //if (!string.IsNullOrEmpty(Request.Form["query[urunCinsiId]"]))
            //{
            //    var urunCinsiId = Convert.ToInt32(Request.Form["query[urunCinsiId]"]);
            //    model = model.Where(i => i.UrunCinsiId == urunCinsiId);
            //}

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("FiyatId Desc");
                dtMeta.field = "FiyatId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);
            
            dtModel.meta = dtMeta;
            dtModel.data = model.ToList<dynamic>();
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

        [Yetki("Fiyat Kaydet", "Fiyatlar")]
        public JsonResult Kaydet(Fiyat form)
        {
            var response = new Response();

            try
            {
                if (form.FiyatId == 0)
                {
                    try
                    {
                        var kalip = Db.Kalip.FirstOrDefault(x => x.ParcaKodu == form.Kod);
                        kalip.isHazirMalzeme = true;
                        Db.SaveChanges();

                    }
                    catch { }
                    form.KayitTarih = DateTime.Now;
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = repo.Get(form.FiyatId);
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

    }
}