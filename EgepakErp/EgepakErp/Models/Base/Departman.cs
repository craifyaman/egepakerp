namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Departman")]
     
    public partial class Departman
    {
        [Key]
        public int DepartmanId { get; set; }
        public string Adi { get; set; }
         
        public virtual ICollection<Personel> Personel { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }
    }




}
