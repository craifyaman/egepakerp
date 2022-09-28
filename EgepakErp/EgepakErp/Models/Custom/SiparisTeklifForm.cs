using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace EgePakErp.Models
{
    [Table("SiparisTeklifForm")]
    public partial class SiparisTeklifForm
    {
        [Key]
        public int SiparisTeklifFormId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public DateTime KayitTarih { get; set; }
        public string Eposta { get; set; }
        public string GonderilenAdSoyad { get; set; }
        public string Aciklama { get; set; }
        public int MinSiparisAdet { get; set; }
        public string Odeme{ get; set; }
        public DateTime TeslimTarihi { get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public ICollection<SiparisTeklifFormUrun> SiparisTeklifFormUrun { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}