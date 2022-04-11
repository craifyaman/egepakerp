using EgePakErp.Custom;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
namespace EgePakErp.Controllers
{
    public class HammaddeCinsiController : BaseController
    {
        // GET: Cari
        [Menu("Hammadde Cinsi", "flaticon-customer icon-xl", "Üretim", 1,2)]
        public ActionResult Index()
        {
            return View();
        }


        [Yetki("Hammadde Cinsi", "Üretim")]
        public JsonResult Liste()
        {
            
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "hammaddeCinsiId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Adi" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = Db.HammaddeCinsi
                .Include("Kalip")                
                .AsQueryable();


            var count = model.Count();
            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

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
                model = model.OrderBy("HammaddeCinsiId");
                dtMeta.field = "HammaddeCinsiId";
                dtMeta.sort = "Adi";
            }
            
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);
                        
            var dto = model.AsEnumerable().Select(i => new
            {
                HammaddeCinsiId = i.HammaddeCinsiId,
                Adi = i.Adi,                
                Aciklamasi = i.Aciklamasi,
                BirimId = i.BirimId,/*Daaha Sonra değişcek*/
                Kodu= i.Kisaltmasi,
                //UretimZamani = i.UretimZamani,
                Kaliplar = i.Kalip.Select(s => s.Adi),
                KalipKodu = i.Kalip.Select(s => s.Adi+ "-"+s.KalipOzellik)             

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
    }
}