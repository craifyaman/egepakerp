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
    public class UretimAksiyonController : BaseController
    {
        public UretimAksiyonRepository repo { get; set; }
        public UretimAksiyonController()
        {
            repo = new UretimAksiyonRepository();
        }

        //[Menu("Yaldız Pdf Dosyaları", "fas fa-align-justify icon-xl", "Yaldız", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }

        //[Yetki("Yaldız Listesi", "Yaldız")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UretimAksiyonId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            //if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            //{
            //    var searchQuery = Request.Form["query[searchQuery]"].ToString();
            //    model = model.Where(i =>
            //    i.Makine.MakineAd.ToLower().Contains(searchQuery.ToLower())
            //    );
            //}

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("UretimAksiyonId Desc");
                dtMeta.field = "UretimAksiyonId";
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
                Id = i.UretimAksiyonId,
                Baslangic = i.Baslangic,
                Bitis = i.Bitis,
                UretilenAdet = i.UretilenAdet,
                Makine = i.Makine.MakineAd,
                UretimEmirId = i.UretimEmirId,

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        public PartialViewResult Form(int? id,int MakineId, int UretimEmirId= 0)
        {
            ViewBag.UretimEmirId = UretimEmirId;
            ViewBag.MakineId = MakineId;
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);
            return PartialView(model);
        }
        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        //[Yetki("Yaldız Kaydet", "Yaldız")]
        public JsonResult Kaydet(UretimAksiyon form)
        {
            var response = new Response();
            var siparisKalipRepo = new SiparisKalipRepository();
            var kalipRepo = new KalipRepository();
            var uretimEmirRepo = new UretimEmirRepository();

            //tahmini bitiş zamanı hesaplama 
            var uretimEmir = uretimEmirRepo.Get(form.UretimEmirId);
            var _siparisKalip = siparisKalipRepo.Get(uretimEmir.SiparisKalipId);
            var _kalip = kalipRepo.Get(x => x.ParcaKodu == _siparisKalip.KalipKod);
            var _gozSayisi = _kalip.KalipGozSayisi;

            var fireOrani = _kalip.KalipHammaddeRelation.FirstOrDefault().HammaddeCinsi.HammaddeFire.Oran;
            var fireliAdet = form.UretilenAdet + (form.UretilenAdet* fireOrani / 100);
            form.Baslangic = DateTime.Now;
            var bitisZaman = fireliAdet * _kalip.UretimZamani / _gozSayisi;//saniye cinsinden bitis saati
            form.Bitis = form.Baslangic.AddMinutes(bitisZaman / 60);



            if (form.UretimAksiyonId == 0)
            {
                repo.Insert(form);
                response.Success = true;
                response.Description = "Kayıt edildi.";
            }
            else
            {
                var entity = repo.Get(form.UretimAksiyonId);
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