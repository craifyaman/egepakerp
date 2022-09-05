using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Aksiyon")]
    public class Aksiyon
    {
        [Key]
        public int AksiyonId { get; set; }
        public string Aciklama { get; set; }
        public int AksiyonTypeId { get; set; }
        public AksiyonType AksiyonType { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }
}