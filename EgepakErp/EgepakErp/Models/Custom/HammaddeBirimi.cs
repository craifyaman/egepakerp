using System.Collections.Generic;
using EgePakErp.Models;

namespace EgePakErp.Controllers
{
    public class HammaddeBirimi
    {
        public int Id { get; set; }

        public string Birimi { get; set; }

        public virtual ICollection<HammaddeCinsi> HammaddeHaraket { get; set; }
    }
}