using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Siparis")]
    public partial class Siparis
    {
        [Key]
        public int SiparisId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public int UrunId { get; set; }
        public Urun Urun { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public decimal ToplamMaliyetUsd { get; set; }
        public decimal ToplamMaliyetEur { get; set; }
        public ICollection<SiparisKalip> SiparisKalip { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }


    }
}