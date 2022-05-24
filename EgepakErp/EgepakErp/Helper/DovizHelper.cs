using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace EgepakErp.Helper
{
    public class DovizHelper
    {
        public static decimal DovizKuruGetir(string Kod, DateTime date)
        {
            decimal deger;          
            DateTime tarih = Convert.ToDateTime(date.ToString("dd/MM/yyyy"));

            if (tarih.DayOfWeek == DayOfWeek.Saturday)
            {
                //cumartesi ise cumanın kur değeri alınır
                tarih = tarih.AddDays(-1);
            }
            else if (tarih.DayOfWeek == DayOfWeek.Sunday)
            {
                //pazar ise cumanın kur değeri alınır
                tarih = tarih.AddDays(-2);
            }

            if (tarih.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") && tarih.DayOfWeek == DayOfWeek.Monday)
            {
                //bugün pazartesi ise geçen cumanın kuru alınır.
                tarih = Convert.ToDateTime(date.AddDays(-3));

            }
            else
            {
                //önceki günün kur değeri alınır.
                tarih = Convert.ToDateTime(date.AddDays(-1));
            }
            
            

            var t = tarih.ToString("yyyyMM");
            var t1 = tarih.ToString("ddMMyyyy");
            string url = "https://www.tcmb.gov.tr/kurlar/" + t + "/" + t1 + ".xml";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/xml"; // <== THIS FIXED IT

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument doc = XDocument.Load(stream);
                    deger = Convert.ToDecimal(doc.Root.Descendants("Currency").FirstOrDefault(i => i.Attribute("Kod").Value == Kod).Descendants("ForexSelling").FirstOrDefault().Value.Replace(".", ",").ToString());
                }
            }
            return deger;
        }
    }
}