namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("KisitlamaControllerAction")]
     
    public class KisitlamaControllerAction
    {
        [Key]
        public int KisitlamaControllerActionId { get; set; }
        public int KisitlamaId { get; set; }
        public  Kisitlama Kisitlama { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }
    }
}