using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("DovizKur")]
    public partial class DovizKur
    {
        [Key]
        public int DovizKurId { get; set; }
        public decimal UsdKur { get; set; }
        public decimal EurKur { get; set; }
    }
}