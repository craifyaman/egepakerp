using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("HamUrunGrup")]

    public class HamUrunGrup
    {
        [Key]
        public int HamUrunGrupId { get; set; }
        public string UrunCinsi { get; set; }
        public string UrunNo { get; set; }
        public string UrunKodu { get { return UrunCinsi + UrunNo; } set { } }
        public string KalipNo { get; set; }
        public string KalipOzellik { get; set; }
        public string KalipKodu { get { return KalipNo + KalipOzellik; } set { } }
        public string ParcaAdi { get; set; }
        public string Hammadde { get; set; }
        public string Agirlik { get; set; }
        public string TeminSekli { get; set; }
        public string KalipSayisi { get; set; }
        public string UretimZamani { get; set; }
        public string Aciklama { get; set; }
        public string KalipEtiket { get; set; }
        public string HammaddeFormul { get; set; }
        public string YollukTip { get; set; }
        public string YollukAgirlik { get; set; }

    }
}