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
        [NotMapped]
        public string ParcaKodu { get; set; }
        public string Adi { get; set; }
        public int UretimTeminSekliId { get; set; }
        public UretimTeminSekli UretimTeminSekli { get; set; }
        public decimal ParcaAgirlik { get; set; }
        public int KalipGozSayisi{ get; set; }
        public int UretimZamani { get; set; }
        public string Aciklama { get; set; }
        public virtual ICollection<KalipHammaddeRelation> KalipHammaddeRelation { get; set; }
        public virtual ICollection<KalipUrunRelation> KalipUrunRelation { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }

    }
}