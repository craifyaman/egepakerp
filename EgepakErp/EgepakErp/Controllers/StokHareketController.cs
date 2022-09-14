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
        public KalipRepository kalipRepo { get; set; }
        public string dtMetaField { get; set; }

        public StokHareketController()
        {
            repo = new StokHareketRepository();
            siparisKalipRepo = new SiparisKalipRepository();
            yaldizRepo = new YaldizRepository();
            boyaKodRepo = new BoyaKodRepository();
            kalipRepo = new KalipRepository();


            dtMetaField = "StokHareketId";
        }

        [Menu("Depo", "fas fa-warehouse icon-xl", "Depo", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }

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
            var allList = repo.GetAll().ToList();
            var yaldizList = yaldizRepo.GetAll();
            var BoyaKodList = boyaKodRepo.GetAll();
            var montajliKaliplar = repo.GetAll(x => x.MontajliMi).ToList();
            var yaldizKalipList = siparisKalipRepo.GetAll(x => x.YaldizId != null);
            var tozBoyaKodKalipList = siparisKalipRepo.GetAll(x => x.TozBoyaKodId != null);
            var kalipListe = kalipRepo.GetAll();

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

            if (montajli)
            {
                var dto = model.AsEnumerable().Where(x => x.MontajliMi == true).GroupBy(x => x.MontajKod).Select(x =>
                {
                    var temp = x.FirstOrDefault();
                    var montajKod = temp.MontajKod;
                    var montajliList = new List<StokHareket>();
                    var BoyaKodId = 0;
                    var BoyaKodlar = "";
                    var KalipAdlar = "";

                    if (montajKod != null)
                    {
                        montajliList = allList.Where(a => a.MontajKod == montajKod).ToList();
                    }

                    var KalipKodList = temp.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == temp.MontajKod).Select(a => a.SiparisKalip.KalipKod) : new List<string>() { temp.SiparisKalip.KalipKod };

                    var YaldizId = 0;
                    var YaldizAd = "";

                    foreach (var item in KalipKodList)
                    {
                        var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == item);
                        if (_kalip != null)
                        {
                            KalipAdlar += "_" + _kalip.Adi;
                        }

                        var yaldizSiparisKalip = yaldizKalipList.FirstOrDefault(c => c.SiparisId == temp.SiparisId && c.KalipKod == item && c.MaliyetType.ToLower() == "yaldiz");
                        var tozBoyaKodSiparisKalip = tozBoyaKodKalipList.Where(c => c.SiparisId == temp.SiparisId && c.KalipKod == item && c.MaliyetType.ToLower() == "tozboya").ToList();

                        if (yaldizSiparisKalip != null)
                        {
                            YaldizId = (int)yaldizSiparisKalip.YaldizId;
                            YaldizAd = yaldizList.FirstOrDefault(a => a.YaldizId == YaldizId).Aciklama + " (" + _kalip.Adi + ")";
                        }

                        if (tozBoyaKodSiparisKalip != null && tozBoyaKodSiparisKalip.Count > 0)
                        {
                            foreach (var b in tozBoyaKodSiparisKalip)
                            {
                                BoyaKodId = (int)b.TozBoyaKodId;
                                BoyaKodlar += _kalip.Adi + "  : " + BoyaKodList.FirstOrDefault(a => a.BoyaKodId == BoyaKodId).Aciklama + "<br/>";
                            }
                        }


                    }

                    dynamic ret = new
                    {
                        Id = temp.StokHareketId,
                        Type = temp.StokHareketType.Type,
                        SiparisId = temp.SiparisId,
                        KalipKodList = KalipAdlar.Remove(0, 1),
                        Yaldiz = YaldizAd,
                        BoyaKod = BoyaKodlar,
                        SiparisAdi = temp.Siparis.SiparisAdi,
                        Adet = temp.Adet,
                        MontajliMi = temp.MontajliMi,
                    };

                    return ret;

                }).ToList();


                dtModel.meta = dtMeta;
                dtModel.data = dto.ToList<dynamic>();
                return Json(dtModel);
            }

            else
            {
                var dto = model.AsEnumerable().Where(x => x.MontajliMi == false).Select(x =>
                {
                    var montajKod = x.MontajKod;
                    var montajliList = new List<StokHareket>();
                    var BoyaKodId = 0;
                    var BoyaKodlar = "";
                    var KalipAdlar = "";

                    if (montajKod != null)
                    {
                        montajliList = allList.Where(a => a.MontajKod == montajKod).ToList();
                    }

                    var KalipKodList = x.MontajliMi == true ? montajliKaliplar.Where(a => a.MontajKod == x.MontajKod).Select(a => a.SiparisKalip.KalipKod) : new List<string>() { x.SiparisKalip.KalipKod };

                    var YaldizId = 0;
                    var YaldizAd = "";

                    foreach (var item in KalipKodList)
                    {
                        var _kalip = kalipListe.FirstOrDefault(a => a.ParcaKodu == item);
                        if (_kalip != null)
                        {
                            KalipAdlar += "_" + _kalip.Adi;
                        }

                        var yaldizSiparisKalip = yaldizKalipList.FirstOrDefault(c => c.SiparisId == x.SiparisId && c.KalipKod == item && c.MaliyetType.ToLower() == "yaldiz");
                        var tozBoyaKodSiparisKalip = tozBoyaKodKalipList.Where(c => c.SiparisId == x.SiparisId && c.KalipKod == item && c.MaliyetType.ToLower() == "tozboya").ToList();

                        if (yaldizSiparisKalip != null)
                        {
                            YaldizId = (int)yaldizSiparisKalip.YaldizId;
                            YaldizAd = yaldizList.FirstOrDefault(a => a.YaldizId == YaldizId).Aciklama + " (" + _kalip.Adi + ")";
                        }

                        if (tozBoyaKodSiparisKalip != null && tozBoyaKodSiparisKalip.Count > 0)
                        {
                            foreach (var b in tozBoyaKodSiparisKalip)
                            {
                                BoyaKodId = (int)b.TozBoyaKodId;
                                BoyaKodlar += _kalip.Adi + "  : " + BoyaKodList.FirstOrDefault(a => a.BoyaKodId == BoyaKodId).Aciklama + "<br/>";
                            }
                        }


                    }

                    dynamic ret = new
                    {
                        Id = x.StokHareketId,
                        Type = x.StokHareketType.Type,
                        SiparisId = x.SiparisId,
                        KalipKodList = KalipAdlar.Remove(0, 1),
                        Yaldiz = YaldizAd,
                        BoyaKod = BoyaKodlar,
                        SiparisAdi = x.Siparis.SiparisAdi,
                        Adet = x.Adet,
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

            try
            {
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

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }

            return Json(response);

        }



        public JsonResult DepoyaAktarTekli(int SiparisKalipId, int SiparisId, int Adet)
        {
            var response = new Response();

            try
            {
                StokHareket hareket = new StokHareket();
                hareket.StokHareketTypeId = (int)EStokHareketType.Giris;
                hareket.SiparisId = SiparisId;
                hareket.SiparisKalipId = SiparisKalipId;
                hareket.Adet = Adet;
                hareket.MontajliMi = false;

                //kalıbı bul depoda diye işaretle
                var siparisKalip = Db.SiparisKalip.Find(hareket.SiparisKalipId);
                if (siparisKalip != null)
                {
                    if (siparisKalip.DepodaMi == true)
                    {
                        hareket.Adet = Adet;
                        repo.Update(hareket);
                        response.Success = true;
                        response.Description = "Ürünün Depodaki miktarı güncellendi !!";
                        return Json(response);
                    }
                    repo.Insert(hareket);
                    siparisKalip.DepodaMi = true;
                    Db.SaveChanges();

                    response.Success = true;
                    response.Description = "Depoya ekleme başarı ile gerçekleşti";
                    return Json(response);
                }

                response.Success = false;
                response.Description = "kalıp bulunamadı";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }

            return Json(response);

        }

        public JsonResult DepoyaAktarCoklu(List<SiparisDepoAktarimDto> liste)
        {
            var response = new Response();

            try
            {
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
                }
                repo.AddRange(hareketList);
                foreach (var item in kalipList)
                {
                    item.DepodaMi = true;
                }
                Db.SaveChanges();
                response.Success = true;
                response.Description = "Depoya ekleme başarı ile gerçekleşti";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }

            return Json(response);

        }
    }
}