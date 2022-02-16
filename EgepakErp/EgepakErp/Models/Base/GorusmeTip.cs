using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("GorusmeTip")]

    public class GorusmeTip
    {
        [Key]
        public int GorusmeTipId { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<Gorusme> Gorusme { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }
    }
}