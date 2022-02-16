namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PersonelTip")] 
    public partial class PersonelTip
    {
        [Key]
        public int PersonelTipId { get; set; }
        public string Adi { get; set; }
        
        public virtual ICollection<Personel> Personel { get; set; }
    }




}
