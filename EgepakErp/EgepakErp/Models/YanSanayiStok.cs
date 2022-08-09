using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("YanSanayiStok")]

    public class YanSanayiStok
    {
        [Key]
        public int YanSanayiStokId { get; set; }
        public string UrunAdi { get; set; }
        public string Kod { get; set; }
        public string YanMamul { get; set; }
        public decimal BirimFiyat { get; set; }

        public int? DovizId { get; set; }
        public Doviz Doviz { get; set; }
        public int? TableHammaddeBirimId { get; set; }
        public TableHammaddeBirim TableHammaddeBirim { get; set; }
    }
}