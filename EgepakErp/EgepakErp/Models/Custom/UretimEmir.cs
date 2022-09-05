using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimEmir")]
    public partial class UretimEmir
    {
        [Key]
        public int UretimEmirId { get; set; }
        public int SiparisKalipId { get; set; }
        public SiparisKalip SiparisKalip { get; set; }

        public int MakineId { get; set; }
        public Makine Makine { get; set; }


        public DateTime Baslangic { get; set; }

        public DateTime Bitis { get; set; }
        public string Durum { get; set; }
        public int UretilenAdet { get; set; }
        public int SiparisAdet { get; set; }
        public int UretimEmirDurumId { get; set; }
        public UretimEmirDurum UretimEmirDurum { get; set; }

        [NotMapped]
        public int KalanAdet
        {
            get
            {
                if (UretilenAdet != 0 && SiparisAdet != 0)
                {
                    return SiparisAdet - UretilenAdet;
                }
                return -1;
            }
        }

        [NotMapped]
        public List<string> Include { get; set; }

    }


    [Table("Makine")]
    public partial class Makine
    {
        [Key]
        public int MakineId { get; set; }
        public string MakineAd { get; set; }
        public ICollection<UretimEmir> UretimEmir { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }

}