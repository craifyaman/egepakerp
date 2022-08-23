using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{

    public class UrunController : BaseController
    {
        public UrunRepository repo { get; set; }
        public UrunController()
        {
            repo = new UrunRepository();
        }
        // GET: Cari
        [Menu("Ürünler", "flaticon2-list-2 icon-xl", "Üretim", 0, 4)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Ürün Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UrunId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i => (i.UrunCinsi.Kisaltmasi + i.UrunNo).Contains(searchQuery) ||
                i.UrunNo.ToLower().Contains(searchQuery.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[urunCinsiId]"]))
            {
                var urunCinsiId = Convert.ToInt32(Request.Form["query[urunCinsiId]"]);
                model = model.Where(i => i.UrunCinsiId == urunCinsiId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("UrunId Desc");
                dtMeta.field = "UrunId";
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
                UrunId = i.UrunId,
                UrunKodu = string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo),
                UrunCinsi = i.UrunCinsi.Aciklamasi,
                Kalip = i.KalipUrunRelation.Select(s => s.Kalip).Select(s => s.KalipNo + " " + s.KalipOzellik)

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

        [Yetki("Ürün Kaydet", "Üretim")]
        public JsonResult Kaydet(Urun form)
        {
            var response = new Response();

            try
            {
                if (form.UrunId == 0)
                {
                    form.KalipUrunRelation = form.KalipList.Select(s => new KalipUrunRelation { KalipId = s }).ToList();
                    form.isAktif = true;
                    repo.Insert(form);
                }
                else
                {
                    var entity = repo.Get(form.UrunId);
                    if (entity != null)
                    {
                        var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                        foreach (var prop in propList)
                        {
                            if (form.Include.Contains(prop.Name))
                            {
                                prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                            }
                        }
                        repo.Update(entity);

                        Db.KalipUrunRelation.RemoveRange(Db.KalipUrunRelation.Where(i => i.UrunId == form.UrunId));
                        if (form.KalipList != null)
                        {
                            Db.KalipUrunRelation.AddRange(form.KalipList.Select(s => new KalipUrunRelation { KalipId = s, UrunId = entity.UrunId }));
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

        public JsonResult ListeSelect2(string q)
        {
            //kabasını aldır
            var model = repo.GetAll();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(i =>
                string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo).Contains(searchQuery.ToLower()) ||
                i.UrunCinsi.Kisaltmasi.ToLower().Contains(searchQuery.ToLower())
                );
            }

            model = model.OrderBy("UrunId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.UrunId,
                    text = string.Concat(i.UrunCinsi.Kisaltmasi + i.UrunNo),
                    markup = "urun"

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AktifPasif(int id)
        {
            Response response = new Response();
            var urun = repo.Get(id);
            bool statu = urun.isAktif == true ? false : true;
            try
            {
                urun.isAktif = statu;
                repo.Update(urun);
                response.Success = true;
                response.Description = "ürün silindi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }

    }
}