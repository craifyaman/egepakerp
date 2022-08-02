using EgePakErp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgepakErp.Models.Custom
{
    [Table("HammaddeFire")]
    public class HammaddeFire
    {
        [Key]
        public int HammaddeFireId { get; set; }
        public double Oran { get; set; }
        public ICollection<HammaddeCinsi> HammaddeCinsler { get; set; }

    }
}
