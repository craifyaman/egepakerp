using EgePakErp.Models.Audit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Kisi")]
    public class Kisi
    {
        [Key]
        public int KisiId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public int? PersonelId { get; set; }
        public Personel Personel { get; set; }
        public string AdSoyad { get; set; }
        public string Görev { get; set; }
        public string Eposta { get; set; }
        public string Eposta2 { get; set; }
        public string Telefon { get; set; }
        public string Telefon2 { get; set; }
        public bool Birincil { get; set; }
        public bool Aktif { get; set; }
        public DateTime KayitTarihi { get; set; }
        public ICollection<UretimEmirAksiyon> UretimEmirAksiyon { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }
    }
}
