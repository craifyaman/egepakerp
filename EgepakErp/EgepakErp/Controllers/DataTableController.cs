using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class DataTableController : Controller
    {
        // GET: DataTable

        public JsonResult Index()
        {
            var keys = Request.Form.AllKeys.Select(s => s);
            foreach (var key in keys)
            {
                var deger = "";
            }
            return Json("");
        }
    }
}