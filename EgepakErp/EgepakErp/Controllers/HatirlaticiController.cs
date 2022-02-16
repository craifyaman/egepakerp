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

    public class HatirlaticiController : BaseController
    {
        // GET: Cari
        [Menu("Hatırlatıcı", "flaticon2-time icon-xl", "Hatırlatıcı", 1, 3)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Hatırlatıcı Listesi", "Hatırlatıcı")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "HatirlaticiId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.Hatirlatici
                .Include("Personel")
                .AsQueryable();


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

            if (!string.IsNullOrEmpty(Request.Form["query[durum]"]))
            {
                var durum = Request.Form["query[durum]"] == "Aktif";
                model = model.Where(i => i.Durum == durum);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("HatirlaticiId Desc");
                dtMeta.field = "HatirlaticiId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                HatirlaticiId = i.HatirlaticiId,
                Personel = i.Personel.Adi,
                HatirlatmaTarihi = i.HatirlatmaTarihi.ToString("yyyy/MM/dd HH:mm"),
                Durum = i.Durum?"Açık":"Kapalı",
                Sms = i.Sms?"Sms":"-",
                Eposta = i.Eposta? "Eposta" : "-",
                Whatsapp = i.Whatsapp? "Whatsapp" : "-",

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.Hatirlatici
                .Include("Personel")
                .FirstOrDefault(i => i.HatirlaticiId == id);

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Hatırlatıcı Kaydet", "Hatırlatıcı")]
        public JsonResult Kaydet(Hatirlatici form)
        {
            var response = new Response();

            try
            {
                if (form.HatirlaticiId == 0)
                {
                    form.KayitTarihi = DateTime.Now;
                    form.Durum = true;
                    Db.Hatirlatici.Add(form);
                }
                else
                {
                    var entity = Db.Hatirlatici.Find(form.HatirlaticiId);
                    if (entity != null)
                    {
                        foreach (var prop in entity.GetType().GetProperties())
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