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
        public string dtMetaField { get; set; }

        public StokHareketController()
        {
            repo = new StokHareketRepository();
            dtMetaField = "StokHareketId";
        }

        [Menu("Depo", "fas fa-warehouse icon-xl", "Depo", 0, 1)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? dtMetaField : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();
            var montajliKaliplar = repo.GetAll(x => x.MontajliMi).ToList();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[MontajliMi]"]))
            {
                bool MontajDurum = Request.Form["query[MontajliMi]"].ToLower() == "true" ? true : false;

                if (MontajDurum)
                {
                    model = model.Where(x => x.MontajliMi)
                    .GroupBy(x => x.MontajKod)
                    .AsEnumerable()
                    .Select(a => a.FirstOrDefault())
                    .AsQueryable();
                }
                else
                {
                    model = model.Where(x => x.MontajliMi == false);
                }
                    
            }
            else
            {
                model = model.Where(x => x.MontajliMi == false);
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



            var dto = model.AsEnumerable().Select(x => new
            {
                Id = x.StokHareketId,
                Type = x.StokHareketType.Type,
                KalipKodList = x.MontajliMi == true ? montajliKaliplar.Select(a => a.SiparisKalip.KalipKod) : new List<string>() { x.SiparisKalip.KalipKod },
                SiparisId = x.SiparisId,
                SiparisAdi = x.Siparis.SiparisAdi,
                Adet = x.Adet,
                MontajliMi = x.MontajliMi,

            }).ToList();


            dtModel.meta = dtMeta;
            dtModel.data = dto.ToList<dynamic>();
            return Json(dtModel);

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