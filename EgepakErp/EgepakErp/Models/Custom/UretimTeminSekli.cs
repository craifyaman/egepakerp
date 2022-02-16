using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EgePakErp.Models
{
    [Table("UretimTeminSekli")]
    public class UretimTeminSekli
    {
        [Key]
        public int UretimTeminSekliId { get; set; }
        public string Kisaltmasi { get; set; }
        public string Adi { get; set; }
        public string Aciklamasi { get; set; }
        public virtual ICollection<Kalip> Kalip { get; set; }
         
    }
}