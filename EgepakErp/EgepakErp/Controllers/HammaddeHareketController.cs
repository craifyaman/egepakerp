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

namespace EgePakErp.Controllers
{
    public class HammaddeHareketController:BaseController
    {
        // GET: Cari
        [Menu("Hammadde Hareket", "flaticon2-download icon-xl", "Üretim", 0, 4)]
        public ActionResult Index()
        {
            return View();
        }


        [Yetki("Hammadde Hareket", "Üretim")]
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

            //Filtre
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

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("HammaddeHaraketId Desc");
                dtMeta.field = "HammaddeHaraketId";
                dtMeta.sort = "Aciklama";
            }

            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);


            var dto = model.Select(i => new
            {
                HammaddeHaraketId = i.HammaddeHaraketId,
                FaturaTarihi = i.FaturaTarihi,
                Miktar = i.Miktar,
                KdvOranı = i.KdvOranı,
                FaturaNo = i.FaturaNo,
                OrjinalDovizCinsi=i.Doviz.Adi,
                KdvTutarı = i.KdvTutari,
                ToplamTutar = i.ToplamTutar,
                HammaddeCinsi=i.HammaddeCinsi.Adi,
                Aciklama=i.Aciklama
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
                var model = Db.HammaddeHaraket
                    .Include("HammaddeTipi")
                    .Include("Marka")
                    .Include("HammaddeCinsi")
                    .Include("Doviz")
                    .FirstOrDefault(i => i.HammaddeHaraketId == id);

                return PartialView(model);
            }

            return PartialView(new HammaddeHaraket());
        }


        [Yetki("Hammadde Hareket Kaydet", "Üretim")]
        public JsonResult Kaydet(HammaddeHaraket form)
        {
            var response = new Response();

            try
            {
                if (form.HammaddeCinsiId == 0)
                {

                    Db.HammaddeHaraket.Add(form);
                }
                else
                {
                    var entity = Db.HammaddeHaraket
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