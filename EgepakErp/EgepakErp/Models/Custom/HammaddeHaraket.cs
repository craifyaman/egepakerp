using System;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{
    public class HammaddeHaraket
    {
        public int HammaddeHaraketId { get; set; }

        public DateTime KayitTarihi { get; set; }
        public DateTime HammaddeGirisTarihi { get; set; }
        public int HammaddeCinsiId { get; set; }
        public decimal Miktar { get; set; }
        public Cari TedarikciId { get; set; }
        public decimal KdvTutarı { get; set; }

        public decimal ToplamTutar { get; set; }
        public int KdvOranı { get; set; }

        public string FaturaNo { get; set; }

        public int MarkaId { get; set; }

        public int DovizId { get; set; }

        public decimal DolarKuru { get; set; }

        public decimal EuroKuru { get; set; }

        public decimal PoundKuru { get; set; }

        public int HammaddetipiId { get; set; }
        public virtual HammaddeTipi HammaddeTipi { get; set; }


        //Virtual
        public virtual Marka Marka { get; set; }

        public virtual HammaddeCinsi HammaddeCinsi { get; set; }

        public virtual Doviz Doviz { get; set; }
    }
}



