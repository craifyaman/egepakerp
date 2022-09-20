using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.Mvc;

namespace EgePakErp.Models
{
    [Table("StokHareket")]
    public class StokHareket
    {
        [Key]
        public int StokHareketId { get; set; }
        public int StokHareketTypeId { get; set; }
        public StokHareketType StokHareketType { get; set; }

        public int SiparisId { get; set; }
        public Siparis Siparis { get; set; }

        public int SiparisKalipId { get; set; }
        public SiparisKalip SiparisKalip { get; set; }

        public int? Adet { get; set; }
        public bool MontajliMi { get; set; }
        public int? MontajKod { get; set; }

        public string Yer { get; set; }
        public ICollection<StokCikisHareket> StokCikisHareket { get; set; }

        [NotMapped]
        public int? DepodaKalanAdet
        {
            get
            {
                var cikanToplam = StokCikisHareket?.Sum(x => x.Adet);
                return Adet - cikanToplam;
            }
        }


        [NotMapped]
        public List<string> Include { get; set; }


    }


}