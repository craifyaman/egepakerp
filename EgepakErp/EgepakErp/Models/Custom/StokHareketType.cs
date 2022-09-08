using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("StokHareketType")]
    public class StokHareketType
    {
        [Key]
        public int StokHareketTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<StokHareket> StokHareket { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}