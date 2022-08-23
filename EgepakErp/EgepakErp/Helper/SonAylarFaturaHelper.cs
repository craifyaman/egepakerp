using EgePakErp;
using System;
using System.Linq;

namespace EgePakErp.Helper
{
    public class SonAylarFaturaHelper
    {
        private Db db;
        public DateTime fromDate;
        public DateTime toDate;

        public SonAylarFaturaHelper(DateTime _fromDate, DateTime _toDate)
        {
            db = new Db();
            fromDate = _fromDate;
            toDate = _toDate;            
        }
        public double Calculate()
        {
            var hareketler = db.HammaddeHareket.Where(x => x.KayitTarihi.Date >= fromDate.Date && x.KayitTarihi.Date <= toDate).ToList();
            return (double)hareketler.Average(x => x.BirimFiyat);            
        }

    }
}