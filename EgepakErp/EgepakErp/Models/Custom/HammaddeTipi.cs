using System.Collections.Generic;

namespace EgePakErp.Controllers
{
    public class HammaddeTipi
    {
        public int Id { get; set; }

        public string Tipi { get; set; }

        public virtual ICollection<HammaddeHareket> HammaddeHareket { get; set; }
    }
}