 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{

    [Table("Doviz")]
    public partial class Doviz
    {
        [Key]
        public int DovizId { get; set; }
        public string Adi { get; set; }

        public string Kisaltma { get; set; }
        public string Sembol { get; set; }

        [InverseProperty("Doviz1")]
        public virtual ICollection<Cari> Doviz1CariListesi { get; set; }

        [InverseProperty("Doviz2")]
        public virtual ICollection<Cari> Doviz2CariListesi { get; set; }

        [InverseProperty("Doviz3")]
        public virtual ICollection<Cari> Doviz3CariListesi { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

        public virtual ICollection<HammaddeHareket> HammaddeHareket { get; set; }
        public virtual ICollection<YanSanayiStok> YanSanayiStokListesi { get; set; }
        public virtual ICollection<HazirMalzemeFiyat> HazirMalzemeFiyatListesi { get; set; }
        public virtual ICollection<Fiyat> FiyatListesi { get; set; }
        public virtual ICollection<SiparisTeklifForm> SiparisTeklifForm { get; set; }
    }

}
