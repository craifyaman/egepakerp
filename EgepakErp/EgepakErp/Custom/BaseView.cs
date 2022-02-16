using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Controllers;
using EgePakErp.Models;

namespace EgePakErp.Custom
{
    public class BaseView : WebViewPage
    {
        private Db db;
        public BaseView()
        {
            db = new Db();
        }
        public BaseController Controller => ViewContext.Controller as BaseController;
        public Personel CurrentUser => Controller.CurrentUser;

        public List<Departman> baseDepartman
        {
            get
            {
                if (Session["Departman"] == null)
                {
                    var list = db.Departman.ToList();
                    Session["Departman"] = list;
                    return list;
                }
                else
                {
                    return (List<Departman>)Session["Departman"];
                }
            }
            set { }
        }

        public List<PersonelTip> basePersonelTip
        {
            get
            {
                if (Session["PersonelTip"] == null)
                {
                    var list = db.PersonelTip.ToList();
                    Session["PersonelTip"] = list;
                    return list;
                }
                else
                {
                    return (List<PersonelTip>)Session["PersonelTip"];
                }
            }
            set { }
        }

        public override void Execute()
        {

        }

    }

    public class BaseView<T> : WebViewPage<T>
    {

        private Db db;

        public BaseView()
        {
            db = new Db();

        }
        public BaseController Controller => ViewContext.Controller as BaseController;
        public Personel CurrentUser => Controller.CurrentUser;

        public List<Ulke> baseUlke()
        {
            return db.Ulke.ToList();
        }
        public List<Il> baseIl(int? ulkeId)
        {
            ulkeId = ulkeId == null ? 1 : ulkeId.Value;
            return db.Il.Where(i => i.UlkeId == ulkeId).ToList();
        }
        public List<Ilce> baseIlce(int ilId)
        {
            return db.Ilce.Where(i => i.IlId == ilId).ToList();
        }
        public List<Cari> baseCari
        {
            get
            {
                if (Session["Cari"] == null)
                {
                    var list = db.Cari.Where(i => !string.IsNullOrEmpty(i.Unvan)).ToList().Select(s => new Cari { CariId = s.CariId, Unvan = s.Unvan }).ToList();
                    Session["Cari"] = list;
                    return list;
                }
                else
                {
                    return (List<Cari>)Session["Cari"];
                }
            }
            set { }
        }
        public List<GorusmeTip> baseGorusmeTip
        {
            get
            {
                if (Session["GorusmeTip"] == null)
                {
                    var list = db.GorusmeTip.ToList();
                    Session["GorusmeTip"] = list;
                    return list;
                }
                else
                {
                    return (List<GorusmeTip>)Session["GorusmeTip"];
                }
            }
            set { }
        }

        public List<Personel> basePersonel
        {
            get
            {
                var list = db.Personel.ToList();
                return list;
            }
            set { }
        }

        public List<CariGrup> baseCariGrup
        {
            get
            {
                if (Session["CariGrup"] == null)
                {
                    var list = db.CariGrup.ToList();
                    Session["CariGrup"] = list;
                    return list;
                }
                else
                {
                    return (List<CariGrup>)Session["CariGrup"];
                }
            }
            set { }
        }

        public List<Departman> baseDepartman
        {
            get
            {
                if (Session["Departman"] == null)
                {
                    var list = db.Departman.ToList();
                    Session["Departman"] = list;
                    return list;
                }
                else
                {
                    return (List<Departman>)Session["Departman"];
                }
            }
            set { }
        }
        public List<PersonelTip> basePersonelTip
        {
            get
            {
                if (Session["PersonelTip"] == null)
                {
                    var list = db.PersonelTip.ToList();
                    Session["PersonelTip"] = list;
                    return list;
                }
                else
                {
                    return (List<PersonelTip>)Session["PersonelTip"];
                }
            }
            set { }
        }


        public Dictionary<string, string> baseDurum
        {
            get
            {
                Dictionary<string, string> _dictonary = new Dictionary<string, string>()
                    {
                        {"true","Aktif"},
                        {"false","Pasif"},
                    };

                return _dictonary;
            }
            set { }
        }

        public List<UrunCinsi> baseUrunCinsi
        {
            get
            {
                if (Session["UrunCinsi"] == null)
                {
                    var list = db.UrunCinsi.ToList();
                    Session["UrunCinsi"] = list;
                    return list;
                }
                else
                {
                    return (List<UrunCinsi>)Session["UrunCinsi"];
                }
            }
            set { }
        }

        public List<BaseMenu> baseMenu(string pre = "Menu", string nameSpace = "EgePakErp.Controllers")
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
                    .Select(x => new BaseMenu
                    {
                        Parent = x.Attribute.GetType().GetProperty("Parent").GetValue(x.Attribute, null).ToString(),
                        Title = x.Attribute.GetType().GetProperty("Title").GetValue(x.Attribute, null).ToString(),
                        Icon = x.Attribute.GetType().GetProperty("Icon").GetValue(x.Attribute, null).ToString(),
                        Order = Convert.ToInt32(x.Attribute.GetType().GetProperty("Order").GetValue(x.Attribute, null).ToString()),
                        ParentOrder = Convert.ToInt32(x.Attribute.GetType().GetProperty("ParentOrder").GetValue(x.Attribute, null).ToString()),
                        Url = "/" + x.Controller.Replace("Controller", "") + "/" + x.Action,

                    })
                    .OrderBy(x => x.ParentOrder)
                    .ThenBy(x => x.Order)
                    .ToList();
            return controlleractionlist;


        }
        public override void Execute()
        {
        }

    }
}