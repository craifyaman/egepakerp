using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimAksiyon")]
    public class UretimAksiyon
    {
        public int UretimAksiyonId { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public int UretilenAdet { get; set; }
        //public string UretimEleman { get; set; } ToDo üretim elemanı ile ilişkilendirilecek
        public int UretimEmirId { get; set; }
        public UretimEmir UretimEmir { get; set; }

        public int MakineId { get; set; }
        public Makine Makine { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }
    }

}