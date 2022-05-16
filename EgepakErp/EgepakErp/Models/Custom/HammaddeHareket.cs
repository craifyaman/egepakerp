using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EgePakErp.Models;

namespace EgePakErp.Models
{
    [Table("HammaddeHareket")]
    public class HammaddeHareket
    {
        [Key]
        public int HammaddeHareketId { get; set; }
        public string FaturaNo { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime HammaddeGirisTarihi { get; set; }
        public int? TedarikciId { get; set; }
        public Cari Tedarikci { get; set; }
        public int? HammaddeCinsiId { get; set; }
        public HammaddeCinsi HammaddeCinsi { get; set; }
        public string UrunAdi { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamTutar { get; set; }
        public int DovizId { get; set; }
        public virtual Doviz Doviz { get; set; }
        public decimal Miktar { get; set; }

        public decimal? DolarKuru { get; set; }
        public decimal? EuroKuru { get; set; }
        public decimal? PoundKuru { get; set; }

        public int HammaddeBirimiId { get; set; }
        public HammaddeBirimi HammaddeBirimi { get; set; }


        public int? HammaddetipiId { get; set; }
        public virtual HammaddeTipi HammaddeTipi { get; set; }

        public int? MarkaId { get; set; }
        public virtual Marka Marka { get; set; }

        public decimal? KdvTutarı { get; set; }

        public int? KdvOranı { get; set; }

    }
}



