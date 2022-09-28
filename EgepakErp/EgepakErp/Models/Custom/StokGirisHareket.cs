using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("StokGirisHareket")]
    public class StokGirisHareket
    {
        [Key]
        public int StokGirisHareketId { get; set; }
        public int Adet { get; set; }
        public DateTime GirisTarih { get; set; }
        public int StokHareketId { get; set; }
        public StokHareket StokHareket { get; set; }
        public string Aciklama { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }


}