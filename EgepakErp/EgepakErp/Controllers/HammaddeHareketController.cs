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
        [Menu("Hammadde Hareket", "flaticon-customer icon-xl", "Üretim", 1, 2)]
        public ActionResult Index()
        {
            return View();
        }


        [Yetki("Hammadde Cinsi", "Üretim")]
        public JsonResult Liste()
        {
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "CariId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.HammaddeHaraket
                .Include("HammaddeTipi")
                .Include("Marka")
                .Include("HammaddeCinsi")
                .Include("Doviz")
                .AsQueryable();

            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            ////Filtre
            //if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            //{
            //    var searchQuery = Request.Form["query[searchQuery]"].ToString();
            //    model = model.Where(i =>
            //    i.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
            //    i.Il.Adi.ToLower().Contains(searchQuery) ||
            //    i.Eposta.ToLower().Contains(searchQuery) ||
            //    i.WebSitesi.ToLower().Contains(searchQuery) ||
            //    i.Telefon.ToLower().Contains(searchQuery)
            //    );
            //}

            //if (!string.IsNullOrEmpty(Request.Form["query[durum]"]))
            //{
            //    var durum = Request.Form["query[durum]"] == "Aktif";
            //    model = model.Where(i => i.Aktif == durum);
            //}

            //if (!string.IsNullOrEmpty(Request.Form["query[cariGrupId]"]))
            //{
            //    var cariGrupId = Convert.ToInt32(Request.Form["query[cariGrupId]"]);
            //    model = model.Where(i => i.CariGrupId == cariGrupId);
            //}

            //try
            //{
            //    model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            //}
            //catch (Exception)
            //{
            //    model = model.OrderBy("CariId Desc");
            //    dtMeta.field = "CariId";
            //    dtMeta.sort = "Desc";
            //}
            
            //model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            
            //var dto = model.Select(i => new
            //{
            //    CariId = i.CariId,
            //    Unvan = i.Unvan,
            //    BirincilKisi = i.Kisi.Count >= 1 ? i.Kisi.FirstOrDefault(f => f.Birincil).AdSoyad : "",
            //    BirincilKisiEposta = i.Kisi.Count >= 1 ? i.Kisi.FirstOrDefault(f => f.Birincil).Eposta : "",
            //    Ilce = i.IlceId != null ? i.Ilce.Adi : "",
            //    Il = i.IlId != null ? i.Il.Adi : "",
            //    Durum = i.Aktif ? "Aktif" : "Pasif"
            //}).ToList<dynamic>();


            //dtModel.meta = dtMeta;
            //dtModel.data = dto;
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


        [Yetki("Hammadde Kaydet", "Üretim")]
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