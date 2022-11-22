using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Calisan")]
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }
        public string TcKimlik { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public string AdSoyad { get; set; }
        public bool Aktif { get; set; }
        public int CalisanTipId { get; set; }
        public CalisanTip CalisanTip { get; set; }
        public ICollection<UretimEmirAksiyon> UretimEmirAksiyon { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }

    }

}