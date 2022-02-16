using System.ComponentModel.DataAnnotations;

namespace EgePakErp.Models
{
    public class BaglantiTipi
    {
        [Key]
        public int BaglantiTipiId { get; set; }
        public string Adi { get; set; }
    }
}