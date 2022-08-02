using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{

    public class HammaddeHareketController : BaseController
    {
        // GET: Cari
        [Menu("Satın Alma Listesi", "flaticon2-list-1 icon-xl", "Üretim", 0,6)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Hammadde Hareket", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "HammaddeHareketId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.HammaddeHareket
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
                .AsQueryable();


            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i => i.UrunAdi.ToLower().Contains(searchQuery.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[dovizId]"]))
            {
                var dovizId = Convert.ToInt32(Request.Form["query[dovizId]"]);
                model = model.Where(i => i.DovizId == dovizId);
            }
            if (!string.IsNullOrEmpty(Request.Form["query[hammaddeCinsiId]"]))
            {
                var hammaddeCinsiId = Convert.ToInt32(Request.Form["query[hammaddeCinsiId]"]);
                model = model.Where(i => i.HammaddeCinsiId == hammaddeCinsiId);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }
            catch (Exception)
            {
                model = model.OrderBy("HammaddeHareketId Desc");
                dtMeta.field = "HammaddeHareketId";
                dtMeta.sort = "Desc";
            }
            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                HammaddeHareketId = i.HammaddeHareketId,
                FaturaNo = i.FaturaNo,
                UrunAdi = i.UrunAdi,
                KayitTarihi = i.KayitTarihi.ToString("yyyy/MM/dd"),
                TedarikciId = i.TedarikciId,
                HammaddeCinsi = i.HammaddeCinsi?.Adi,
                BirimFiyat = i.BirimFiyat,
                ToplamTutar = i.ToplamTutar,
                Doviz = i.Doviz.Adi,
                Miktar = i.Miktar,
                DolarKuru = i.DolarKuru,
                EuroKuru = i.EuroKuru,
                HammaddeBirimi = i.TableHammaddeBirim.Birimi
            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);
        }

        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = Db.HammaddeHareket
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
                .FirstOrDefault(i => i.HammaddeHareketId == id);
            return PartialView(model);
        }

        public PartialViewResult FilterForm()
        {
            return PartialView();
        }


        [Yetki("HammaddeHareket Kaydet", "Uretim")]
        public JsonResult Kaydet(HammaddeHareket form)
        {
            var response = new Response();

            try
            {
                if (form.HammaddeHareketId == 0)
                {
                    form.KayitTarihi = DateTime.Now;                    
                    Db.HammaddeHareket.Add(form);
                }
                else
                {
                    var entity = Db.HammaddeHareket.Find(form.HammaddeHareketId);
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