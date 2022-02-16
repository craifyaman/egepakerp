using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    public class Hatirlatici
    {
        [Key]
        public int HatirlaticiId { get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public string Aciklama { get; set; }
        public string KapanisAciklama { get; set; }
        public DateTime HatirlatmaTarihi{ get; set; }
        public DateTime KayitTarihi{ get; set; }
        public bool Durum { get; set; }
        public bool Sms { get; set; }
        public bool Eposta { get; set; }
        public bool Whatsapp { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}