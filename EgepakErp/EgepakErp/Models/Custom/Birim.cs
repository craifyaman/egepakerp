using System.Collections.Generic;
using EgePakErp.Models;

namespace EgePakErp.Models
{
    public class Birim
    {
        public int BirimId { get; set; }

        public string Adi { get; set; }

        public virtual ICollection<HammaddeCinsi> HammaddeCinsi { get; set; }
    }
}