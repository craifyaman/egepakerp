namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PersonelKisitlamaRelation")]
     
    public partial class PersonelKisitlamaRelation
    {
        [Key]
        public int PersonelKisitlamaRelationId { get; set; }

        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        public int KisitlamaId { get; set; }
        public Kisitlama Kisitlama { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }

    }
}
