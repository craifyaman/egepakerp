using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Brans")]
    public class Brans
    {
        [Key]
        public int BransId { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<Kisi> Cari { get; set; }

    }
}