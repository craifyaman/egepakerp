using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Kategori")]
    public partial class Kategori
    {
        [Key]
        public int KategoriId { get; set; }
        public string Adi { get; set; }
        public ICollection<HammaddeCinsi> HammaddeCinsler { get; set; }
        public ICollection<HammaddeHareket> HammaddeHareketler { get; set; }


        [NotMapped]
        public List<string> Include { get; set; }

    }
}