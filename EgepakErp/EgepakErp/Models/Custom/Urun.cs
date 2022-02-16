
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Table("Urun")]
    public partial class Urun
    {
        [Key]
        public int UrunId { get; set; }
        public int UrunCinsiId { get; set; }
        public UrunCinsi UrunCinsi { get; set; }
        public string UrunNo { get; set; }

        [NotMapped]
        public string UrunKodu
        {
            get
            {
                return string.Concat(this.UrunCinsi?.Kisaltmasi + this.UrunNo);
            }
            set { }
        }
        public virtual ICollection<KalipUrunRelation> KalipUrunRelation { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}