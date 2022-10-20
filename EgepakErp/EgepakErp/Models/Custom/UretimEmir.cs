using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EgePakErp.Models
{
    [Table("UretimEmir")]
    public class UretimEmir
    {
        [Key]
        public int UretimEmirId { get; set; }
        public int SiparisKalipId { get; set; }
        public SiparisKalip SiparisKalip { get; set; }
        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }

        public int MakineId { get; set; }
        public Makine Makine { get; set; }


        public DateTime? Baslangic { get; set; }

        public DateTime? Bitis { get; set; }
        public DateTime? TamamlanmaTarih { get; set; }
        //public int UretilenAdet { get; set; }
        public int SiparisAdet { get; set; }

        //public int UretimEmirDurumId { get; set; }
        //public UretimEmirDurum UretimEmirDurum { get; set; }

        //public string UretimEmirDurumList { get; set; }


        public bool? isUretimBitti { get; set; } //enjeksiyon
        public bool? isSicakBaskiBitti { get; set; }
        public bool? isSpreyBoyaBitti { get; set; }
        public bool? isMetalizeBitti { get; set; }
        public bool? isMontajBitti { get; set; }
        public bool? isEvMontajBitti { get; set; }
         
   
        public bool SicakBaskiYapilacak { get; set; }
     
        public bool SpreyYapilacak { get; set; }
     
        public bool MetalizeYapilacak { get; set; }
     
        public bool MontajYapilacak { get; set; }
    
        public bool EvMontajYapilacak { get; set; }
        public bool DepodaMi { get; set; }

        public int? KisiId { get; set; }
        public Kisi Kisi { get; set; }

        //public ICollection<Aksiyon> Aksiyon { get; set; }
        //public ICollection<UretimAksiyon> UretimAksiyon { get; set; }
        public ICollection<UretimEmirAksiyon> UretimEmirAksiyon { get; set; }

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


        //[NotMapped]
        //public int UretilenAdet
        //{
        //    get
        //    {
        //        return UretimAksiyon.Sum(x => x.UretilenAdet);
        //    }
        //}

    }
}