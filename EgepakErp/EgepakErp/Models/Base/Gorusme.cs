using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    [Table("Gorusme")]

    public class Gorusme
    {
        [Key]
        public int GorusmeId { get; set; }
        public int KisiId { get; set; }
        public Kisi Kisi { get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public int GorusmeTipId { get; set; }
        public GorusmeTip GorusmeTip { get; set; }
        public string Konu { get; set; }
        public string Aciklama { get; set; }
        public DateTime GorusmeTarihi{ get; set; }
        public DateTime KayitTarihi{ get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}