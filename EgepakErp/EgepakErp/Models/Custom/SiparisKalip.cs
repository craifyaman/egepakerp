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

        [Column(TypeName = "money")]
        public decimal Maliyet { get; set; }
        public string KalipKod { get; set; }
        public string KalipAdi { get; set; }
        public string MaliyetType { get; set; }
        public bool isEnable { get; set; }
        //public int? YaldizId { get; set; }
        //public Yaldiz Yaldiz { get; set; }
        public string YaldizKodList { get; set; }

        [ForeignKey("TozBoyaKod")]
        public int? TozBoyaKodId { get; set; }
        public BoyaKod TozBoyaKod { get; set; }


        [ForeignKey("SpreyBoyaKod")]
        public int? SpreyBoyaKodId { get; set; }
        public BoyaKod SpreyBoyaKod { get; set; }


        [ForeignKey("MetalizeKod")]
        public int? MetalizeKodId { get; set; }
        public BoyaKaplama MetalizeKod { get; set; }

        [ForeignKey("GranulKod")]
        public int? GranulKodId { get; set; }
        public BoyaKaplama GranulKod { get; set; }

        public string TozBoyaKodList { get; set; }

        public bool DepodaMi { get; set; }
        public bool UretimBasladiMi { get; set; }
        public string Aciklama { get; set; }
        public string Formul { get; set; }
        public string EnjeksiyonRenk { get; set; }

        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }
        public ICollection<UretimEmir> UretimEmir { get; set; }
        public ICollection<StokHareket> StokHareket { get; set; }
        //public ICollection<BoyaKodSiparisKalipRel> BoyaKodSiparisKalipRel { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

       


    }
}