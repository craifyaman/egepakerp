using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Not")]
    public class Not
    {
        [Key]
        public int NotId { get; set; }
        public int CariId { get; set; }
        public Cari Cari{ get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
       
        public string Aciklama { get; set; }
       
        public DateTime KayitTarihi{ get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}