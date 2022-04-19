using System.Collections.Generic;

namespace EgePakErp.Models
{
    public class HammaddeTipi
    {
        public int Id { get; set; }

        public string Tipi { get; set; }

        public virtual ICollection<HammaddeHaraket> HammaddeHaraket { get; set; }
    }
}