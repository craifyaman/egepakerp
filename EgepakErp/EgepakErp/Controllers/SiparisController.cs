using EgepakErp.DtModels;
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
        public List<Kalip> Kaliplar { get; set; }
        // GET: Cari
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
            var kaliplar = Db.Kalip
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Where(i => idList.Contains(i.KalipId)).ToList();
            return PartialView(kaliplar);
        }

        public PartialViewResult MaliyetHesap(List<MaliyetDto> liste)
        {
            return PartialView(liste);
        }


    }
}