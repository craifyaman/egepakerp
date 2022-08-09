
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("Fiyat")]
    public partial class Fiyat
    {
        [Key]
        public int FiyatId { get; set; }
        public string Aciklama { get; set; }
        public string Kod { get; set; }
        public decimal Tutar { get; set; }
        public decimal ToplamTutar { get; set; }
        public DateTime KayitTarih { get; set; }
        public int? DovizId { get; set; }
        public Doviz Doviz { get; set; }
        public int? TableHammaddeBirimId { get; set; }
        public TableHammaddeBirim TableHammaddeBirim { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }


    }
}