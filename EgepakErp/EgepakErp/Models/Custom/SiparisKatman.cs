using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("SiparisKatman")]
    public partial class SiparisKatman
    {
        [Key]
        public int SiparisKatmanId { get; set; }
        public string UrunKod { get; set; }
        public string UrunAd { get; set; }

        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }


    }
}