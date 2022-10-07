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
        public string Gonderen { get; set; }
        public string GonderenEposta { get; set; }
        public string GonderenTel { get; set; }
       
        public string Alan { get; set; }
        public string AlanEposta { get; set; }
        public string AlanBilgi { get; set; }
        public string PersonelAd { get; set; }
        public string PersonelUnvan { get; set; }
        [AllowHtml]
        public string Aciklama { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public int DovizId { get; set; }
        public Doviz Doviz { get; set; }

        public ICollection<SiparisTeklifFormUrun> SiparisTeklifFormUrun { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}