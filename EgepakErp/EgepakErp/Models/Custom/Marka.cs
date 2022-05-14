using System.Collections.Generic;

namespace EgePakErp.Controllers
{
    public class Marka
    {
        public int Id { get; set; }

        public string Adi { get; set; }

        public virtual ICollection<HammaddeHareket> HammaddeHareket { get; set; }
    }
}