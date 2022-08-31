using EgePakErp.Concrete;
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
    public class YaldizController : BaseController
    {
        public YaldizRepository repo { get; set; }
        public YaldizController()
        {
            repo = new YaldizRepository();
        }
        // GET: Fiyat
        [Menu("Yaldız listesi", "fas fa-align-justify icon-xl", "Yaldız", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Yaldız Listesi", "Yaldız")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "YaldizId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.Aciklama.ToLower().Contains(searchQuery.ToLower())
                );
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("YaldizId Desc");
                dtMeta.field = "YaldizId";
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
                YaldizId = i.YaldizId,
                Aciklama = i.Aciklama,
                Cari = i.Cari.Unvan
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
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        [Yetki("Yaldız Kaydet", "Yaldız")]
        public JsonResult Kaydet(Yaldiz form)
        {
            var response = new Response();

            try
            {
                if (form.YaldizId == 0)
                {
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = repo.Get(form.YaldizId);
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