using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimEmirAksiyonRel")]
    public class UretimEmirAksiyonRel
    {
        [Key]
        public int UretimEmirAksiyonRelId { get; set; }
        public int UretimEmirId { get; set; }
        public UretimEmir UretimEmir { get; set; }
        public int AksiyonId { get; set; }
        public Aksiyon Aksiyon { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}