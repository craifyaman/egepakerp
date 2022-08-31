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
    public class BoyaKodController : BaseController
    {
        public BoyaKodRepository repo { get; set; }
        public BoyaKodController()
        {
            repo = new BoyaKodRepository();
        }
        // GET: Fiyat
        [Menu("Boya Kodları", "fas fa-paint-brush icon-xl", "Boya Kodları", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Boya Kodları Listesi", "Boya Kodları")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "BoyaKodId" : Request.Form["sort[field]"];
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

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("BoyaKodId Desc");
                dtMeta.field = "BoyaKodId";
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

        [Yetki("Boya Kod Kaydet", "Boya Kodları")]
        public JsonResult Kaydet(BoyaKod form)
        {
            var response = new Response();

            try
            {
                if (form.BoyaKodId== 0)
                {
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }
                else
                {
                    var entity = repo.Get(form.BoyaKodId);
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