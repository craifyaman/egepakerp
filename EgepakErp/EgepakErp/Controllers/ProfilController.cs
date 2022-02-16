using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{
    public class ProfilController : BaseController
    {
        // GET: Cari
        public ActionResult Index()
        {
            return View(Db.Personel.Include("PersonelTip").ToList());
        }

        public ActionResult Detay(int id)
        {
            id = CurrentUser.PersonelId;
            ViewBag.PersonelTip = Db.PersonelTip.ToList();
            return View(Db.Personel.Include("PersonelTip").FirstOrDefault(i => i.PersonelId == id));
        }

        public PartialViewResult Menu(int id)
        {
            id = CurrentUser.PersonelId;
            return PartialView(Db.Personel.Find(id));
        }

        public JsonResult Guncelle(Personel personel)
        {

            Db.Update(personel, new List<string> { "PersonelId", "Adi", "Eposta", "PersonelTipId" });
            Db.SaveChanges();
            var response = new Response();
            response.Success = true;
            response.Description = "Personel Bilgileri Başarıyla Güncellendi";
            return Json(response);
        }

        public ActionResult Parola(int id)
        {
            id = CurrentUser.PersonelId;
            return View(Db.Personel.Include("PersonelTip").FirstOrDefault(i => i.PersonelId == id));
        }

        public JsonResult ParolaGuncelle(Personel personel)
        {
            var response = new Response();
            if (personel.Parola != personel.YeniParola)
            {
                response.Success = false;
                response.Description = "Parolalar aynı olmalı";
                return Json(response);
            }

            Db.Update(personel, new List<string> { "PersonelId", "Parola" });
            Db.SaveChanges();

            response.Success = true;
            response.Description = "Personel Parola Bilgileri Başarıyla Güncellendi";
            return Json(response);
        }

    

    }
}