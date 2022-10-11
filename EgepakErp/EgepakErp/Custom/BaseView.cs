using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Enums;
using EgePakErp.Helper;
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
                var list = db.UretimTeminSekli.ToList();
                return list;
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
        public List<HammaddeHareket> baseHammaddeHareket()
        {
            return db.HammaddeHareket
                .Include("Tedarikci").ToList();
        }
        public List<Cari> baseCari
        {
            get
            {
                var list = db.Cari
               .Include(x => x.CariGrup)
               .Include(x => x.Kisi)
               .Include(x => x.Il)
               .Include(x => x.Ilce)
               .Include(x => x.BaglantiTipi)
                    .ToList();
                return list;
            }
            set { }
        }
      
        public List<GorusmeTip> baseGorusmeTip
        {
            get
            {
                var list = db.GorusmeTip.ToList();
                return list;
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
                var list = db.CariGrup.ToList();
                return list;
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
                var list = db.UrunCinsi.ToList();
                return list;
            }
            set { }
        }

        public List<UretimTeminSekli> baseUretimTeminSekli
        {
            get
            {
                var list = db.UretimTeminSekli.ToList();
                return list;
            }
            set { }
        }

        public List<HammaddeCinsi> baseHammaddeCinsi
        {
            get
            {
                var list = db.HammaddeCinsi.ToList();
                return list;

            }
            set { }
        }

        public List<Urun> baseUrun
        {
            get
            {
                var list = db.Urun.Include("UrunCinsi").ToList();
                return list;
            }
            set { }
        }
        public List<Kalip> baseKalip
        {
            get
            {
                var list = db.Kalip.ToList();
                return list;
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
            if (Kategori == null)
            {
                return null;
            }
            var liste = db.HammaddeHareket
                .OrderByDescending(x => x.KayitTarihi)
                .Where(x => x.KategoriId == Kategori.KategoriId)
                .Include(x => x.TableHammaddeBirim)
                .ToList();
            return liste;

        }
        public IQueryable<Yaldiz> BaseYaldiz()
        {
            var liste = db.Yaldiz.AsQueryable();
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
        public List<HammaddeHareket> BaseTutkal()
        {
            var tutkalId = db.HammaddeCinsi.FirstOrDefault(x => x.Adi.ToLower().Contains("tutkal")).HammaddeCinsiId;
            var list = db.HammaddeHareket.Include("Doviz").Where(x => x.HammaddeCinsiId == tutkalId).ToList();
            return list;
        }

        public List<HammaddeHareket> BaseHammaddeHareketler()
        {
            var list = db.HammaddeHareket
                .Include("Doviz")
                .Include(x => x.HammaddeCinsi)
                .ToList();
            return list;
        }
        public HammaddeHareket KoliSohHareket()
        {
            var liste = db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).FirstOrDefault(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı"));
            return liste;
        }
        public List<HammaddeHareket> KoliHareketler()
        {
            var liste = db.HammaddeHareket.OrderByDescending(x => x.KayitTarihi).Where(x => x.UrunAdi.Contains("koli") && !x.UrunAdi.ToLower().Contains("bantı")).ToList();
            return liste;
        }
        public List<KoliTur> BaseKoliTur()
        {
            var liste = db.KoliTur.ToList();
            return liste;
        }
        public List<KoliTur> BaseKoliTur(string Kod)
        {
            var liste = db.KoliTur.Where(x => x.Kod == Kod).ToList();
            return liste;
        }
        public List<HammaddeHareket> BaseSarfMalzeme()
        {
            var katId = db.Kategori.FirstOrDefault(x => x.Adi == "sarf malzeme").KategoriId;
            return db.HammaddeHareket
              .Include("Tedarikci")
              .Where(i => i.KategoriId == katId).ToList();
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
                .Include(x => x.Doviz)
                .ToList();
            return list;
        }
        public List<UretimSabitler> BaseUretimSabitler()
        {
            var list = db.UretimSabitler.ToList();
            return list;
        }
        public List<Kalip> BaseFircaListe()
        {
            var list = db.Kalip
                .Where(x => x.Adi.ToLower().Contains("fırça"))
                .ToList();
            return list;
        }
        public List<Kalip> BaseKalipListe(string urunAdi)
        {
            var list = db.Kalip
                .Where(x => x.Adi.ToLower().Contains(urunAdi))
                .ToList();
            return list;
        }

        public List<SiparisKalip> BaseSiparisKalipListe()
        {
            var list = db.SiparisKalip
                .Include(x => x.TozBoyaKod)
                .Include(x => x.SpreyBoyaKod)
                .Include(x => x.Siparis)
                //.Include(x => x.Yaldiz)
                .Include(x => x.MetalizeKod)
                .ToList();
            return list;
        }
        public IQueryable<SiparisKalip> BaseSiparisKalipListeQ()
        {
            var list = db.SiparisKalip
                .Include(x => x.TozBoyaKod)
                .Include(x => x.SpreyBoyaKod)
                .Include(x => x.Siparis)
                //.Include(x => x.Yaldiz)
                .AsQueryable();
            return list;
        }
        public IQueryable<SiparisKalip> BaseSiparisKalipListe(int siparisId)
        {
            var list = db.SiparisKalip
                .Include(x => x.TozBoyaKod)
                .Include(x => x.SpreyBoyaKod)
                .Include(x => x.Siparis)
                //.Include(x => x.Yaldiz)
                .Include(x => x.MetalizeKod)
                .Include(x => x.GranulKod)
                .Where(x => x.SiparisId == siparisId)
                .AsQueryable();

            return list;
        }
        public IQueryable<Siparis> BaseSiparis()
        {
            var list = db.Siparis
                .Include(x => x.Cari)
                .AsQueryable();
            return list;
        }
        public IQueryable<SiparisDurum> BaseSiparisDurum()
        {
            var list = db.SiparisDurum
                .AsQueryable();
            return list;
        }
        public IQueryable<Makine> BaseMakine()
        {
            var list = db.Makine
                .AsQueryable();
            return list;
        }
        public IQueryable<UretimEmirDurum> BaseUretimEmirDurum()
        {
            var list = db.UretimEmirDurum
                .AsQueryable();
            return list;
        }
        public IQueryable<UretimEmir> BaseUretimEmir()
        {
            var list = db.UretimEmir
                .Include(x=>x.SiparisKalip)
                .AsQueryable();
            return list;
        }
        public IQueryable<Kisi> BaseMontajKisi()
        {
            var list = db.Kisi
                .AsQueryable();
            return list;
        }
        public IQueryable<Kisi> BaseKisi()
        {
            var list = db.Kisi
                .AsQueryable();
            return list;
        }
        public IQueryable<Cari> BaseCari()
        {
            var list = db.Cari
               .Include(x => x.CariGrup)
               .Include(x => x.Kisi)
               .Include(x => x.Il)
               .Include(x => x.Ilce)
               .Include(x => x.BaglantiTipi)
               .AsQueryable();
            return list;

        }

        public decimal BaseKur(string kurType, DateTime date)
        {
            var dovizKur = db.DovizKur.FirstOrDefault();
            if (date == null)
            {
                date = DateTime.Now;
            }
            if (kurType == EDoviz.USD.ToString())
            {
                //var usdKur = DovizHelper.DovizKuruGetir("USD", date);
                var usdKur = dovizKur.UsdKur;
                return usdKur;
            }

            if (kurType == EDoviz.EUR.ToString())
            {
                //var eurKur = DovizHelper.DovizKuruGetir("EUR", date);
                var eurKur = dovizKur.EurKur;
                return eurKur;
            }

            return 0;
        }

        public DovizKur BaseDovizKur()
        {
            var kur = db.DovizKur.FirstOrDefault();
            return kur;
        }
        public IQueryable<BoyaKod> BaseBoyaKod()
        {
            var liste = db.BoyaKod.AsQueryable();
            return liste;
        }
        public IQueryable<BoyaKaplama> BaseBoyaKaplama()
        {
            var liste = db.BoyaKaplama.AsQueryable();
            return liste;
        }
        public IQueryable<BoyaKodType> BaseBoyaKodType()
        {
            var liste = db.BoyaKodType.AsQueryable();
            return liste;
        }
        public IQueryable<BoyaKaplamaType> BaseBoyaKaplamaType()
        {
            var liste = db.BoyaKaplamaType.AsQueryable();
            return liste;
        }
        public IQueryable<Kalip> BaseKalip()
        {
            var liste = db.Kalip.AsQueryable();
            return liste;
        }
        public Kalip KalipFindByKalipKod(string kod)
        {
            var kalip = db.Kalip.FirstOrDefault(x => x.ParcaKodu == kod);
            return kalip;
        }
        public IQueryable<Aksiyon> BaseAksiyon()
        {
            var liste = db.Aksiyon
                .Include(x => x.AksiyonType)
                .AsQueryable();
            return liste;
        }
        public IQueryable<AksiyonType> BaseAksiyonType()
        {
            var liste = db.AksiyonType.AsQueryable();
            return liste;
        }
        public IQueryable<StokHareketType> BaseStokHareketType()
        {
            var liste = db.StokHareketType.AsQueryable();
            return liste;
        }
        public IQueryable<StokHareket> BaseStokHareket()
        {
            var liste = db.StokHareket.AsQueryable();
            return liste;
        }
        //public List<EvMontaj> BaseEvMontaj()
        //{
        //    var liste = db.EvMontaj.ToList();
        //    return liste;
        //}

        public List<BaseMenu> baseMenu(string pre = "Menu", string nameSpace = "EgePakErp.Controllers")
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace == nameSpace).ToList();

            var cList = controlleractionlist.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
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

            var list = cList.Where(x => x.Attributes.Contains(pre))
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