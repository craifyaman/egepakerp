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

    public class GorusmeController : BaseController
    {
        // GET: Cari
        [Menu("Görüşme Listesi", "flaticon-chat-1 icon-xl", "Müşteri", 3, 2)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Görüşme Listesi", "Görüşme")]
        public JsonResult Liste(int? cariId)
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "GorusmeId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Gorusme
                .Include("Kisi")
                .Include("Kisi.Cari")
                .Include("Personel")
                .Include("GorusmeTip")
                .AsQueryable();

            if (cariId != null)
            {
                model = model.Where(i => i.Kisi.Cari.CariId == cariId);
            }

            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;



            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.Kisi.AdSoyad.ToLower().Contains(searchQuery.ToLower()) ||
                i.Kisi.Cari.Unvan.ToLower().Contains(searchQuery.ToLower()) ||
                i.Konu.ToLower().Contains(searchQuery.ToLower()) ||
                i.Aciklama.ToLower().Contains(searchQuery.ToLower())


                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[gorusmeTipId]"]))
            {
                var gorusmeTipId = Convert.ToInt32(Request.Form["query[gorusmeTipId]"]);
                model = model.Where(i => i.GorusmeTipId == gorusmeTipId);
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
            var dto = model.AsEnumerable().Select(i => new
            {
                GorusmeId = i.GorusmeId,
                Kisi = i.Kisi.AdSoyad,
                Cari = i.Kisi.Cari.Unvan,
                Personel = i.Personel.Adi,
                GorusmeTip = i.GorusmeTip.Adi,
                Konu = i.Konu,
                Aciklama = i.Aciklama,
                GorusmeTarihi=i.GorusmeTarihi.ToString("yyyy/MM/dd HH:mm"),
                KayitTarihi = i.KayitTarihi

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }

        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Gorusme
                 .Include("Kisi")
                .Include("Kisi.Cari")
                .Include("Personel")
                .Include("GorusmeTip")
                .FirstOrDefault(i => i.GorusmeId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Görüşme Kaydet", "Görüşme")]
        public JsonResult Kaydet(Gorusme frm)
        {
            var response = new Response();

            try
            {
                if (frm.GorusmeId == 0)
                {
                    frm.KayitTarihi = DateTime.Now;
                    frm.PersonelId = CurrentUser.PersonelId;

                    Db.Gorusme.Add(frm);
                }
                else
                {
                    Db.Update<Gorusme>(frm, "PersonelId","KayitTarihi");
                }
                Db.SaveChanges();
                response.Success = true;
                response.Description = "İşlem Başarılı";
            }
            catch (Exception ex )
            {
                response.Success = true;
                response.Description = "Hata Oluştu. Hata Mesajı: "+ex.Message.ToString();
            }



            return Json(response);

        }

    }
}