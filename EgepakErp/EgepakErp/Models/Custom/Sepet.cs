using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Sepet")]
    public class Sepet
    {
        [Key]
        public int SepetId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}