using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("SiparisDurum")]
    public class SiparisDurum
    {
        [Key]
        public int SiparisDurumId { get; set; }
        public string Durum { get; set; }
        public ICollection<Siparis> Siparis { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}