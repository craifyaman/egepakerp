using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Hosting;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class RaporController : BaseController
    {
        public UretimEmirAksiyonRepository uretimEmirAksiyonRepo { get; set; }
        public StokCikisHareketRepository stokCikisHareketRepo { get; set; }
        public string dtMetaField { get; set; }

        public RaporController()
        {
            uretimEmirAksiyonRepo = new UretimEmirAksiyonRepository();
            stokCikisHareketRepo = new StokCikisHareketRepository();
        }

        [Menu("Enjeksiyon Raporları", "fas fa-syringe fa-2x text-info mt-2 text-info", "Rapor", 0, 7)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Enjeksiyon Rapor Liste", "Rapor")]
        public JsonResult EnjeksiyonListe()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "UretimEmirAksiyonId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            //var model = uretimEmirAksiyonRepo.GetAll(x => x.UretimEmirAksiyonTypeId == (int)EUretimEmirAksiyonType.Enjeksiyon);
            var model = uretimEmirAksiyonRepo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(
                    i => i.Kisi.AdSoyad.ToLower().Contains(searchQuery.ToLower()) ||
                    i.UretimEmir.SiparisKalip.EnjeksiyonRenk.ToLower().Contains(searchQuery.ToLower())

                    );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[RaporTypeId]"]))
            {
                int RaporTypeId = Convert.ToInt32(Request.Form["query[RaporTypeId]"].ToString());
                if (RaporTypeId > 0)
                {
                    model = model.Where(i => i.UretimEmirAksiyonTypeId == RaporTypeId);
                }

            }

            if (!string.IsNullOrEmpty(Request.Form["query[KisiId]"]))
            {
                int KisiId = Convert.ToInt32(Request.Form["query[KisiId]"].ToString());
                if (KisiId > 0)
                {
                    model = model.Where(i => i.KisiId == KisiId);
                }

            }


            //tarih aralığındaki kayıtları getirme
            string baslamaTarih = Request.Form["query[baslama]"];
            string bitisTarih = Request.Form["query[bitis]"];

            if (!string.IsNullOrEmpty(baslamaTarih) && !string.IsNullOrEmpty(bitisTarih))
            {
                DateTime baslama = DateTime.Parse(baslamaTarih);
                DateTime bitis = DateTime.Parse(bitisTarih);
                model = model.Where(i => i.KayitTarih >= baslama && i.KayitTarih <= bitis);
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("UretimEmirAksiyonId Desc");
                dtMeta.field = "UretimEmirAksiyonId";
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
                Id = i.UretimEmirAksiyonId,
                UretimEmirId = i.UretimEmirId,
                BitenAdet = i.BitenAdet,
                KisiId = i.KisiId,
                Kisi = i.Kisi.AdSoyad,
                KayitTarih = i.KayitTarih.ToString("dd MM yyyy"),
                Parca = i.UretimEmir.SiparisKalip.KalipAdi,
                Bolum = i.UretimEmirAksiyonType.Type

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }

        [Menu("Sevkiyat Hareket", "fa-solid fa-truck icon-xl", "Rapor", 0, 7)]
        public ActionResult Sevkiyat()
        {
            return View();
        }

        
        [Yetki("Sevkiyat Rapor Liste", "Rapor")]
        public JsonResult SevkiyatListe()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "StokHareketId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = stokCikisHareketRepo.GetAll();

            //tarih aralığındaki kayıtları getirme
            string tarih = Request.Form["query[tarih]"];

            if (!string.IsNullOrEmpty(tarih))
            {
                DateTime _tarih = DateTime.Parse(tarih);
                model = model.Where(i => i.CikisTarih >= _tarih);
            }

            if (!string.IsNullOrEmpty(Request.Form["query[CariId]"]))
            {
                int CariId = Convert.ToInt32(Request.Form["query[CariId]"].ToString());
                if (CariId > 0)
                {
                    model = model.Where(i => i.CariId == CariId);
                }

            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("StokCikisHareketId Desc");
                dtMeta.field = "StokCikisHareketId";
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
                Id = i.StokCikisHareketId,
                Adet = i.Adet,
                CikisTarih = i.CikisTarih.ToString("dd MM yyyy"),
                StokHareketId = i.StokHareketId,
                CariId = i.CariId,
                Cari = i.Cari.Unvan,
                Aciklama = i.Aciklama,

            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
    }
}