﻿using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data.Entity;

namespace EgePakErp.Controllers
{

    public class KalipController : BaseController
    {
        public KalipRepository repo { get; set; }
        public UrunRepository urunRepo { get; set; }
        public KalipController()
        {
            repo = new KalipRepository();
            urunRepo = new UrunRepository();
        }
        // GET: Cari
        [Menu("Kalıplar", "flaticon2-soft-icons icon-xl", "Üretim", 0, 4)]
        public ActionResult Index()
        {
            return View();
        }

        [Yetki("Kalıp Listesi", "Üretim")]
        public JsonResult Liste()
        {
            //kabasını aldır
            var dtModel = new DataTableModel<dynamic>();
            var dtMeta = new DataTableMeta();

            dtMeta.field = Request.Form["sort[field]"] == null ? "KalipId" : Request.Form["sort[field]"];
            dtMeta.sort = Request.Form["sort[sort]"] == null ? "Desc" : Request.Form["sort[sort]"];

            dtMeta.page = Convert.ToInt32(Request.Form["pagination[page]"]);
            dtMeta.perpage = Convert.ToInt32(Request.Form["pagination[perpage]"]);

            var model = repo.GetAll();

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[searchQuery]"]))
            {
                var searchQuery = Request.Form["query[searchQuery]"].ToString();
                model = model.Where(i =>
                i.ParcaKodu.ToLower() == searchQuery.ToLower() ||
                i.Adi.ToLower().Contains(searchQuery.ToLower())
                );
            }
            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[agirlik]"]))
            {
                var agirlik = Convert.ToDecimal(Request.Form["query[agirlik]"].ToString());
                model = model.Where(i => i.ParcaAgirlik == agirlik);
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[gozSayisi]"]))
            {
                var gozSayisi = Convert.ToDecimal(Request.Form["query[gozSayisi]"].ToString());
                model = model.Where(i => i.KalipGozSayisi == gozSayisi);
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[urunCinsiId]"]))
            {
                var searchQuery = Request.Form["query[urunId]"].ToString();
                var id = Convert.ToInt32(searchQuery);
                model = model.Where(i => i.KalipUrunRelation.FirstOrDefault().Urun.UrunCinsi.UrunCinsiId == id
                );
            }
            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[cevrimSuresi]"]))
            {
                var cevrimSuresi = Convert.ToInt32(Request.Form["query[cevrimSuresi]"].ToString());
                model = model.Where(i => i.UretimZamani == cevrimSuresi);
            }

            //Filtre
            if (!string.IsNullOrEmpty(Request.Form["query[urunId]"]))
            {
                var urunId = Convert.ToInt32(Request.Form["query[urunId]"].ToString());
                if(urunId > 0)
                {
                    model = model.Where(i => i.KalipUrunRelation.Where(x => x.UrunId == urunId).ToList().Count() > 0);
                }
                
            }

            try
            {
                model = model.OrderBy(dtMeta.field + " " + dtMeta.sort);
            }

            catch (Exception)
            {
                model = model.OrderBy("KalipId Desc");
                dtMeta.field = "KalipId";
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
                KalipId = i.KalipId,
                KalipKodu = i.ParcaKodu,
                Adi = i.Adi,
                Birim = i.KalipHammaddeRelation?.Select(x => x.HammaddeCinsi).FirstOrDefault()?.TableHammaddeBirim?.Birimi,
                UretimTeminSekli = i.UretimTeminSekli?.Adi,
                Agirlik = i.ParcaAgirlik,
                GozSayisi = i.KalipGozSayisi,
                UretimZamani = i.UretimZamani,
                Hammadde = i.KalipHammaddeRelation.Select(s => s.HammaddeCinsi).Select(s => s.Adi),
                Urun = i.KalipUrunRelation?.Select(s => s.Urun).Select(s => s.UrunCinsi.Kisaltmasi + s.UrunNo)

            }).ToList<dynamic>();


            dtModel.meta = dtMeta;
            dtModel.data = dto;
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

        [Yetki("Kalıp Kaydet", "Üretim")]
        public JsonResult Kaydet(Kalip form)
        {
            var response = new Response();

            try
            {
                if (form.KalipId == 0)
                {

                    form.KalipHammaddeRelation = form.HammaddeList.Select(s => new KalipHammaddeRelation { HammaddeCinsiId = s }).ToList();
                    form.KalipUrunRelation = form.UrunList.Select(s => new KalipUrunRelation { UrunId = s }).ToList();
                    form.isAktive = true;
                    repo.Insert(form);
                }
                else
                {
                    var entity = repo.Get(form.KalipId);
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
                        //relationları güncelle
                        Db.KalipHammaddeRelation.RemoveRange(Db.KalipHammaddeRelation.Where(i => i.KalipId == form.KalipId));
                        Db.KalipUrunRelation.RemoveRange(Db.KalipUrunRelation.Where(i => i.KalipId == form.KalipId));
                        if (form.HammaddeList != null)
                        {
                            Db.KalipHammaddeRelation.AddRange(form.HammaddeList.Select(s => new KalipHammaddeRelation { KalipId = entity.KalipId, HammaddeCinsiId = s }));
                        }
                        if (form.UrunList != null)
                        {
                            Db.KalipUrunRelation.AddRange(form.UrunList.Select(s => new KalipUrunRelation { KalipId = entity.KalipId, UrunId = s }));
                        }

                    }


                }
                Db.SaveChanges(CurrentUser.PersonelId);
                response.Success = true;
                response.Description = "İşlem Başarılı";
            }
            catch (Exception ex)
            {
                response.Success = true;
                response.Description = "Hata Oluştu Hata Mesajı: " + ex.Message.ToString();
            }

            return Json(response);

        }

        public JsonResult ListeSelect2(string q)
        {
            //kabasını aldır
            var model = repo.GetAll();
            var count = model.Count();
            //Filtre
            if (!string.IsNullOrEmpty(q))
            {
                var searchQuery = q;
                model = model.Where(
                    x => x.KalipNo.Contains(q) ||
                    x.Adi.Contains(q) ||
                    x.Aciklama.Contains(q)
                    );
            }
            model = model.OrderBy("KalipId Asc");

            //sayfala
            model = model.Take(10);

            //dto yap burda
            var dto = new
            {
                incomplete_results = true,
                items = model.Select(i => new
                {
                    id = i.KalipId,
                    text = string.Concat(i.KalipNo + " " + i.Adi),
                    markup = "urun"

                }),
                total_count = count
            };
            return Json(dto, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Sil(int KalipId)
        {
            Response response = new Response();
            var kalip = repo.Get(KalipId);
            try
            {
                kalip.isAktive = false;
                repo.Update(kalip);
                response.Success = true;
                response.Description = "kalıp silindi";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Description = ex.Message;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult KalipDetay(int KalipId)
        {
            var Kalip = repo.Get(KalipId);
            if (Kalip != null)
            {
                return PartialView(Kalip);
            }
            return PartialView();
        }
        public void KalipAgirlikDuzelt()
        {
            var list = Db.Kalip.ToList();
            foreach (var kalip in list)
            {
                kalip.ParcaAgirlik *= 10;
            }
            Db.SaveChanges();
        }

    }
}