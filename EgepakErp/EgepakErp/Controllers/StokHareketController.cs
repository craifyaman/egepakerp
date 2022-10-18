using EgePakErp.Concrete;
using EgePakErp.Controllers;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
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

        [Menu("Depo", "fas fa-warehouse icon-xl", "Depo", 0, 5)]
        public ActionResult Index()
        {
            return View();
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

        public JsonResult Liste()
        {
            bool montajli = false;

            if (!string.IsNullOrEmpty(Request.Form["query[MontajliMi]"]))
            {
                montajli = Request.Form["query[MontajliMi]"].ToLower() == "true" ? true : false;
            }

            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? dtMetaField : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            var montajliKaliplar = repo.GetAll(x => x.MontajliMi).ToList();
            var kalipListe = kalipRepo.GetAll();

            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                string q = Request.Form["query[searchQuery]"].ToString();
                model = model
                    .Where(
                    x => x.Siparis.SiparisKod.Contains(q) ||
                    x.SiparisKalip.KalipKod == q ||
                    x.SiparisKalip.KalipAdi.Contains(q)
                    );
            }

            if (!string.IsNullOrEmpty(Request.Form["query[cariId]"]))
            {
                int cariId = Convert.ToInt32(Request.Form["query[cariId]"].ToString());
                model = model.Where(x => x.Siparis.CariId == cariId);
            }

            if (!string.IsNullOrEmpty(Request.Form["query[yer]"]))
            {
                string yer = Request.Form["query[cariId]"].ToString();
                model = model.Where(x => x.Yer.Contains(yer));
            }
            if (!string.IsNullOrEmpty(Request.Form["query[adi]"]))
            {
                string adi = Request.Form["query[adi]"].ToString();
                model = model.Where(x => (x.SiparisKalip.EnjeksiyonRenk + " " + x.SiparisKalip.KalipAdi).Contains(adi));
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy(dtMetaField + " Desc");
                dtMeta.field = dtMetaField;
                dtMeta.sort = "Desc";
            }

            var count = model.Count();

            dtMeta.total = count;
            dtMeta.pages = dtMeta.total / dtMeta.perpage + 1;


            //sayfala
            model = model.Skip((dtMeta.page - 1) * dtMeta.perpage).Take(dtMeta.perpage);

            //kalıplar birbirine montajlı ise
            if (montajli)
            {
                var dto = model.AsEnumerable().Where(x => x.MontajliMi == true).GroupBy(x => x.MontajKod).Select(x =>
                {
                    var temp = x.FirstOrDefault();
                    var montajKod = temp.MontajKod;
                    var montajliList = new List<StokHareket>();
                    var KalipAdlar = "";
                    var KalipKodList = temp.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == temp.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { temp.SiparisKalip.SiparisKalipId };

                    foreach (var item in KalipKodList)
                    {
                        var siparisKalip = siparisKalipRepo.Get(item);
                        var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
                        if (_kalip != null)
                        {
                            KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
                        }


                    }

                    dynamic ret = new
                    {
                        Id = temp.StokHareketId,
                        Type = temp.StokHareketType.Type,
                        SiparisId = temp.SiparisId,
                        SiparisKalipId = temp.SiparisKalipId,
                        Yer = x.FirstOrDefault(y => y.Yer != null)?.Yer,
                        SiparisKod = temp.Siparis.SiparisKod,
                        CariId = temp.Siparis.CariId,
                        Cari = temp.Siparis.Cari.Unvan,
                        Adet = x.FirstOrDefault(y => y.Adet != null && y.Adet != 0)?.Adet,
                        Kalan = temp.DepodaKalanAdet,
                        MontajliMi = temp.MontajliMi,
                    };

                    return ret;

                }).ToList();


                dtModel.meta = dtMeta;
                dtModel.data = dto.ToList<dynamic>();
                return Json(dtModel);
            }

            //kalıplar tek tek girecek ise
            else
            {
                var dto = model.AsEnumerable().Where(x => x.MontajliMi == false).Select(x =>
                {
                    var montajKod = x.MontajKod;
                    var montajliList = new List<StokHareket>();
                    var KalipAdlar = "";

                    var KalipKodList = x.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == x.MontajKod).Select(a => a.SiparisKalip.SiparisKalipId) : new List<int>() { x.SiparisKalip.SiparisKalipId };


                    foreach (var item in KalipKodList)
                    {
                        var siparisKalip = siparisKalipRepo.Get(item);
                        var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == siparisKalip.KalipKod);
                        if (_kalip != null)
                        {
                            KalipAdlar += siparisKalip.EnjeksiyonRenk + " " + _kalip.Adi + "<br />";
                        }
                    }

                    dynamic ret = new
                    {
                        Id = x.StokHareketId,
                        Type = x.StokHareketType.Type,
                        SiparisId = x.SiparisId,
                        SiparisKalipId = x.SiparisKalipId,
                        KalipKodList = KalipAdlar,
                        Yer = x.Yer,
                        SiparisKod = x.Siparis.SiparisKod,
                        CariId = x.Siparis.CariId,
                        Cari = x.Siparis.Cari.Unvan,
                        Adet = x.Adet,
                        Kalan = x.DepodaKalanAdet,
                        MontajliMi = x.MontajliMi,
                    };

                    return ret;

                }).ToList();


                dtModel.meta = dtMeta;
                dtModel.data = dto.ToList<dynamic>();
                return Json(dtModel);
            }

        }


        public PartialViewResult Form(int? id)
        {
            id = id == null ? 0 : id.Value;
            var model = repo.Get((int)id);

            return PartialView(model);
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
                repo.Insert(form);
                response.Success = true;
                response.Description = "Kayıt edildi.";
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



        public JsonResult DepoyaAktarTekli(int SiparisKalipId, int SiparisId, int Adet, string Yer, int UretimEmirId)
        {
            var response = new Response();

            StokHareket hareket = new StokHareket();
            hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
            hareket.SiparisId = SiparisId;
            hareket.SiparisKalipId = SiparisKalipId;
            hareket.Adet = Adet;
            hareket.MontajliMi = false;
            hareket.Yer = Yer;


            //kalıbı bul depoda diye işaretle
            //var siparisKalip = Db.SiparisKalip.Find(hareket.SiparisKalipId);
            //if (siparisKalip != null)
            //{
            //    if (siparisKalip.DepodaMi == true)
            //    {
            //        hareket.Adet = Adet;
            //        repo.Update(hareket);
            //        response.Success = true;
            //        response.Description = "Ürünün Depodaki miktarı güncellendi !!";
            //        return Json(response);
            //    }
            //    repo.Insert(hareket);
            //    siparisKalip.DepodaMi = true;
            //    Db.SaveChanges();

            //    response.Success = true;
            //    response.Description = "Depoya ekleme başarı ile gerçekleşti";
            //    return Json(response);
            //}

            //response.Success = false;
            //response.Description = "kalıp bulunamadı";


            //üretim emrini depoda olarak işaretle
            var uretimEmir = Db.UretimEmir.Find(UretimEmirId);
            if (uretimEmir != null)
            {
                uretimEmir.DepodaMi = true;
                Db.SaveChanges();
            }


            repo.Insert(hareket);
            response.Success = true;
            response.Description = "Ürün depoya aktarıldı";

            return Json(response);

        }

        public JsonResult DepoyaAktarCoklu(List<SiparisDepoAktarimDto> liste)
        {
            var response = new Response();

            var sonStokHareket = Db.StokHareket.OrderByDescending(x => x.MontajKod).FirstOrDefault(x => x.MontajKod != null);
            int montajKod = 0;

            if (sonStokHareket != null)
            {
                montajKod = (int)sonStokHareket.MontajKod + 1;
            }
            var hareketList = new List<StokHareket>();
            var kalipList = new List<SiparisKalip>();
            foreach (var item in liste)
            {
                StokHareket hareket = new StokHareket();
                hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
                hareket.SiparisId = item.SiparisId;
                hareket.SiparisKalipId = item.SiparisKalipId;
                hareket.Adet = item.Adet;
                hareket.MontajliMi = true;
                hareket.MontajKod = montajKod;
                hareket.Yer = item.Yer;

                //kalıbı bul depoda diye işaretle
                var siparisKalip = Db.SiparisKalip.Find(hareket.SiparisKalipId);
                if (siparisKalip != null)
                {
                    hareketList.Add(hareket);
                    kalipList.Add(siparisKalip);
                    //repo.Insert(hareket);
                    //siparisKalip.DepodaMi = true;
                    //Db.SaveChanges();
                    //return Json(response);
                }

                //üretim emrini depoda olarak işaretle
                var uretimEmir = Db.UretimEmir.Find(item.UretimEmirId);
                if (uretimEmir != null)
                {
                    uretimEmir.DepodaMi = true;
                    Db.SaveChanges();
                }


            }
            repo.AddRange(hareketList);
            foreach (var item in kalipList)
            {
                item.DepodaMi = true;
            }
            Db.SaveChanges();
            response.Success = true;
            response.Description = "Depoya ekleme başarı ile gerçekleşti";

            return Json(response);

        }
    }
}