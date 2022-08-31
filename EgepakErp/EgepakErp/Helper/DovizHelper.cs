using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace EgePakErp.Helper
{
    public class DovizHelper
    {
        public static decimal DovizKuruGetir(string Kod, DateTime date)
        {
            decimal deger = 0;
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

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        XDocument doc = XDocument.Load(stream);
                        deger = Convert.ToDecimal(doc.Root.Descendants("Currency").FirstOrDefault(i => i.Attribute("Kod").Value == Kod).Descendants("ForexSelling").FirstOrDefault().Value.Replace(".", ",").ToString());
                    }
                }
            }
            catch (Exception ex)
            {


            }

            return deger;
        }
        public static decimal BugunDoviz(string doviz="usd")
        {
            try
            {
                XmlDocument xmlVerisi = new XmlDocument();
                xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (doviz.ToLower() == "usd")
                {
                    decimal dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                    return dolar;
                }
                if (doviz.ToLower() == "eur")
                {
                    decimal euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                    return euro;
                }
                else
                {
                    decimal sterlin = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));
                    return sterlin;
                }
            }
            catch (XmlException xml)
            {
                var ex = xml.ToString();
                return 0;
            }

        }
    }
}