using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimEmirAksiyonType")]
    public class UretimEmirAksiyonType
    {
        public int UretimEmirAksiyonTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<UretimEmirAksiyon> UretimEmirAksiyon { get; set; }
    }
}