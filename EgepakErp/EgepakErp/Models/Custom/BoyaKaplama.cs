using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("BoyaKaplama")]
    public partial class BoyaKaplama
    {
        [Key]
        public int BoyaKaplamaId { get; set; }
        public string Aciklama { get; set; }
        public string Kod { get; set; }
        public int BoyaKaplamaTypeId { get; set; }
        public BoyaKaplamaType BoyaKaplamaType { get; set; }


        [InverseProperty("MetalizeKod")]
        public ICollection<SiparisKalip> MetalizeKods { get; set; }
       
        [InverseProperty("GranulKod")]
        public ICollection<SiparisKalip> GranulKods { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }

    [Table("BoyaKaplamaType")]
    public class BoyaKaplamaType
    {
        [Key]
        public int BoyaKaplamaTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<BoyaKaplama> BoyaKaplama { get; set; }
    }
}