namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Kisitlama")]
     
    public partial class Kisitlama
    {
        [Key]
        public int KisitlamaId { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public string Grup { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string NameSpace { get; set; }

        public virtual ICollection<PersonelKisitlamaRelation> PersonelKisitlamaRelation { get; set; }
        public virtual ICollection<KisitlamaControllerAction> KisitlamaControllerAction { get; set; }

    }
}
