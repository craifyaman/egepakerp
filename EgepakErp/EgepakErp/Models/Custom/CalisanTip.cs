using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EgePakErp.Models
{
    public class CalisanTip
    {
        [Key]
        public int CalisanTipId { get; set; }
        public string Tip { get; set; }
        public ICollection<Calisan> Calisan { get; set; }
    }

}