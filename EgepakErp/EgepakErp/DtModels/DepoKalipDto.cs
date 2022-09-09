using System.Collections.Generic;

namespace EgePakErp.DtModels
{
    public class DepoKalipDto
    {
        public int StokHareketId { get; set; }
        public int? MontajKod { get; set; }
        public List<int> KalipKodList { get; set; }

    }
}