using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("BoyaKodSiparisKalipRel")]
    public class BoyaKodSiparisKalipRel
    {
        [Key]
        public int BoyaKodSiparisKalipRelId { get; set; }
        public int BoyaKodId { get; set; }
        public BoyaKod BoyaKod { get; set; }
        public int SiparisKalipId { get; set; }
        public SiparisKalip SiparisKalip { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }
}