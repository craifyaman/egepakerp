﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("Urun")]
    public partial class Urun
    {
        [Key]
        public int UrunId { get; set; }
        public int UrunCinsiId { get; set; }
        public UrunCinsi UrunCinsi { get; set; }
        public string UrunNo { get; set; }
        public bool isAktif { get; set; }
        public bool isPimUsed { get; set; }
        public bool isTutkalUsed { get; set; } = false;
        public bool isSilikonYagiUsed { get; set; }
        public bool isTinerUsed { get; set; }
        //public decimal EvMaliyet { get; set; }

        [NotMapped]
        public string UrunKodu { get; set; }

        public virtual ICollection<KalipUrunRelation> KalipUrunRelation { get; set; }
        public virtual ICollection<SepetIcerik> SepetIcerik { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

        [NotMapped]
        public List<int> KalipList { get; set; }
        [NotMapped]
        public string TamAd
        {
            get
            {
                return this?.UrunCinsi?.Kisaltmasi + " " + this?.UrunNo;
            }
        }

    }
}