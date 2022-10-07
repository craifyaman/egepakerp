using EgePakErp.Concrete;
using EgePakErp.Custom;
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

        [Menu("Enjeksiyon", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 1)]
        public ActionResult Enjeksiyon()
        {
            var uretimEmir = repo.GetAll();
            var list = uretimEmir.ToList();
            return View(uretimEmir.ToList());
        }

        [Menu("Sıcak Baskı", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 2)]
        public ActionResult SicakBaski()
        {
            var uretimEmir = repo.GetAll();
            return View(uretimEmir.ToList());
        }

        [Menu("Metalize", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 3)]
        public ActionResult Metalize()
        {
            var uretimEmir = repo.GetAll();
            return View(uretimEmir.ToList());
        }

        [Menu("Montaj", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 3)]
        public ActionResult Montaj()
        {
            var uretimEmir = repo.GetAll();
            return View(uretimEmir.ToList());
        }
        [Menu("Sprey Boya", "flaticon-cogwheel-1 icon-xl", "Üretim Takip", 4, 3)]
        public ActionResult SpreyBoya()
        {
            var uretimEmir = repo.GetAll();
            return View(uretimEmir.ToList());
        }

    }
}