using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data.Entity;

namespace EgePakErp.Controllers
{

    public class UretimSabitController : BaseController
    {
        public UretimSabitlerRepository repo { get; set; }
        public UretimSabitController()
        {
            repo = new UretimSabitlerRepository();
        }
        // GET: Cari
        [Menu("Üretim Sabitler", "flaticon2-download icon-xl", "Üretim", 0, 5)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Üretim Sabitleri Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UretimSabitlerId" : Request.Form["sort[field]"];
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
                i.Kod.ToString() == searchQuery
                );
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("UretimSabitlerId Desc");
                dtMeta.field = "UretimSabitlerId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                UretimSabitlerId = i.UretimSabitlerId,
                Aciklama = i.Aciklama,
                Maliyet = i.Maliyet,
                Birim = i.Birim,
                Kod = i.Kod
            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);
            return PartialView(model);
        }


        [Yetki("Uretim Sabit Kaydet", "Üretim")]
        public JsonResult Kaydet(UretimSabitler form)
        {
            var response = new Response();

            try
            {
                if (form.UretimSabitlerId == 0)
                {
                    repo.Insert(form);
                }
                else
                {
                    var entity = repo.Get(form.UretimSabitlerId);
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

                    }


                }
                Db.SaveChanges(CurrentUser.PersonelId);
                response.Success = true;
                response.Description = "Kayıt Güncellendi";
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