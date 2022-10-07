using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("SiparisTeklifFormUrun")]
    public class SiparisTeklifFormUrun
    {
        [Key]
        public int SiparisTeklifFormUrunId { get; set; }
        public string Kod { get; set; }
        public string UrunAdi { get; set; }

        [Column(TypeName = "money")]
        public decimal Teklif { get; set; }

        public int SiparisTeklifFormId { get; set; }
        public SiparisTeklifForm SiparisTeklifForm { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }


}