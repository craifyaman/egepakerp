using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("EvMontaj")]

    public class EvMontaj
    {
        [Key]
        public int EvMontajId { get; set; }
        public string UrunCins { get; set; }
        public string UrunNo { get; set; }
        public string Aciklama { get; set; }
        public decimal Maliyet { get; set; }

    }
}