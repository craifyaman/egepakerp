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
        public Db _db { get; set; }
        public PdfController()
        {
            _db = new Db();
        }

        public ActionResult SiparisDetay(int siparisId)
        {
            var siparis = _db.Siparis
                .Include("Cari")
                .Include("Urun")
                .Include(x=>x.Urun.UrunCinsi)
                .FirstOrDefault(x => x.SiparisId == siparisId);

            return View(siparis);
        }

        public ActionResult SiparisUretimDetay(int siparisId)
        {
            var siparis = _db.Siparis
                .Include("Cari")
                .Include("Urun")
                .Include(x => x.Urun.UrunCinsi)
                .FirstOrDefault(x => x.SiparisId == siparisId);
            return View(siparis);
        }

        public ActionResult TeklifFormu(int formId,string lang)
        {
            var teklifForm = _db.SiparisTeklifForm
                .Include(x=>x.SiparisTeklifFormUrun)
                .Include(x=>x.Doviz)
                .FirstOrDefault(x => x.SiparisTeklifFormId == formId);
            ViewBag.lang = lang;
            return View(teklifForm);
        }

        public ActionResult ProformaFatura(int faturaId, string lang)
        {
            var teklifForm = _db.ProformaFatura
                .Include(x => x.ProformaUrun)
                .Include(x => x.Doviz)
                .FirstOrDefault(x => x.ProformaFaturaId== faturaId);
            ViewBag.lang = lang;
            return View(teklifForm);
        }

    }
}