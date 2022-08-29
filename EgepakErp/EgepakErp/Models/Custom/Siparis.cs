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
        public string SiparisAdi { get; set; }
        public DateTime TerminTarihi { get; set; }
        public DateTime KayitTarihi { get; set; }

        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public int UrunId { get; set; }
        public Urun Urun { get; set; }

        [Column(TypeName = "money")]
        public decimal ToplamMaliyet { get; set; }

        [Column(TypeName = "money")]
        public decimal ToplamMaliyetUsd { get; set; }

        [Column(TypeName = "money")]
        public decimal ToplamMaliyetEur { get; set; }
        [AllowHtml]
        public string Aciklama { get; set; }
        public string AciklamaPdf { get; set; }


        public ICollection<SiparisKalip> SiparisKalip { get; set; }

        //public ICollection<SiparisKatman> SiparisKatman { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }


    }
}