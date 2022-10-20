using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace EgePakErp.Models
{
    [Table("Siparis")]
    public partial class Siparis
    {
        [Key]
        public int SiparisId { get; set; }
        public string SiparisKod { get; set; }
        public string SiparisIsim { get; set; }
        public DateTime TerminTarihi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int SiparisAdet { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public int UrunId { get; set; }
        public Urun Urun { get; set; }
        public int SiparisDurumId { get; set; }
        public SiparisDurum SiparisDurum { get; set; }

        [Column(TypeName = "money")]
        public decimal TeklifFiyat { get; set; }

        [Column(TypeName = "money")]
        public decimal TeklifFiyatUsd { get; set; }

        [Column(TypeName = "money")]
        public decimal TeklifFiyatEur { get; set; }

        public double NakitKatsayi { get; set; }
        public double VadeliKatsayi { get; set; }


        [AllowHtml]
        public string Aciklama { get; set; }

        public ICollection<SiparisKalip> SiparisKalip { get; set; }
        public ICollection<StokHareket> StokHareket { get; set; }
        public ICollection<UretimEmir> UretimEmir { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

      



    }
}