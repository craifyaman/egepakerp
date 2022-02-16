using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public string TeminŞekli { get; set; }
        public string KalıpSayisi { get; set; }
        public string UretimZamani { get; set; }
        public string Aciklama { get; set; }

    }
}