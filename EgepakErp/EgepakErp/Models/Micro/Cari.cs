using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{

    public partial class Cari
    {
        public int? BaglantiTipiId { get; set; }
        public BaglantiTipi BaglantiTipi { get; set; }
        public string MuhasebeKodu1 { get; set; }
        public string MuhasebeKodu2 { get; set; }
        public string MuhasebeKodu3 { get; set; }
        public decimal AnaDovizBakiye { get; set; }
        public decimal AlternatifDovizBakiye { get; set; }
        public decimal Bakiye1 { get; set; }
        public decimal Bakiye2 { get; set; }
        public decimal Bakiye3 { get; set; }

        [ForeignKey("Doviz1")]
        public int? Doviz1Id { get; set; }
        public Doviz Doviz1 { get; set; }

        [ForeignKey("Doviz2")]
        public int? Doviz2Id { get; set; }
        public Doviz Doviz2 { get; set; }

        [ForeignKey("Doviz3")]
        public int? Doviz3Id { get; set; }
        public Doviz Doviz3 { get; set; }

    }
}