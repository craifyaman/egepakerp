using EgepakErp.DtModels;
using EgepakErp.Enums;
using EgepakErp.Helper;
using EgePakErp.Custom;
using EgePakErp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

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

        public PartialViewResult UrunKaliplari(int urunId, List<int> exclude)
        {
            //exclude = çıkarılan kalıpların id listesi.
            var model = new List<Kalip>();
            if (exclude == null)
            {
                model = Db.KalipUrunRelation.Include("Kalip").Where(x => x.UrunId == urunId).Select(x => x.Kalip).ToList();
            }
            else
            {
                model = Db.KalipUrunRelation.Include("Kalip").Where(x => x.UrunId == urunId && !exclude.Contains(x.KalipId)).Select(x => x.Kalip).ToList();
            }
            return PartialView(model);
        }

        public PartialViewResult MaliyetForm(List<int> idList)
        {
            ViewBag.TozBoyaSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("toz")).BirimFiyat;
            ViewBag.BaskiMalzemeler = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).Where(x => x.HammaddeCinsiId == 6632).ToList();
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("poşet")).BirimFiyat;
            ViewBag.KoliSonHareket = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            var kaliplar = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
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

        public PartialViewResult MaliyetDetay(string MaliyetType, int KalipId, string PosetParametre = "0")
        {
            var Kalip = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .FirstOrDefault(x => x.KalipId == KalipId);

            ViewBag.MaliyetType = MaliyetType;
            ViewBag.TozBoyaSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("toz")).BirimFiyat;
            ViewBag.BaskiMalzemeler = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).Where(x => x.HammaddeCinsiId == 6632).ToList();
            ViewBag.PosetSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("poşet")).BirimFiyat;
            ViewBag.KoliSonBirimFiyat = Db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı")).BirimFiyat;
            ViewBag.HamMaddeHareket = Db.HammaddeHareket.ToList();
            ViewBag.PosetParametre = PosetParametre;
            return PartialView(Kalip);
        }

    }
}