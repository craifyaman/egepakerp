using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EgePakErp.Models
{
    [Table("UrunCinsi")]
    public class UrunCinsi
    {
        [Key]
        public int UrunCinsiId { get; set; }
        public string Kisaltmasi { get; set; }
        public string Adi { get; set; }
        public string Aciklamasi { get; set; }
        public virtual ICollection<Urun> Urun { get; set; }
    }
}