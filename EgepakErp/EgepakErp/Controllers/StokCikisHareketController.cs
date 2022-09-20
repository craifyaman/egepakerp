using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class StokCikisHareketController : BaseController
    {
        public StokCikisHareketRepository repo { get; set; }
        public string dtMetaField { get; set; }

        public StokCikisHareketController()
        {
            repo = new StokCikisHareketRepository();


            dtMetaField = "StokCikisHareketId";
        }

        [Menu("Çıkış Hareketleri", "fa-solid fa-industry icon-xl", "Depo", 0, 5)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? dtMetaField : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                string q = Request.Form["query[searchQuery]"].ToString();
                model = model
                    .Where(
                    x => x.Cari.Unvan.Contains(q)
                    );
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy(dtMetaField + " Desc");
                dtMeta.field = dtMetaField;
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;


            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);
            var dto = model.Select(x => new
            {
                Id = x.StokCikisHareketId,
                Adet = x.Adet,
                CikisTarih = x.CikisTarih,
                StokHareketId = x.StokHareketId,
                Cari = x.Cari.Unvan,
                Aciklama = x.Aciklama
            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }


        public PartialViewResult Form(int? id, int stokHareketId,int cariId)
        {
            ViewBag.StokHareketId = stokHareketId;
            ViewBag.CariId = cariId;
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);

            return PartialView(model);
        }
        public PartialViewResult GetListById(int stokHareketId)
        {
            var model = repo.GetAll(x=>x.StokHareketId == stokHareketId);
            return PartialView(model);
        }
        
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        public JsonResult Kaydet(StokCikisHareket form)
        {
            var response = new Response();

            try
            {
                if (form.StokCikisHareketId == 0)
                {
                    form.CikisTarih = DateTime.Now;
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = repo.Get(form.StokCikisHareketId);
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