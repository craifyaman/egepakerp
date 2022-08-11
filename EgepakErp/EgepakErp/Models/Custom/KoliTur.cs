using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("KoliTur")]
    public partial class KoliTur
    {
        [Key]
        public int KoliTurId { get; set; }
        public string Tur { get; set; }
        public string Kod { get; set; }
        public decimal KatSayi { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }


    }
}