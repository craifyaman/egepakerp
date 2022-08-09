using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("HazirMalzemeFiyat")]
    public class HazirMalzemeFiyat
    {
        [Key]
        public int HazirMalzemeFiyatId { get; set; }
        public string Ad { get; set; }
        public decimal Tutar { get; set; }
        public DateTime KayitTarih { get; set; }
        public string Kod { get; set; }
        public int? DovizId { get; set; }
        public Doviz Doviz { get; set; }
        public int? TableHammaddeBirimId { get; set; }
        public TableHammaddeBirim TableHammaddeBirim { get; set; }


    }
}



