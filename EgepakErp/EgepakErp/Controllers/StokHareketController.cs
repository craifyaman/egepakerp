using EgePakErp.Concrete;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Migrations;
using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class StokHareketController : BaseController
    {
        public StokHareketRepository repo { get; set; }
        public SiparisKalipRepository siparisKalipRepo { get; set; }
        public YaldizRepository yaldizRepo { get; set; }
        public BoyaKodRepository boyaKodRepo { get; set; }
        public BoyaKaplamaRepository boyaKaplamaRepo { get; set; }
        public KalipRepository kalipRepo { get; set; }
        public string dtMetaField { get; set; }

        public StokHareketController()
        {
            repo = new StokHareketRepository();
            siparisKalipRepo = new SiparisKalipRepository();
            yaldizRepo = new YaldizRepository();
            boyaKodRepo = new BoyaKodRepository();
            kalipRepo = new KalipRepository();
            boyaKaplamaRepo = new BoyaKaplamaRepository();

            dtMetaField = "StokHareketId";
        }

        [Menu("Depo", "fas fa-warehouse icon-xl", "Depo", 0, 1)]
        public ActionResult Index()
        {
            var model = repo.GetAll();
            return View(model.ToList());
        }
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "StokHareketId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[adi]"]))
            {
                var adi = Request.Form["query[adi]"].ToString();
                model = model.Where(i => i.Adi.ToLower().Contains(adi.ToLower()));
            }

            if (!string.IsNullOrEmpty(Request.Form["query[siparis]"]))
            {
                var siparis = Request.Form["query[siparis]"].ToString();
                model = model.Where(i => i.Siparis.SiparisIsim.ToLower().Contains(siparis.ToLower()));
            }

            if (!string.IsNullOrEmpty(Request.Form["query[urun]"]))
            {
                var urun = Request.Form["query[urun]"].ToString();
                model = model.Where(i => (i.Siparis.Urun.UrunCinsi.Kisaltmasi.ToLower()+ i.Siparis.Urun.UrunNo).Contains(urun.ToLower()));
            }

            if (!string.IsNullOrEmpty(Request.Form["query[cariId]"]))
            {
                var cariId = Convert.ToInt32(Request.Form["query[cariId]"].ToString());
                if(cariId != 0)
                {
                    model = model.Where(i => i.Siparis.CariId == cariId);
                }
                
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("StokHareketId Desc");
                dtMeta.field = "StokHareketId";
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;

            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //dto yap burda
            var dto = model.AsEnumerable().Select(i => new
            {
                Id = i.StokHareketId,
                Adi = i.Adi,
                Type = i.StokHareketType.Type,
                SiparisId = i.SiparisId,
                Yer = i.Yer,
                SiparisAdi = i.Siparis.SiparisIsim,
                CariId = i.Siparis.CariId,
                Cari = i.Siparis.Cari.Unvan,
                Adet = i.Adet,
                DepoToplam = i.Toplam,
                Kalan = i.DepodaKalanAdet,
                MontajliMi = i.MontajliMi,
                Urun = i.Siparis.Urun.TamAd
            }).ToList<dynamic>();

            dtModel.meta = dtMeta;
            dtModel.data = dto;
            return Json(dtModel);

        }
        //public JsonResult Liste()
        //{
        //    bool montajli = false;

        //    if (!string.IsNullOrEmpty(Request.Form["query[MontajliMi]"]))
        //    {
        //        montajli = Request.Form["query[MontajliMi]"].ToLower() == "true" ? true : false;
        //    }

        //    //kabasını aldır
        //    var dtModel = new DataTableModel<dynamic>();
        //    var dtMeta = new DataTableMeta();

        //    dtMeta.field = Request.Form["sort[field]"] == null ? dtMetaField : Request.Form["sort[field]"];
        //    dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

        //    dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
        //    dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

        //    var model = repo.GetAll();
        //    var allList = repo.GetAll().ToList();
        //    var yaldizList = yaldizRepo.GetAll();
        //    var BoyaKodList = boyaKodRepo.GetAll();
        //    var BoyaKaplamaList = boyaKaplamaRepo.GetAll();
        //    var YaldizPdfList = yaldizRepo.GetAll();

        //    var montajliKaliplar = repo.GetAll(x => x.MontajliMi).ToList();
        //    var yaldizKalipList = siparisKalipRepo.GetAll(x => x.YaldizKodList != null);
        //    //var yaldizKalipList = siparisKalipRepo.GetAll(x => x.YaldizId != null);
        //    var tozBoyaKodKalipList = siparisKalipRepo.GetAll(x => x.TozBoyaKodList != null);
        //    var kalipListe = kalipRepo.GetAll();

        //    if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
        //    {
        //        string q = Request.Form["query[searchQuery]"].ToString();
        //        model = model
        //            .Where(
        //            x => x.Siparis.SiparisAdi.Contains(q) ||
        //            x.SiparisKalip.KalipKod == q ||
        //            x.SiparisKalip.KalipAdi.Contains(q)
        //            );
        //    }

        //    if (!string.IsNullOrEmpty(Request.Form["query[cariId]"]))
        //    {
        //        int cariId = Convert.ToInt32(Request.Form["query[cariId]"].ToString());
        //        model = model.Where(x => x.Siparis.CariId == cariId);
        //    }

        //    if (!string.IsNullOrEmpty(Request.Form["query[yer]"]))
        //    {
        //        string yer = Request.Form["query[cariId]"].ToString();
        //        model = model.Where(x => x.Yer.Contains(yer));
        //    }
        //    if (!string.IsNullOrEmpty(Request.Form["query[adi]"]))
        //    {
        //        string adi = Request.Form["query[adi]"].ToString();
        //        model = model.Where(x => (x.SiparisKalip.EnjeksiyonRenk+" "+x.SiparisKalip.KalipAdi).Contains(adi));
        //    }

        //    try
        //    {
        //        model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
        //    }

        //    catch (Exception)
        //    {
        //        model = model.OrderBy(dtMetaField + " Desc");
        //        dtMeta.field = dtMetaField;
        //        dtMeta.sort = "Desc";
        //    }

        //    var count = model.Count();

        //    dtMeta.total = count;
        //    dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;


        //    //sayfala
        //    model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

        //    //kalıplar birbirine montajlı ise
        //    if (montajli)
        //    {
        //        var dto = model.AsEnumerable().Where(x => x.MontajliMi == true).GroupBy(x => x.MontajKod).Select(x =>
        //        {
        //            var temp = x.FirstOrDefault();
        //            var montajKod = temp.MontajKod;
        //            var montajliList = new List<StokHareket>();

        //            var GranulBoyalar = "";
        //            var Yaldizlar = "";
        //            var KalipAdlar = "";

        //            if (montajKod != null)
        //            {
        //                montajliList = allList.Where(a => a.MontajKod == montajKod).ToList();
        //            }

        //            var KalipKodList = temp.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == temp.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { temp.SiparisKalip.SiparisKalipId };

        //            foreach (var item in KalipKodList)
        //            {
        //                var siparisKalip = siparisKalipRepo.Get(item);
        //                var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
        //                if (_kalip != null)
        //                {
        //                    KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
        //                }

        //                var yaldizSiparisKalip = yaldizKalipList.Where(c => c.SiparisId == temp.SiparisId && c.KalipKod == siparisKalip.KalipKod && c.MaliyetType.ToLower() == "yaldiz").ToList();
        //                var tozBoyaKodSiparisKalip = tozBoyaKodKalipList.Where(c => c.SiparisId == temp.SiparisId && c.KalipKod == siparisKalip.KalipKod && c.MaliyetType.ToLower() == "tozboya").ToList();

        //                if (yaldizSiparisKalip != null && yaldizSiparisKalip.Count > 0)
        //                {
        //                    foreach (var y in yaldizSiparisKalip)
        //                    {
        //                        var yKodList = y.YaldizKodList.Split(',');
        //                        Yaldizlar += "<p class=\"BoyaKodList\">";
        //                        foreach (var yId in yKodList)
        //                        {
        //                            var id = Convert.ToInt32(yId);
        //                            Yaldizlar += _kalip.Adi + "  : " + YaldizPdfList.FirstOrDefault(a => a.YaldizId == id)?.Aciklama + "<br/>";
        //                        }
        //                        Yaldizlar += "</p>";
        //                    }
        //                }

        //                if (tozBoyaKodSiparisKalip != null && tozBoyaKodSiparisKalip.Count > 0)
        //                {
        //                    foreach (var b in tozBoyaKodSiparisKalip)
        //                    {
        //                        var bKodList = b.TozBoyaKodList.Split(',');
        //                        GranulBoyalar += "<p class=\"BoyaKodList\">";
        //                        foreach (var bId in bKodList)
        //                        {
        //                            var id = Convert.ToInt32(bId);
        //                            GranulBoyalar += _kalip.Adi + "  : " + BoyaKaplamaList.FirstOrDefault(a => a.BoyaKaplamaId == id)?.Aciklama + "<br/>";
        //                        }
        //                        GranulBoyalar += "</p>";
        //                    }
        //                }


        //            }

        //            dynamic ret = new
        //            {
        //                Id = temp.StokHareketId,
        //                Type = temp.StokHareketType.Type,
        //                SiparisId = temp.SiparisId,
        //                SiparisKalipId = temp.SiparisKalipId,
        //                KalipKodList = KalipAdlar,
        //                Yer = x.FirstOrDefault(y => y.Yer != null)?.Yer,
        //                Yaldiz = Yaldizlar,
        //                BoyaKod = GranulBoyalar,
        //                SiparisAdi = temp.Siparis.SiparisAdi,
        //                CariId = temp.Siparis.CariId,
        //                Cari = temp.Siparis.Cari.Unvan,
        //                Adet = x.FirstOrDefault(y => y.Adet != null && y.Adet != 0)?.Adet,
        //                Kalan = temp.DepodaKalanAdet,
        //                MontajliMi = temp.MontajliMi,
        //            };

        //            return ret;

        //        }).ToList();


        //        dtModel.meta = dtMeta;
        //        dtModel.data = dto.ToList<dynamic>();
        //        return Json(dtModel);
        //    }

        //    //kalıplar tek tek girecek ise
        //    else
        //    {
        //        var dto = model.AsEnumerable().Where(x => x.MontajliMi == false).Select(x =>
        //        {
        //            var montajKod = x.MontajKod;
        //            var montajliList = new List<StokHareket>();

        //            var GranulBoyalar = "";
        //            var Yaldizlar = "";
        //            var KalipAdlar = "";

        //            if (montajKod != null)
        //            {
        //                montajliList = allList.Where(a => a.MontajKod == montajKod).ToList();
        //            }

        //            var KalipKodList = x.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == x.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { x.SiparisKalip.SiparisKalipId };


        //            foreach (var item in KalipKodList)
        //            {
        //                var siparisKalip = siparisKalipRepo.Get(item);
        //                var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
        //                if (_kalip != null)
        //                {
        //                    KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
        //                }

        //                var yaldizSiparisKalip = yaldizKalipList.Where(c => c.SiparisId == x.SiparisId && c.KalipKod == siparisKalip.KalipKod && c.MaliyetType.ToLower() == "yaldiz").ToList();
        //                var tozBoyaKodSiparisKalip = tozBoyaKodKalipList.Where(c => c.SiparisId == x.SiparisId && c.KalipKod == siparisKalip.KalipKod && c.MaliyetType.ToLower() == "tozboya").ToList();

        //                if (yaldizSiparisKalip != null && yaldizSiparisKalip.Count > 0)
        //                {
        //                    foreach (var y in yaldizSiparisKalip)
        //                    {
        //                        var yKodList = y.YaldizKodList.Split(',');
        //                        Yaldizlar += "<p class=\"BoyaKodList\">";
        //                        foreach (var yId in yKodList)
        //                        {
        //                            var id = Convert.ToInt32(yId);
        //                            Yaldizlar += _kalip.Adi + "  : " + YaldizPdfList.FirstOrDefault(a => a.YaldizId == id)?.Aciklama + "<br/>";
        //                        }
        //                        Yaldizlar += "</p>";
        //                    }
        //                }


        //                if (tozBoyaKodSiparisKalip != null && tozBoyaKodSiparisKalip.Count > 0)
        //                {
        //                    foreach (var b in tozBoyaKodSiparisKalip)
        //                    {
        //                        GranulBoyalar += "<p class=\"BoyaKodList\">";
        //                        var bKodList = b.TozBoyaKodList.Split(',');
        //                        foreach (var bId in bKodList)
        //                        {
        //                            var id = Convert.ToInt32(bId);
        //                            GranulBoyalar += _kalip.Adi + "  : " + BoyaKaplamaList.FirstOrDefault(a => a.BoyaKaplamaId == id)?.Aciklama + "<br/>";
        //                        }
        //                        GranulBoyalar += "</p>";
        //                    }

        //                }


        //            }

        //            dynamic ret = new
        //            {
        //                Id = x.StokHareketId,
        //                Type = x.StokHareketType.Type,
        //                SiparisId = x.SiparisId,
        //                SiparisKalipId = x.SiparisKalipId,
        //                KalipKodList = KalipAdlar,
        //                Yer = x.Yer,
        //                Yaldiz = Yaldizlar,
        //                BoyaKod = GranulBoyalar,
        //                SiparisAdi = x.Siparis.SiparisAdi,
        //                CariId = x.Siparis.CariId,
        //                Cari = x.Siparis.Cari.Unvan,
        //                Adet = x.Adet,
        //                Kalan = x.DepodaKalanAdet,
        //                MontajliMi = x.MontajliMi,
        //            };

        //            return ret;

        //        }).ToList();


        //        dtModel.meta = dtMeta;
        //        dtModel.data = dto.ToList<dynamic>();
        //        return Json(dtModel);
        //    }

        //}

        //public JsonResult Liste()
        //{
        //    bool montajli = false;

        //    if (!string.IsNullOrEmpty(Request.Form["query[MontajliMi]"]))
        //    {
        //        montajli = Request.Form["query[MontajliMi]"].ToLower() == "true" ? true : false;
        //    }

        //    //kabasını aldır
        //    var dtModel = new DataTableModel<dynamic>();
        //    var dtMeta = new DataTableMeta();

        //    dtMeta.field = Request.Form["sort[field]"] == null ? dtMetaField : Request.Form["sort[field]"];
        //    dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

        //    dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
        //    dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

        //    var model = repo.GetAll();

        //    var montajliKaliplar = repo.GetAll(x => x.MontajliMi).ToList();
        //    var kalipListe = kalipRepo.GetAll();

        //    if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
        //    {
        //        string q = Request.Form["query[searchQuery]"].ToString();
        //        model = model
        //            .Where(
        //            x => x.Siparis.SiparisKod.Contains(q) ||
        //            x.SiparisKalip.KalipKod == q ||
        //            x.SiparisKalip.KalipAdi.Contains(q)
        //            );
        //    }

        //    if (!string.IsNullOrEmpty(Request.Form["query[cariId]"]))
        //    {
        //        int cariId = Convert.ToInt32(Request.Form["query[cariId]"].ToString());
        //        model = model.Where(x => x.Siparis.CariId == cariId);
        //    }

        //    if (!string.IsNullOrEmpty(Request.Form["query[yer]"]))
        //    {
        //        string yer = Request.Form["query[cariId]"].ToString();
        //        model = model.Where(x => x.Yer.Contains(yer));
        //    }
        //    if (!string.IsNullOrEmpty(Request.Form["query[adi]"]))
        //    {
        //        string adi = Request.Form["query[adi]"].ToString();
        //        model = model.Where(x => (x.SiparisKalip.EnjeksiyonRenk + " " + x.SiparisKalip.KalipAdi).Contains(adi));
        //    }

        //    try
        //    {
        //        model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
        //    }

        //    catch (Exception)
        //    {
        //        model = model.OrderBy(dtMetaField + " Desc");
        //        dtMeta.field = dtMetaField;
        //        dtMeta.sort = "Desc";
        //    }

        //    var count = model.Count();

        //    dtMeta.total = count;
        //    dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;


        //    //sayfala
        //    model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

        //    //kalıplar birbirine montajlı ise
        //    if (montajli)
        //    {
        //        var dto = model.AsEnumerable().Where(x => x.MontajliMi == true).GroupBy(x => x.MontajKod).Select(x =>
        //        {
        //            var temp = x.FirstOrDefault();
        //            var montajKod = temp.MontajKod;
        //            var montajliList = new List<StokHareket>();
        //            var KalipAdlar = "";
        //            var KalipKodList = temp.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == temp.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { temp.SiparisKalip.SiparisKalipId };

        //            foreach (var item in KalipKodList)
        //            {
        //                var siparisKalip = siparisKalipRepo.Get(item);
        //                var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
        //                if (_kalip != null)
        //                {
        //                    KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
        //                }


        //            }

        //            dynamic ret = new
        //            {
        //                Id = temp.StokHareketId,
        //                Type = temp.StokHareketType.Type,
        //                SiparisId = temp.SiparisId,
        //                SiparisKalipId = temp.SiparisKalipId,
        //                Yer = x.FirstOrDefault(y => y.Yer != null)?.Yer,
        //                SiparisKod = temp.Siparis.SiparisKod,
        //                CariId = temp.Siparis.CariId,
        //                Cari = temp.Siparis.Cari.Unvan,
        //                Adet = x.FirstOrDefault(y => y.Adet != null && y.Adet != 0)?.Adet,
        //                Kalan = temp.DepodaKalanAdet,
        //                MontajliMi = temp.MontajliMi,
        //            };

        //            return ret;

        //        }).ToList();


        //        dtModel.meta = dtMeta;
        //        dtModel.data = dto.ToList<dynamic>();
        //        return Json(dtModel);
        //    }

        //    //kalıplar tek tek girecek ise
        //    else
        //    {
        //        var dto = model.AsEnumerable().Where(x => x.MontajliMi == false).Select(x =>
        //        {
        //            var montajKod = x.MontajKod;
        //            var montajliList = new List<StokHareket>();
        //            var KalipAdlar = "";

        //            var KalipKodList = x.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == x.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { x.SiparisKalip.SiparisKalipId };


        //            foreach (var item in KalipKodList)
        //            {
        //                var siparisKalip = siparisKalipRepo.Get(item);
        //                var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
        //                if (_kalip != null)
        //                {
        //                    KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
        //                }
        //            }

        //            dynamic ret = new
        //            {
        //                Id = x.StokHareketId,
        //                Type = x.StokHareketType.Type,
        //                SiparisId = x.SiparisId,
        //                SiparisKalipId = x.SiparisKalipId,
        //                KalipKodList = KalipAdlar,
        //                Yer = x.Yer,
        //                SiparisKod = x.Siparis.SiparisKod,
        //                CariId = x.Siparis.CariId,
        //                Cari = x.Siparis.Cari.Unvan,
        //                Adet = x.Adet,
        //                Kalan = x.DepodaKalanAdet,
        //                MontajliMi = x.MontajliMi,
        //            };

        //            return ret;

        //        }).ToList();


        //        dtModel.meta = dtMeta;
        //        dtModel.data = dto.ToList<dynamic>();
        //        return Json(dtModel);
        //    }

        //}

        [Yetki("Detay Listesi", "Depo")]
        public ActionResult Detay(int stokHareketId)
        {
            var model = repo.Get(stokHareketId);
            return View(model);
        }
        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);

            return PartialView(model);
        }

        public PartialViewResult CikisHareketForm(int id)
        {
            var model = Db.StokCikisHareket.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CikisHareketSil(int id)
        {
            var response = new Response();
            var hareket = Db.StokCikisHareket.Find(id);
            Db.StokCikisHareket.Remove(hareket);
            Db.SaveChanges();
            response.Success = true;
            response.Description = "Kayıt Silindi";
            return Json(response);
        }


        public PartialViewResult GirisHareketForm(int id)
        {
            var model = Db.StokGirisHareket.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GirisHareketSil(int id)
        {
            var response = new Response();
            var hareket = Db.StokGirisHareket.Find(id);
            Db.StokGirisHareket.Remove(hareket);
            Db.SaveChanges();
            response.Success = true;
            response.Description = "Kayıt Silindi";
            return Json(response);
        }



        public PartialViewResult FilterForm()
        {
            return PartialView();
        }

        public JsonResult Kaydet(StokHareket form)
        {
            var response = new Response();

            if (form.StokHareketId == 0)
            {
                var hareket = repo.Get(x => x.Adi.ToLower() == form.Adi.ToLower());
                if (hareket != null)
                {
                    //depoda ürün varsa stok güncelle
                    hareket.Adet += form.Adet;
                    repo.Update(hareket);
                    response.Description = "Stok Güncellendi";
                    response.Success = true;
                }
                else
                {
                    repo.Insert(form);
                    response.Success = true;
                    response.Description = "Kayıt edildi.";
                }

            }
            else
            {
                var entity = repo.Get(form.StokHareketId);
                if (entity != null)
                {
                    //alanları güncelle
                    var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                    foreach (var prop in propList)
                    {
                        if (form.Include.Contains(prop.Name))
                        {
                            prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                        }
                    }
                    repo.Update(entity);
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }

            return Json(response);

        }


        public JsonResult CikisHareketKaydet(StokCikisHareket form)
        {
            var response = new Response();

            if (form.StokCikisHareketId == 0)
            {
                Db.StokCikisHareket.Add(form);
                Db.SaveChanges();
                response.Description = "Eklendi";
                response.Success = true;

            }
            else
            {
                var entity = Db.StokCikisHareket.Find(form.StokCikisHareketId);
                if (entity != null)
                {
                    //alanları güncelle
                    var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                    foreach (var prop in propList)
                    {
                        if (form.Include.Contains(prop.Name))
                        {
                            prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                        }
                    }
                    Db.SaveChanges();
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }

            return Json(response);

        }

        public JsonResult GirisHareketKaydet(StokGirisHareket form)
        {
            var response = new Response();

            if (form.StokGirisHareketId == 0)
            {
                Db.StokGirisHareket.Add(form);
                Db.SaveChanges();
                response.Description = "Eklendi";
                response.Success = true;

            }
            else
            {
                var entity = Db.StokGirisHareket.Find(form.StokGirisHareketId);
                if (entity != null)
                {
                    //alanları güncelle
                    var propList = entity.GetType().GetProperties().Where(prop => !prop.IsDefined(typeof(NotMappedAttribute), false)).ToList();
                    foreach (var prop in propList)
                    {
                        if (form.Include.Contains(prop.Name))
                        {
                            prop.SetValue(entity, form.GetType().GetProperty(prop.Name).GetValue(form, null));
                        }
                    }
                    Db.SaveChanges();
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }

            return Json(response);

        }

        [HttpPost]
        public JsonResult Sil(int id)
        {
            var response = new Response();
            var hareket = repo.Get(id);
            repo.Delete(hareket);
            response.Success = true;
            response.Description = "Kayıt Silindi";
            return Json(response);

        }


        public JsonResult DepoyaAktarTekli(int SiparisKalipId, int SiparisId, int Adet, string Yer, int UretimEmirId, string UretimParcaAdi)
        {
            var response = new Response();
            var _hareket = repo.Get(x => x.UretimEmirIdList == UretimEmirId.ToString() + ",");
            if (_hareket != null)
            {
                //depoda ürün varsa stok güncelle
                StokGirisHareket girisHareket = new StokGirisHareket();
                girisHareket.Adet = Adet;
                girisHareket.GirisTarih = DateTime.Now;
                girisHareket.StokHareketId = _hareket.StokHareketId;
                girisHareket.Aciklama = "Üretimden Eklendi";
                Db.StokGirisHareket.Add(girisHareket);
                Db.SaveChanges();
                response.Description = "Giriş Hareketi Eklendi";
                response.Success = true;
            }
            else
            {
                StokHareket hareket = new StokHareket();
                hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
                hareket.SiparisId = SiparisId;
                hareket.Adet = Adet;
                hareket.MontajliMi = false;
                hareket.Yer = Yer;
                hareket.Adi = UretimParcaAdi;
                hareket.UretimEmirIdList = UretimEmirId.ToString();
                repo.Insert(hareket);
                response.Success = true;
                response.Description = "Ürün depoya aktarıldı";
                response.Success = true;
                response.Description = "Kayıt edildi.";
            }

            //üretim emrini depoda olarak işaretle
            var uretimEmir = Db.UretimEmir.Find(UretimEmirId);
            if (uretimEmir != null)
            {
                uretimEmir.DepodaMi = true;
                Db.SaveChanges();
            }

            return Json(response);

        }
        public JsonResult DepoyaAktarCoklu(List<SiparisDepoAktarimDto> liste)
        {
            var response = new Response();
            string uretimEmirIdList = "";
            liste.ForEach(x => uretimEmirIdList += x.UretimEmirId.ToString() + ",");
            var _hareket = repo.Get(x => x.UretimEmirIdList == uretimEmirIdList);
            if (_hareket != null)
            {
                //depoda ürün varsa stok güncelle
                StokGirisHareket girisHareket = new StokGirisHareket();
                girisHareket.Adet = liste.Sum(x => x.Adet);
                girisHareket.GirisTarih = DateTime.Now;
                girisHareket.StokHareketId = _hareket.StokHareketId;
                girisHareket.Aciklama = "Üretimden Eklendi";
                Db.StokGirisHareket.Add(girisHareket);
                Db.SaveChanges();
                response.Description = "Giriş Hareketi Eklendi";
                response.Success = true;
            }
            else
            {
                StokHareket hareket = new StokHareket();
                hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
                hareket.SiparisId = liste.FirstOrDefault().SiparisId;
                hareket.Adi = "";
                hareket.Yer = "";
                var adet = liste.FirstOrDefault(x => x.Adet != 0).Adet;
                hareket.Adet = adet;
                hareket.MontajliMi = false;
                hareket.UretimEmirIdList = uretimEmirIdList;

                foreach (var item in liste)
                {
                    hareket.Adi += " " + item.UretimParcaAdi;
                    hareket.Yer += " " + item.Yer;

                }
                Db.StokHareket.Add(hareket);
                Db.SaveChanges();
                response.Success = true;
                response.Description = "Depoya ekleme başarı ile gerçekleşti";
            }
            
            foreach(var item in liste)
            {
                //üretim emrini depoda olarak işaretle
                var uretimEmir = Db.UretimEmir.Find(item.UretimEmirId);
                if (uretimEmir != null)
                {
                    uretimEmir.DepodaMi = true;
                    Db.SaveChanges();
                }
            }
            
            return Json(response);

        }
        //public JsonResult DepoyaAktarCoklu(List<SiparisDepoAktarimDto> liste)
        //{
        //    var response = new Response();

        //    var sonStokHareket = Db.StokHareket.OrderByDescending(x => x.MontajKod).FirstOrDefault(x => x.MontajKod != null);
        //    int montajKod = 0;

        //    if (sonStokHareket != null)
        //    {
        //        montajKod = (int)sonStokHareket.MontajKod + 1;
        //    }
        //    var hareketList = new List<StokHareket>();
        //    var kalipList = new List<SiparisKalip>();
        //    foreach (var item in liste)
        //    {
        //        StokHareket hareket = new StokHareket();
        //        hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
        //        hareket.SiparisId = item.SiparisId;
        //        hareket.SiparisKalipId = item.SiparisKalipId;
        //        hareket.Adet = item.Adet;
        //        hareket.MontajliMi = true;
        //        hareket.MontajKod = montajKod;
        //        hareket.Yer = item.Yer;

        //        //kalıbı bul depoda diye işaretle
        //        var siparisKalip = Db.SiparisKalip.Find(hareket.SiparisKalipId);
        //        if (siparisKalip != null)
        //        {
        //            hareketList.Add(hareket);
        //            kalipList.Add(siparisKalip);
        //        }

        //        //üretim emrini depoda olarak işaretle
        //        var uretimEmir = Db.UretimEmir.Find(item.UretimEmirId);
        //        if (uretimEmir != null)
        //        {
        //            uretimEmir.DepodaMi = true;
        //            Db.SaveChanges();
        //        }


        //    }
        //    repo.AddRange(hareketList);
        //    foreach (var item in kalipList)
        //    {
        //        item.DepodaMi = true;
        //    }
        //    Db.SaveChanges();
        //    response.Success = true;
        //    response.Description = "Depoya ekleme başarı ile gerçekleşti";

        //    return Json(response);

        //}
    }
}