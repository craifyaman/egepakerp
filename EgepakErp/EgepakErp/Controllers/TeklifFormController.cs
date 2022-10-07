using EgePakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.DtModels;
using EgePakErp.Enums;
using EgePakErp.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Hosting;
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

        [Menu("Teklif Formları", "fa-solid fa-industry icon-xl", "Sipariş", 0, 6)]
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
            var dto = model.AsEnumerable().Select(x => new
            {
                Id = x.SiparisTeklifFormId,
                Cari = x.Cari.Unvan,
                KayitTarih = x.KayitTarih.ToString("dd/MM/yyyy"),
                GonderenEposta = x.GonderenEposta,
                Gonderen = x.Gonderen,
                GonderenTel = x.GonderenTel,
                Alan = x.Alan,
                AlanEposta = x.AlanEposta,
                AlanBilgi = x.AlanBilgi,
                PersonelAd = x.PersonelAd,
                PersonelUnvan = x.PersonelUnvan,
                Aciklama = x.Aciklama,
                TeslimTarihi = x.TeslimTarihi.ToString("dd/MM/yyyy"),
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

                    if (form.SiparisTeklifFormUrun.Count() > 0)
                    {
                        foreach(var item in form.SiparisTeklifFormUrun)
                        {
                            item.SiparisTeklifFormId = form.SiparisTeklifFormId;
                        }
                    }

                    repo.Update(entity);
                    response.Success = true;
                    response.Description = "Güncellendi.";
                }

            }

            return Json(response);

        }

        public FileResult Pdf(int formId, string lang)
        {
            var teklifForm = repo.Get(formId);
            var bId = teklifForm.Cari.Unvan + "_Teklif_Formu_" + formId + ".pdf";
            var vPath = "~/Content/TeklifForm/" + bId;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
            string url = baseUrl + "/Pdf/TeklifFormu?formId=" + formId+"&lang="+lang;
            //string url = "https://localhost:44381/Pdf/TeklifFormu?formId=" + formId+"&lang="+lang;

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            doc.Save(HostingEnvironment.MapPath(vPath));

            // close pdf document
            doc.Close();

            return File(vPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(bId));

        }
    }
}