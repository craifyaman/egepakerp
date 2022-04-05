using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("SepetIcerik")]
    public class SepetIcerik
    {
        [Key]
        public int SepetIcerikId { get; set; }
        public int SepetId { get; set; }
        public Sepet Sepet { get; set; }

        public int? UrunId { get; set; }
        public Urun Urun { get; set; }

        public int? KalipId{ get; set; }
        public Kalip Kalip { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}