
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("UretimSabitler")]
    public partial class UretimSabitler
    {
        [Key]
        public int UretimSabitlerId { get; set; }
        public string Aciklama { get; set; }
        public string Maliyet { get; set; }
        public string Birim { get; set; }
        public int Kod { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}