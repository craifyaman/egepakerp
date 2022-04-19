using System.Collections.Generic;

namespace EgePakErp.Models
{
    public class Marka
    {
        public int Id { get; set; }

        public string Adi { get; set; }

        public virtual ICollection<HammaddeHaraket> HammaddeHaraket { get; set; }
    }
}