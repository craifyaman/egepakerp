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
    public class AksiyonController : BaseController
    {
        public AksiyonRepository repo { get; set; }
        public AksiyonController()
        {
            repo = new AksiyonRepository();
        }

        [Menu("Aksiyonlar", "flaticon2-menu-2 icon-xl", "Üretim", 0, 5)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "AksiyonId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("AksiyonId Desc");
                dtMeta.field = "AksiyonId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            var dto = model.AsEnumerable().Select(x => new
            {
                Id = x.AksiyonId,
                Aciklama = x.Aciklama,
                AksiyonType = x.AksiyonType.Aciklama,
                Baslangic = x.AksiyonBaslangic.ToString("dd-MM-yyyy HH:mm"),
                Bitis = x.AksiyonBitis.ToString("dd-MM-yyyy HH:mm"),
            }).ToList();


            dtModel.meta = dtMeta;
            dtModel.data = dto.ToList<dynamic>();
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id, int UretimEmirId, string aksiyonType, int UretimEmirDurumId)
        {
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);
            ViewBag.UretimEmirId = UretimEmirId;
            ViewBag.AksiyonType = aksiyonType;
            ViewBag.UretimEmirDurumId = UretimEmirDurumId;

            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        public JsonResult Kaydet(Aksiyon form)
        {
            var response = new Response();
            var uretimEmirRepo = new UretimEmirRepository();

            if (form.AksiyonId == 0)
            {
                repo.Insert(form);

                var uretimemir = uretimEmirRepo.Get(form.UretimEmirId);
                //uretimemir.UretimEmirDurumList = uretimemir.UretimEmirDurumList + "," + form.UretimEmirDurumId;
                uretimEmirRepo.Update(uretimemir);
                response.Success = true;
                response.Description = "Kayıt edildi.";
            }
            else
            {
                var entity = repo.Get(form.AksiyonId);
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

            return Json(response);

        }


    }
}