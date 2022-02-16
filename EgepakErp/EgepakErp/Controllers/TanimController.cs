using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EgePakErp.Custom;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{

    public class TanimController : BaseController
    {

        public JsonResult SelectIlListe(int ulkeId)
        {
            IQueryable<Il> model = null;
            model = Db.Il.AsQueryable();

            model = model.Where(i => i.UlkeId == ulkeId);
            model = model.OrderBy(i => i.Adi);

            return Json(model.Select(s => new { Id = s.IlId, Value = s.Adi }).ToArray(), JsonRequestBehavior.AllowGet);

        }
        public JsonResult SelectIlceListe(int ilId)
        {
            IQueryable<Ilce> model = null;
            model = Db.Ilce.AsQueryable();

            model = model.Where(i => i.IlId == ilId);
            model = model.OrderBy(i => i.Adi);

            return Json(model.Select(s => new { Id = s.IlId, Value = s.Adi }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CariGrupForm (int id) {

            return PartialView(Db.CariGrup.Find(id));
        }
        public JsonResult CariGrupKaydet(CariGrup cariGrup)
        {
            var response = new Response<CariGrup>();
            if (cariGrup.CariGrupId>0)
            {
                
            }
            else
            {
                Db.CariGrup.Add(cariGrup);
            }
            Db.SaveChanges();

            Session["CariGrup"] = Db.CariGrup.ToList();

            response.Description = "İşlem Başarılı";
            response.Success = true;
            response.Data = new CariGrup { CariGrupId = cariGrup.CariGrupId, Adi = cariGrup.Adi };

            return Json(response);
        }


       

        public PartialViewResult GorusmeTipForm(int id)
        {
            return PartialView(Db.GorusmeTip.Find(id));
        }
        public JsonResult GorusmeTipKaydet(GorusmeTip frm)
        {
            var response = new Response<GorusmeTip>();
            if (frm.GorusmeTipId > 0)
            {

            }
            else
            {
                Db.GorusmeTip.Add(frm);
            }
            Db.SaveChanges();

            Session["GorusmeTip"] = Db.CariGrup.ToList();

            response.Description = "İşlem Başarılı";
            response.Success = true;
            response.Data = new GorusmeTip { GorusmeTipId = frm.GorusmeTipId, Adi = frm.Adi };

            return Json(response);
        }






    }
}