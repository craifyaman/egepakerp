using EgePakErp.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace EgePakErp.Controllers
{
    public class FileController : Controller
    {
        public JsonResult DosyaKaydet()
        {
            var response = new Response<string>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        var TargetDirectory = Request.Headers.Get("TargetDirectory");
                        TargetDirectory= HttpUtility.UrlDecode(TargetDirectory);
                        HttpFileCollectionBase files = Request.Files;
                        HttpPostedFileBase dosya = files[0];
                        string fname = dosya.FileName;
                        string directory = TargetDirectory;
                        string strFileName;
                        string strFilePath;
                        string strFolder;
                        strFolder = Server.MapPath("./" + directory + "/" + DateTime.Now.ToString("dd_MM_yyyy"));
                        // Retrieve the name of the file that is posted.
                        strFileName = dosya.FileName;
                        strFileName = Path.GetFileName(strFileName);

                        // Create the folder if it does not exist.
                        if (!Directory.Exists(strFolder))
                        {
                            Directory.CreateDirectory(strFolder);
                        }

                        // Save the uploaded file to the server.
                        strFilePath = strFolder + "/" + strFileName;
                        if (System.IO.File.Exists(strFilePath))
                        {
                            response.Success = true;
                            response.Description = strFileName + " dosya zaten mevcut";
                            response.Data = "/File/" + directory + "/" + DateTime.Now.ToString("dd_MM_yyyy") + "/" + strFileName;
                        }
                        else
                        {
                            dosya.SaveAs(strFilePath);
                            response.Success = true;
                            response.Description = strFileName + " dosya başarı ile yüklendi.";

                            response.Data = "/File/" + directory + "/" + DateTime.Now.ToString("dd_MM_yyyy") + "/" + strFileName;
                        }
                        return Json(response);  

                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Description = ex.Message;
                        return Json(response);
                    }

                }

            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Description = "bir hata oluştu";
            }

            return Json(response);
        }
    }
}