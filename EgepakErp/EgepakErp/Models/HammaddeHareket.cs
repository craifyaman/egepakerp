using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgepakErp.Models
{
    public class HammaddeHareket
    {
        public int HammaddeHareketId { get; set; }
        public int HammaddeCinsiId{ get; set; }
        public HammaddeCinsi HammaddeCinsi { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public int BirimFiyatDovizId { get; set; }
        //public Doviz BirimFiyatDoviz { get; set; }
        public int Kdv { get; set; }
        public decimal KdvTutari { get; set; }
        public decimal ToplamTutar { get; set; }

        public decimal DolarKur { get; set; }
        public decimal EuroKur { get; set; }
        public decimal PoundKur { get; set; }
        
        //
        public int TedarikciId{ get; set; }

        public string FaturaNo { get; set; }
        public string BelgeNo { get; set; }
        public string SiraNo { get; set; }

        public DateTime FaturaTarihi { get; set; }
        public DateTime KayitTarihi { get; set; }



    }
}