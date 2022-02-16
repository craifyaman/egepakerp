using EgePakErp.Custom;
using EgePakErp.Helper;
using EgePakErp.Models;
using EgePakErp.Models.Audit;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class HomeController : BaseController
    {
        [Menu("Anasayfa", "flaticon-squares icon-xl", "Anasayfa", 0, 0)]
        public ActionResult Index()
        {
            return View();
        }

    }
}