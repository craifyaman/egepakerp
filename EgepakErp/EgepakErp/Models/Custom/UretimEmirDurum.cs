using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimEmirDurum")]
    public class UretimEmirDurum
    {
        [Key]
        public int UretimEmirDurumId { get; set; }
        public string Durum { get; set; }
        //public ICollection<UretimEmir> UretimEmir { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}