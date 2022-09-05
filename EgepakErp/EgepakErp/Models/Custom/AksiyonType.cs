using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("AksiyonType")]
    public class AksiyonType
    {
        [Key]
        public int AksiyonTypeId { get; set; }
        public string Aciklama { get; set; }
        public ICollection<Aksiyon> Aksiyon { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}