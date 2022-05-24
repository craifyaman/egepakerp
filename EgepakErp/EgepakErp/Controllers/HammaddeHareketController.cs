using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgepakErp.Controllers
{
    public class HammaddeHareketController:BaseController
    {
        // GET: Cari
        [Menu("Hammadde Hareket", "flaticon-customer icon-xl", "Hammade Hreket", 1, 4)]
        public ActionResult Index()
        {
            return View();
        }


        [Yetki("Hammadde Hareket", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "HammaddeHareketId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.HammaddeHareket.AsQueryable();


            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("HammaddeHareketId Desc");
                dtMeta.field = "HammaddeHareketId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                HammaddeHareketId = i.HammaddeHareketId,
                FaturaNo = i.FaturaNo,
                UrunAdi = i.UrunAdi
            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);
        }

        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            if (id != 0)
            {
                var model = Db.HammaddeCinsi

                    .FirstOrDefault(i => i.HammaddeCinsiId == id);

                return PartialView(model);
            }

            return PartialView(new HammaddeCinsi());
        }


        [Yetki("Hammadde Hareket Kaydet", "Üretim")]
        public JsonResult Kaydet(HammaddeCinsi form)
        {
            var response = new Response();

            try
            {
                if (form.HammaddeCinsiId == 0)
                {

                    Db.HammaddeCinsi.Add(form);
                }
                else
                {
                    var entity = Db.HammaddeCinsi
                        .FirstOrDefault(i => i.HammaddeCinsiId == form.HammaddeCinsiId);

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