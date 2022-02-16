using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Ulke")]
    public class Ulke
    {
        [Key]
        public int UlkeId { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<Il> Il { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }

    }
}