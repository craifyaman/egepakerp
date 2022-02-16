using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Il")]
    public class Il
    {
        [Key]
        public int IlId { get; set; }
        public string Adi { get; set; }
        public int? UlkeId { get; set; }
        public Ulke Ulke { get; set; }
        public virtual ICollection<Ilce> Ilce { get; set; }
        public virtual ICollection<Cari> Cari { get; set; }

    }
}