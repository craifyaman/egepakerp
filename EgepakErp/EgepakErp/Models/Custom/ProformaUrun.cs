using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("ProformaUrun")]
    public class ProformaUrun
    {
        [Key]
        public int ProformaUrunId { get; set; }
        public string Aciklama { get; set; }
        public int Adet { get; set; }

        [Column(TypeName = "money")]
        public decimal BirimFiyat { get; set; }

        [NotMapped]
        public decimal Tutar
        {
            get
            {
                return Adet * BirimFiyat;
            }
        }

        public int ProformaFaturaId { get; set; }
        public ProformaFatura ProformaFatura { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }


}