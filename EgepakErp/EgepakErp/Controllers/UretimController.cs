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
    public class UretimController : BaseController
    {
        private UretimEmirRepository repo;
        public UretimController()
        {
            repo = new UretimEmirRepository();
        }
        public ActionResult Index()
        {
            var uretimEmir = repo.GetAll();
            return View(uretimEmir);
        }
    }
}