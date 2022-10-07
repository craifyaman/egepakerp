using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Makine")]
    public class Makine
    {
        [Key]
        public int MakineId { get; set; }
        public string MakineAd { get; set; }
        public ICollection<UretimEmir> UretimEmir { get; set; }
       public ICollection<UretimAksiyon> UretimAksiyon { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }

}