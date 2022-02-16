using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("KalipUrunRelation")]
    public class KalipUrunRelation
    {
        [Key]
        public int KalipUrunRelationId { get; set; }
        public int  KalipId{ get; set; }
        public Kalip Kalip { get; set; }
        public int UrunId { get; set; }
        public Urun Urun { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}