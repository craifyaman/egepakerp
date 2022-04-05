using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{
    public class SiparisController : BaseController
    {
        // GET: Cari
        [Menu("Sipariş Formu", "flaticon2-cart icon-xl", "Sipariş", 0, 5)]
        public ActionResult SiparisFormu()
        {
            return View();
        }

    }
}