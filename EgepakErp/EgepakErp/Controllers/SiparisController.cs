using EgepakErp.DtModels;
using EgepakErp.Enums;
using EgepakErp.Helper;
using EgePakErp.Custom;
using EgePakErp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data.Entity;


namespace EgePakErp.Controllers
{
    public class SiparisController : BaseController
    {
        //public List<Kalip> Kaliplar { get; set; }

        [Menu("Sipariş Formu", "flaticon2-cart icon-xl", "Sipariş", 0, 5)]
        public ActionResult SiparisFormu()
        {
            return View();
        }

        public PartialViewResult UrunKaliplari(int urunId, List<int> exclude, List<int> includes)
        {
            //exclude = çıkarılan kalıpların id listesi.
            var model = new List<Kalip>();
            if (includes.Count() > 0)
            {
                foreach (var item in includes)
                {
                    var kalip = Db.Kalip.FirstOrDefault(x => x.KalipId == item);
                    if (kalip != null)
                    {
                        model.Add(kalip);
                    }
                }
            }
            if (exclude == null)
            {
                var list = Db.KalipUrunRelation.Include("Kalip").Where(x => x.UrunId == urunId).Select(x => x.Kalip).ToList();
                model.AddRange(list);

            }
            else
            {
                var list = Db.KalipUrunRelation.Include("Kalip").Where(x => x.UrunId == urunId && !exclude.Contains(x.KalipId)).Select(x => x.Kalip).ToList();
                model.AddRange(list);
            }
            var urun = Db.Urun.Include(x => x.UrunCinsi).FirstOrDefault(x => x.UrunId == urunId);
            return PartialView(model);
        }

        public PartialViewResult MaliyetForm(List<int> idList, int urunId)
        {
            ViewBag.TozBoyaSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.ToLower().Contains("toz")).BirimFiyat;
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.ToLower().Contains("poşet")).BirimFiyat;
            ViewBag.KoliSonHareket = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.ToLower().Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            ViewBag.urunId = urunId;
            var kaliplar = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Where(i => idList.Contains(i.KalipId)).ToList();
            return PartialView(kaliplar);
        }

        public PartialViewResult MaliyetHesap(List<MaliyetDto> liste)
        {
            var date = System.DateTime.Now;
            ViewBag.dolarKur = DovizHelper.DovizKuruGetir("USD", date);
            ViewBag.euroKur = DovizHelper.DovizKuruGetir("EUR", date);
            return PartialView(liste);
        }

        [HttpGet]
        public JsonResult UsdKurHesapla(double tutar)
        {
            Response<decimal> response = new Response<decimal>();
            try
            {
                var date = System.DateTime.Now;
                var usdKur = DovizHelper.DovizKuruGetir("USD", date);
                var sonuc = (decimal)tutar / usdKur;
                response.Success = true;
                response.Data = sonuc;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EurKurHesapla(double tutar)
        {
            Response<decimal> response = new Response<decimal>();
            try
            {
                var date = System.DateTime.Now;
                var eurKur = DovizHelper.DovizKuruGetir("EUR", date);
                var sonuc = (decimal)tutar / eurKur;
                response.Success = true;
                response.Description = "başarılı";
                response.Data = sonuc;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult MaliyetDetay(string MaliyetType, int KalipId, int urunId, string PosetParametre = "0.060")
        {
            var Kalip = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .FirstOrDefault(x => x.KalipId == KalipId);

            ViewBag.MaliyetType = MaliyetType;
            //ViewBag.TozBoyaSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("toz")).BirimFiyat;
            //ViewBag.BaskiMalzemeler = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).Where(x => x.HammaddeCinsiId == 6632).ToList();
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("poşet")).BirimFiyat;
            ViewBag.KoliSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı")).BirimFiyat;
            ViewBag.HamMaddeHareket = Db.HammaddeHareket.ToList();
            ViewBag.KoliSonHareket = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            ViewBag.PosetParametre = PosetParametre;
            ViewBag.urunId = urunId;
            return PartialView(Kalip);

        }

    }
}