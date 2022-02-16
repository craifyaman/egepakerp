using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Ilce")]
    public class Ilce
    {
        [Key]
        public int IlceId { get; set; }
        public int IlId { get; set; }
        public Il Il { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<Cari> Cari { get; set; }
    }
}