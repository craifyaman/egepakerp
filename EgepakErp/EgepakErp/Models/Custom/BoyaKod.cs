using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("BoyaKod")]
    public partial class BoyaKod
    {
        [Key]
        public int BoyaKodId { get; set; }
        public string Aciklama { get; set; }
        public string Kod { get; set; }
        public int BoyaKodTypeId { get; set; }
        public BoyaKodType BoyaKodType { get; set; }

        [InverseProperty("TozBoyaKod")]
        public ICollection<SiparisKalip> TozBoyaKods { get; set; }
        [InverseProperty("SpreyBoyaKod")]
        public ICollection<SiparisKalip> SpreyBoyaKods { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }

}