using EgePakErp.Custom;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using EgePakErp.Models;
using EgePakErp.Concrete;

namespace EgePakErp.Controllers
{
    public class HammaddeCinsiController : BaseController
    {
        public HammaddeCinsiRepository repo { get; set; }
        public HammaddeCinsiController()
        {
            repo = new HammaddeCinsiRepository();
        }

        [Menu("Hammadde Cinsi", "flaticon-customer icon-xl", "Üretim", 1,2)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Hammadde Cinsi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "hammaddeCinsiId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();
            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.Adi.ToLower().Contains(searchQuery.ToLower()) ||
                i.Kisaltmasi.ToLower().Contains(searchQuery.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[hammaddeCinsiId]"]))
            {
                var uretimTeminSekliId = Convert.ToInt32(Request.Form["query[hammaddeCinsiId]"]);
                model = model.Where(i => i.HammaddeCinsiId == uretimTeminSekliId);
            }


            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("hammaddeCinsiId Desc");
                dtMeta.field = "hammaddeCinsiId";
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
                HammaddeCinsiId = i.HammaddeCinsiId,
                Adi = i.Adi,
                Birim = i.TableHammaddeBirim.Birimi,
                Aciklamasi = i.Aciklamasi,
                Durum = i.isAktive          

            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            if (id!=0)
            {
                var model = repo.Get((int)id);
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
                    form.isAktive = true;
                    repo.Insert(form);
                }
                else
                {
                    var entity = repo.Get(form.HammaddeCinsiId);
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

        public PartialViewResult HammaddeBirimForm()
        {
            return PartialView();
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }
        

        public JsonResult BirimEkle(TableHammaddeBirim form)
        {
            Response response = new Response();
            try
            {
                Db.TableHammaddeBirim.Add(form);
                Db.SaveChanges();
                response.Success = true;
                response.Description = "Birim Eklendi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DurumGuncelle(int id)
        {
            Response response = new Response();
            try
            {
                var hammadde = repo.Get(id);
                var statu = hammadde.isAktive == true ? false : true;
                hammadde.isAktive = statu;
                repo.Update(hammadde);
                response.Success = true;
                response.Description = "güncellendi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Sil(int id)
        {
            Response response = new Response();
            try
            {
                repo.Delete(repo.Get(id));
                response.Success = true;
                response.Description = "Hammadde Silindi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

    }
}