using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EgePakErp.Custom;
using EgePakErp.Helper;
using Newtonsoft.Json;

namespace EgePakErp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        private Db _db;
        public Db Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new Db();
                }
                return _db;
            }
        }

        public Personel CurrentUser
        {
            get
            {
                return (Personel)HttpContext.Session["Personel"];
            }
            set
            {
                HttpContext.Session["Personel"] = value;
            }
        }
        public List<Kisitlama> Kisitlamalar
        {
            get
            {
                var list = CurrentUser.PersonelKisitlamaRelation.Select(s => s.Kisitlama).ToList();
                return list;
            }
            protected set { }
        }

        public List<dynamic> YetkiControlActionList(string pre = "Yetki", string nameSpace = "EgePakErp.Controllers")
        {

            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace == nameSpace)
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                        Attribute = x.GetCustomAttributes().FirstOrDefault(f => f.GetType().Name == pre + "Attribute"),
                        NameSpace = nameSpace
                    })
                    .Where(x => x.Attributes.Contains(pre))
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList<dynamic>();
            return controlleractionlist;


        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CookieHelper.Exists("egepakPersonelId"))
            {
                string redirectUrl = string.Format("?returnUrl={0}",
                            filterContext.HttpContext.Request.Url.PathAndQuery);
                filterContext.Result = Redirect("/Login/" + redirectUrl);
            }
            else
            {
                var personelId = Convert.ToInt32(CookieHelper.Get("egepakPersonelId"));
                CurrentUser= Db.Personel
                .Include("PersonelTip")
                .Include("PersonelArayuzKisitlama")
                .Include("PersonelKisitlamaRelation")
                .Include("PersonelKisitlamaRelation.Kisitlama")
                .FirstOrDefault(i => i.PersonelId==personelId);

                //Kısıtlaması var mı yok mu kontrol et
                var actionName = filterContext.ActionDescriptor.ActionName;
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";

                if (YetkiControlActionList().Any(i => i.Controller == controllerName && i.Action == actionName))
                {
                    var yetki = Kisitlamalar.FirstOrDefault(i => i.Controller == controllerName && i.Action == actionName);

                    if (yetki == null)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            // Log exception first
                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            JsonResult result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            result.Data = new Response
                            {
                                Description = "Bu işlemi yapmak için yetkiniz yok",
                                Success = false
                            };
                            filterContext.Result = result;
                        }
                        else
                        {
                            var TempData = filterContext.Controller.TempData;
                            TempData["Status"] = null;
                            filterContext.Result = RedirectToAction("Index", "Home");

                        }
                    }
                }

            }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            {
                // Log exception first
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                JsonResult result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                result.ContentType = "application/json";
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                if (ConfigurationManager.AppSettings["Relase"].ToString() == "development")
                {
                    result.Data = new Response
                    {
                        Description = filterContext.Exception.InnerException?.InnerException?.Message + "<br/>" + filterContext.Exception.Message,
                        Success = false
                    };
                }
                else
                {
                    result.Data = new Response
                    {
                        //ex = filterContext.Exception,
                        Description = "İşlem sırasında hata oluştu. Lütfen tekrar deneyin. Hatanın devam etmesi halinde teknik desteğe başvurun",
                        Success = false
                    };
                }
               

               
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                base.OnException(filterContext);
            }
        }

    }
}