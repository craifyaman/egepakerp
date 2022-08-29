using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("SiparisKalip")]
    public partial class SiparisKalip
    {
        [Key]
        public int SiparisKalipId { get; set; }
        public float Maliyet { get; set; }
        public string KalipKod { get; set; }
        public string MaliyetType { get; set; }
        public bool isEnable { get; set; }
        public string YaldizPdf { get; set; }

        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }


    }
}