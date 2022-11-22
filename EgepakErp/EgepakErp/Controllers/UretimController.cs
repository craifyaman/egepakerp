using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.Enums;
using EgePakErp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;



namespace EgePakErp.Controllers
{
    public class UretimController : BaseController
    {
        private UretimEmirRepository repo;
        public UretimController()
        {
            repo = new UretimEmirRepository();
        }

        //[Menu("Enjeksiyon", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 1)]
        //public ActionResult Index()
        //{
        //    var uretimEmir = repo.GetAll();
        //    return View(uretimEmir);
        //}
        private List<UretimEmir> UretimEmirtListe()
        {
            return repo.GetAll(x => x.Siparis.SiparisDurumId == (int)ESiparisType.Uretimde).ToList();
        }
        [Menu("Enjeksiyon", "fas fa-syringe fa-2x text-info mt-2", "Üretim Takip", 4, 1)]
        public ActionResult Enjeksiyon()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }

        [Menu("Sıcak Baskı", "fas fa-compress fa-2x text-info mt-2", "Üretim Takip", 4, 2)]
        public ActionResult SicakBaski()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }

        [Menu("Metalize", "fas fa-paint-brush fa-2x text-info mt-2", "Üretim Takip", 4, 3)]
        public ActionResult Metalize()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }

        [Menu("Montaj", "fas fa-braille fa-2x text-info mt-2", "Üretim Takip", 4, 3)]
        public ActionResult Montaj()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }
        [Menu("Sprey Boya", "fas fa-brush fa-2x text-info mt-2", "Üretim Takip", 4, 3)]
        public ActionResult SpreyBoya()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }

        [Menu("Ev Montaj", "fas fa-braille fa-2x text-info mt-2", "Üretim Takip", 4, 3)]
        public ActionResult EvMontaj()
        {
            var uretimEmir = UretimEmirtListe();
            return View(uretimEmir);
        }

        [Menu("Havuz", "fas fa-braille fa-2x text-info mt-2", "Üretim Takip", 4, 5)]
        public ActionResult Havuz()
        {
            return View();
        }

    }
}