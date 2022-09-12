using EgePakErp.Concrete;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Data.Entity;
namespace EgePakErp.Controllers
{
    public class PdfController : Controller
    {
        // GET: Pdf
        public ActionResult SiparisDetay(int siparisId)
        {
            var db = new Db();
            var siparis = db.Siparis
                .Include("Cari")
                .Include("Urun")
                .Include(x=>x.Urun.UrunCinsi)
                .FirstOrDefault(x => x.SiparisId == siparisId);

            return View(siparis);
        }
    }
}