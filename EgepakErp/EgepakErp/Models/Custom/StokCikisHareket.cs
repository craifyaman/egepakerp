using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("StokCikisHareket")]
    public class StokCikisHareket
    {
        [Key]
        public int StokCikisHareketId { get; set; }
        public int Adet { get; set; }
        public DateTime CikisTarih { get; set; }
        public int StokHareketId { get; set; }
        public StokHareket StokHareket { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public string Aciklama { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }


}