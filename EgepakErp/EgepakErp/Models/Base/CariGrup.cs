using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("CariGrup")]
    public class CariGrup
    {
        [Key]
        public int CariGrupId { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<Cari> Cari { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}