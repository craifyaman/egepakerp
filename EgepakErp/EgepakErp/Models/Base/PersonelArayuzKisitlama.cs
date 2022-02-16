namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
      
    [Table("PersonelArayuzKisitlama")]
    public partial class PersonelArayuzKisitlama
    {
        [Key]
        public int PersonelArayuzKisitlamaId { get; set; }
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public int ArayuzKisitlamaId { get; set; }

        public virtual ArayuzKisitlama ArayuzKisitlama { get; set; }
    }
}
