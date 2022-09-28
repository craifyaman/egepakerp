using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class TeklifFormController : BaseController
    {
        public TeklifFormRepository repo { get; set; }
        public string dtMetaField { get; set; }

        public TeklifFormController()
        {
            repo = new TeklifFormRepository();
            dtMetaField = "SiparisTeklifFormId";
        }

        [Menu("Teklif Formları", "fa-solid fa-industry icon-xl", "Teklif Form", 0, 5)]
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
            var dto = model.Select(x => new
            {
                Id = x.SiparisTeklifFormId,
                Cari = x.Cari.Unvan,
                KayitTarih = x.KayitTarih,
                Eposta = x.Eposta,
                GonderilenAdSoyad = x.GonderilenAdSoyad,
                Aciklama = x.Aciklama,
                MinSiparisAdet = x.MinSiparisAdet,
                Odeme = x.Odeme,
                TeslimTarihi = x.TeslimTarihi,
                Personel = x.Personel.Adi,
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

        public JsonResult Kaydet(SiparisTeklifForm form)
        {
            var response = new Response();

            if (form.SiparisTeklifFormId == 0)
            {
                form.KayitTarih = DateTime.Now;
                form.PersonelId = CurrentUser.PersonelId;
                repo.Insert(form);

                response.Success = true;
                response.Description = "Kaydedildi.";
            }
            else
            {
                var entity = repo.Get(form.SiparisTeklifFormId);
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
    }
}