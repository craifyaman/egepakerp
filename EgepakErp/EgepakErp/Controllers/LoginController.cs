using EgePakErp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EgePakErp.Controllers
{
    public class LoginController : Controller
    {
        //GET:Login
        public Db Db;
        public LoginController()
        {
            Db = new Db();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (CookieHelper.Exists("egepakUserName") && CookieHelper.Exists("egepakPassword"))
            {
                ViewBag.UserName = CookieHelper.Get("egepakUserName");
                ViewBag.Password = CookieHelper.Get("egepakPassword");
                ViewBag.Remember = "Checked";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login(string username, string password, string returnUrl,string remember="off")
        {
            var user = Db.Personel
                .Include("PersonelTip")
                .Include("PersonelArayuzKisitlama")
                .Include("PersonelKisitlamaRelation")
                .Include("PersonelKisitlamaRelation.Kisitlama")
                .FirstOrDefault(i => i.Eposta == username && i.Parola == password);

            if (user != null)
            {
                
                CookieHelper.Set("egepakPersonelId", user.PersonelId.ToString(),1);
                if (remember=="on")
                {
                    CookieHelper.Set("egepakUserName", username, DateTime.Now.AddYears(1));
                    CookieHelper.Set("egepakPassword", password, DateTime.Now.AddYears(1));
                }
                else
                {
                    CookieHelper.Delete("egepakUserName");
                    CookieHelper.Delete("egepakPassword");
                }
               

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    return Redirect(returnUrl);
                }

            }
            else
            {
                return RedirectToAction("index", "login");

            }
        }
        public ActionResult Logout()
        {
            CookieHelper.Delete("PersonelId");
            return RedirectToAction("index", "login");
        }

         
    }
}
