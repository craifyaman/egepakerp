using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgepakErp.Enums;
using EgepakErp.Helper;
using EgePakErp.Controllers;
using EgePakErp.Models;
using System.Data.Entity;


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


        public List<UretimTeminSekli> baseUretimTeminSekli
        {
            get
            {
                if (Session["UretimTeminSekli"] == null)
                {
                    var list = db.UretimTeminSekli.ToList();
                    Session["UretimTeminSekli"] = list;
                    return list;
                }
                else
                {
                    return (List<UretimTeminSekli>)Session["UretimTeminSekli"];
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

        public string ScriptVersion { get { return ConfigurationManager.AppSettings["scriptVersion"].ToString(); } set { } }

        public List<Ulke> baseUlke()
        {
            return db.Ulke.ToList();
        }
        public List<Il> baseIl(int? ulkeId)
        {
            ulkeId = ulkeId == null ? 1 : ulkeId.Value;
            return db.Il.Where(i => i.UlkeId == ulkeId).ToList();
        }

        //public IEnumerable<SelectListItem> BaseBirimSelectList(int? birimId)
        //{
        //    var result = db.HammaddeBirimi.Select(x => new SelectListItem()
        //    {
        //        Value = x.Id.ToString(),
        //        Text = x.Birimi,
        //        Selected = x.Id == birimId
        //    }).ToList();

        //    return result;
        //}
        public List<Ilce> baseIlce(int ilId)
        {
            return db.Ilce.Where(i => i.IlId == ilId).ToList();
        }

        public List<HammaddeHareket> baseHammaddeHareket(int id)
        {
            return db.HammaddeHareket
                .Include("Tedarikci")
                .Where(i => i.HammaddeCinsiId == id).ToList();
        }

        public List<Cari> baseCari
        {
            get
            {
                if (Session["Cari"] == null)
                {
                    var list = db.Cari.Include("BaglantiTipi").ToList();
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

        public List<UretimTeminSekli> baseUretimTeminSekli
        {
            get
            {
                if (Session["UretimTeminSekli"] == null)
                {
                    var list = db.UretimTeminSekli.ToList();
                    Session["UretimTeminSekli"] = list;
                    return list;
                }
                else
                {
                    return (List<UretimTeminSekli>)Session["UretimTeminSekli"];
                }
            }
            set { }
        }

        public List<HammaddeCinsi> baseHammaddeCinsi
        {
            get
            {
                if (Session["HammaddeCinsi"] == null)
                {
                    var list = db.HammaddeCinsi.ToList();
                    Session["HammaddeCinsi"] = list;
                    return list;
                }
                else
                {
                    return (List<HammaddeCinsi>)Session["HammaddeCinsi"];
                }
            }
            set { }
        }

        public List<Urun> baseUrun
        {
            get
            {
                if (Session["Urun"] == null)
                {
                    var list = db.Urun.Include("UrunCinsi").ToList();
                    Session["Urun"] = list;
                    return list;
                }
                else
                {
                    return (List<Urun>)Session["Urun"];
                }
            }
            set { }
        }
        public List<Kalip> baseKalip
        {
            get
            {
                if (Session["Kalip"] == null)
                {
                    var list = db.Kalip.ToList();
                    Session["Kalip"] = list;
                    return list;
                }
                else
                {
                    return (List<Kalip>)Session["Kalip"];
                }
            }
            set { }
        }

        public List<Doviz> baseDoviz
        {
            get
            {
                var list = db.Doviz.ToList();
                return list;
            }
            set { }
        }


        public List<TableHammaddeBirim> baseHammaddeBirimi
        {
            get
            {
                var list = db.TableHammaddeBirim.ToList();
                return list;
            }
            set { }
        }

        public List<HammaddeHareket> YaldizMalzemeListe()
        {
            var Kategori = db.Kategori.FirstOrDefault(x => x.Adi.Contains("yaldız"));
            if(Kategori == null)
            {
                return null;
            }
            var liste = db.HammaddeHareket
                .OrderByDescending(x => x.KayitTarihi)
                .Where(x => x.KategoriId == Kategori.KategoriId)
                .Include(x=>x.TableHammaddeBirim)
                .ToList();
            return liste;
            
        }



        public decimal PonponSonFiyat()
        {
            var fatura = db.HammaddeHareket.OrderByDescending(x => x.HammaddeGirisTarihi).FirstOrDefault(x => x.UrunAdi.Contains("PONPON"));
            decimal fiyat = 0;
            if (fatura != null)
            {
                fiyat = fatura.BirimFiyat;
            }
            return fiyat;
        }

        public decimal TozBoyaSonBirimFiyat()
        {
            decimal fiyat = 0;

            var query = db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("toz"));
            if (query != null)
            {
                fiyat = query.BirimFiyat;
            }
            return fiyat;
        }
        public List<HammaddeHareket> BaseHammaddeHareketler(string urunAdi)
        {
            var list = db.HammaddeHareket.Include("Doviz").Where(x => x.UrunAdi.Contains(urunAdi)).ToList();
            return list;
        }
        public List<HammaddeHareket> BaseHammaddeHareketler()
        {
            var list = db.HammaddeHareket
                .Include("Doviz")
                .Include(x=>x.HammaddeCinsi)
                .ToList();
            return list;
        }
        public HammaddeHareket KoliSohHareket()
        {
            var liste = db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            return liste;
        }

        public List<HammaddeHareket> BaseSarfMalzeme()
        {
            var id = db.HammaddeCinsi.FirstOrDefault(x => x.Adi == "SARF MALZEME").HammaddeCinsiId;
            return db.HammaddeHareket
              .Include("Tedarikci")
              .Where(i => i.HammaddeCinsiId == id).ToList();
        }


        public List<TableHammaddeBirim> BaseHammaddeBirim()
        {
            var list = db.TableHammaddeBirim.ToList();
            return list;
        }
        public List<HazirMalzemeFiyat> BaseHazirMalzeme()
        {
            var list = db.HazirMalzemeFiyat.ToList();
            return list;
        }
        public List<Fiyat> BaseFiyat()
        {
            var list = db.Fiyat
                .Include(x=>x.Doviz)
                .ToList();
            return list;
        }

        public decimal BaseKur(string kurType, DateTime date)
        {
            if(date == null)
            {
                date = DateTime.Now;
            }
            if(kurType == EDoviz.USD.ToString())
            {
                var usdKur = DovizHelper.DovizKuruGetir("USD", date);
                return usdKur;
            }

            if(kurType == EDoviz.EUR.ToString())
            {
                var eurKur = DovizHelper.DovizKuruGetir("EUR", date);
                return eurKur;
            }

            return 0;
        }

        public List<BaseMenu> baseMenu(string pre = "Menu", string nameSpace = "EgePakErp.Controllers")
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace == nameSpace).ToList();

                    var cList= controlleractionlist.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                        Attribute = x.GetCustomAttributes().FirstOrDefault(f => f.GetType().Name == pre + "Attribute"),
                        NameSpace = nameSpace
                    });
                     
                    var list= cList.Where(x => x.Attributes.Contains(pre))
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
            return list;


        }
        public override void Execute()
        {
        }

    }
}