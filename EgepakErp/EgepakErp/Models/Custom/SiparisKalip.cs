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
        public string MaliyetType { get; set; }
        public bool isEnable { get; set; }
        public int? YaldizId { get; set; }
        public Yaldiz Yaldiz { get; set; }
        public int? BoyaKodId { get; set; }
        public BoyaKod BoyaKod { get; set; }

        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }
        public ICollection<UretimEmir> UretimEmir { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}