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

    public class NotController : BaseController
    {
        // GET: Cari
        [Menu("Müşteri Notları", "flaticon-notes icon-xl", "Müşteri", 4, 2)]
        public ActionResult Index()
        {
            return View();
        }

        //[Yetki("Görüşme Listesi", "Görüşme")]
        public JsonResult Liste(int? cariId)
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "GorusmeId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Not
                .Include("Cari")
                .Include("Personel")
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
                i.Aciklama.ToLower().Contains(searchQuery.ToLower())  
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[personelId]"]))
            {
                var personelId = Convert.ToInt32(Request.Form["query[personelId]"]);
                model = model.Where(i => i.PersonelId == personelId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("NotId Desc");
                dtMeta.field = "NotId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                NotId = i.NotId,
                Aciklama = i.Aciklama,
                Cari = i.Cari.Unvan,
                Personel = i.Personel.Adi,
                KayitTarihi = i.KayitTarihi.ToString("yyyy/MM/dd HH:mm"),
            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }

        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Not
                .Include("Cari")
                .Include("Personel")
                .FirstOrDefault(i => i.NotId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Müşteri Notu", "Müşteri Notu")]
        public JsonResult Kaydet(Not frm)
        {
            var response = new Response();

            try
            {
                if (frm.NotId == 0)
                {
                    frm.KayitTarihi = DateTime.Now;
                    frm.PersonelId = CurrentUser.PersonelId;

                    Db.Not.Add(frm);
                }
                else
                {
                    Db.Update<Not>(frm, "PersonelId","KayitTarihi");
                }
                Db.SaveChanges();
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