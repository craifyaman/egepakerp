﻿using System.ComponentModel.DataAnnotations;
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
        public string KalipKodu { get; set; }
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
        public string Yaldiz { get; set; }
        public string KoliIciAdet { get; set; }

        public string SicakBaskiAdet { get; set; }
        public string EgePakMontajAdet { get; set; }
        public string EvMontajMaliyet { get; set; }
        public string KromPlastMetalizeBrFiyat { get; set; }
        public string OncelikMakine { get; set; }
        public string AlternatifMakine { get; set; }



    }
}