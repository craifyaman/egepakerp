using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Cari")]
    public partial class Cari
    {
        [Key]
        public int CariId { get; set; }
        public string Kod { get; set; }
        public int? CariGrupId { get; set; }
        public CariGrup CariGrup { get; set; }
        public string Unvan { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNumarasi { get; set; }
        public string WebSitesi { get; set; }
        public string Eposta { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public int? UlkeId { get; set; }
        public Ulke Ulke { get; set; }
        public int? IlId { get; set; }
        public Il Il { get; set; }
        public string PostaKodu { get; set; }
        public int? IlceId { get; set; }
        public Ilce Ilce { get; set; }
        public bool Aktif { get; set; }
        public bool Efatura { get; set; }
        public int? MusteriNo { get; set; }

        public virtual ICollection<Kisi> Kisi { get; set; }
        public virtual ICollection<Yaldiz> Yaldiz { get; set; }
        public virtual ICollection<Not> Not { get; set; }
        public virtual ICollection<Sepet> Sepet { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }


    }
}