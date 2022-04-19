using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("HammaddeCinsi")]
    public class HammaddeCinsi
    {
        [Key] public int HammaddeCinsiId { get; set; }
        public string Kisaltmasi { get; set; }
        public string Adi { get; set; }
        public string Aciklamasi { get; set; }
        //public int? HammaddeBirimiId { get; set; }
        //public HammaddeBirimi HammaddeBirimi { get; set; }

        public int? BirimId { get; set; }
        public Birim Birim { get; set; }

        public virtual ICollection<Kalip> Kalip { get; set; }
        public virtual ICollection<KalipHammaddeRelation> KalipHammaddeRelation { get; set; }
        [NotMapped] 
        public List<string> Include { get; set; }
    }
}