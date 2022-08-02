using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("TableHammaddeBirim")]
    public class TableHammaddeBirim
    {
        public int TableHammaddeBirimId { get; set; }
        public string Birimi { get; set; }

        public virtual ICollection<HammaddeCinsi> HammaddeCinsi { get; set; }
        public virtual ICollection<HammaddeHareket> HammaddeHareket { get; set; }
    }
}