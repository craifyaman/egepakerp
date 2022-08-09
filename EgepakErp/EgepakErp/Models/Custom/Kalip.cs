using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Kalip")]
    public class Kalip
    {
        [Key]
        public int KalipId { get; set; }
        public string KalipNo { get; set; }
        public string KalipOzellik { get; set; }

        /// <summary>
        /// siparis oluştururken urun cinsi seçerken otomatik oluşacak.
        /// RJ 01 00 00 01 13
        /// </summary>

        public string ParcaKodu { get; set; }
        public string Adi { get; set; }
        public int? UretimTeminSekliId { get; set; }
        public UretimTeminSekli UretimTeminSekli { get; set; }
        public decimal ParcaAgirlik { get; set; }
        public int KalipGozSayisi { get; set; }
        public int UretimZamani { get; set; }
        public string Aciklama { get; set; }
        public string YollukTipi { get; set; }
        public string YollukAgirlik { get; set; }
        public bool isAktive { get; set; }
        public string KalipEtiket { get; set; }
        public string HammaddeFormul { get; set; }
        public string Yaldiz { get; set; }
        public string KoliIciAdet { get; set; }
        public bool isHazirMalzeme { get; set; } = false;
        public string SicakBaskiAdet { get; set; }
        public string EgePakMontajAdet { get; set; }
        public string EvMontajMaliyet { get; set; }


        public virtual ICollection<KalipHammaddeRelation> KalipHammaddeRelation { get; set; }
        public virtual ICollection<KalipUrunRelation> KalipUrunRelation { get; set; }
        public virtual ICollection<SepetIcerik> SepetIcerik { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

        [NotMapped]
        public List<int> UrunList { get; set; }

        [NotMapped]
        public List<int> HammaddeList { get; set; }

    }
}