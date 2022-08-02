using EgepakErp.Models.Custom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace EgePakErp.Models
{
    [Table("HammaddeCinsi")]
    public class HammaddeCinsi
    {
        public HammaddeCinsi()
        {

        }
        public HammaddeCinsi(string kisaltmasi, string adi, string aciklamasi, bool isAktive)
        {
            Kisaltmasi = kisaltmasi;
            Adi = adi;
            Aciklamasi = aciklamasi;
            this.isAktive = isAktive;
        }

        [Key] 
        public int HammaddeCinsiId { get; set; }
        public string Kisaltmasi { get; set; }
        public string Adi { get; set; }
        public string Aciklamasi { get; set; }
        public bool isAktive { get; set; }
        //public int? BirimId { get; set; }

        //public int HammaddeBirimiId { get; set; }
        //public HammaddeBirimi HammaddeBirimi { get; set; }

        public int? TableHammaddeBirimId { get; set; }
        public TableHammaddeBirim TableHammaddeBirim { get; set; }

        public int? HammaddeFireId { get; set; }
        public HammaddeFire HammaddeFire { get; set; }

        public virtual ICollection<Kalip> Kalip { get; set; }
        public virtual ICollection<KalipHammaddeRelation> KalipHammaddeRelation { get; set; }
        public virtual ICollection<HammaddeHareket> HammaddeHareket { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }
    }
}