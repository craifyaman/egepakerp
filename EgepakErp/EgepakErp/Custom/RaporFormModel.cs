using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgePakErp.Custom
{
    public class RaporFormModel<SatırIcerikTipi, BaslikIcerikTipi>
    {
        public List<Baslik<BaslikIcerikTipi, SatırIcerikTipi>> Basliklar { get; set; }
        public List<Satır<SatırIcerikTipi>> Satırlar { get; set; }
        public SatırIcerikTipi Toplam { get; set; }
        public int SatirSayisi { get; set; }
    }

    public class Baslik<BaslikIcerikTipi, SatırIcerikTipi>
    {
        public int Index { get; set; }
        public BaslikIcerikTipi Icerik { get; set; }
        public int Sıra { get; internal set; }
        public SatırIcerikTipi SutunToplamı { get; set; }
    }

    public class Satır<SatırIcerikTipi>
    {
        public string Baslik { get; set; }
        public List<SatirIcerik<SatırIcerikTipi>> SatirICerikler { get; set; }
        public SatırIcerikTipi SatirToplam { get; set; }
    }

    public class SatirIcerik<SatırIcerikTipi>
    {
        public int Index { get; set; }
        public SatırIcerikTipi Icerik { get; set; }
        public SatırIcerikTipi Icerik2 { get; set; }
        public List<int> IdList { get; set; }
    }

    public class Row
    {
        public string Alan { get; set; }
        public string Alan1 { get; set; }
        public string Alan2 { get; set; }
        public string Alan3 { get; set; }
    }

}