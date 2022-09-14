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
        //public int UretilenAdet { get; set; }
        public int SiparisAdet { get; set; }
        public int UretimEmirDurumId { get; set; }
        public UretimEmirDurum UretimEmirDurum { get; set; }
        public bool isUretimBitti { get; set; }
        public bool? isSicakBaskiBitti { get; set; }
        public bool? isSpreyBoyaBitti { get; set; }
        public bool? isMetalizeBitti { get; set; }
        public bool? isMontajBitti { get; set; }
        public bool? isEvMontajBitti { get; set; }

        [NotMapped]
        public bool SicakBaskiYapilacak { get; set; }
        [NotMapped]
        public bool SpreyYapilacak { get; set; }
        [NotMapped]
        public bool MetalizeYapilacak { get; set; }
        [NotMapped]
        public bool MontajYapilacak { get; set; }
        [NotMapped]
        public bool EvMontajYapilacak { get; set; }

        public int? KisiId { get; set; }
        public Kisi Kisi { get; set; }

        public ICollection<Aksiyon> Aksiyon { get; set; }

        //[NotMapped]
        //public int KalanAdet
        //{
        //    get
        //    {
        //        if (UretilenAdet != 0 && SiparisAdet != 0)
        //        {
        //            return SiparisAdet - UretilenAdet;
        //        }
        //        return -1;
        //    }
        //}

        [NotMapped]
        public List<string> Include { get; set; }

    }

}