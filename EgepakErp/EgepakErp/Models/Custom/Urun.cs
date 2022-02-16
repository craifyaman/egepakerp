 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace EgePakErp.Models
{
    [Table("Urun")]
    public partial class Urun
    {
        [Key]
        public int UrunId { get; set; }
        public int UrunCinsiId { get; set; }
        public UrunCinsi UrunCinsi { get; set; }
        public string UrunNo { get; set; }
        public string UrunKodu { get; set; }

    }
}