using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("Yaldiz")]
    public partial class Yaldiz
    {
        [Key]
        public int YaldizId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public string Aciklama { get; set; }

        public string PdfYol { get; set; }
        public ICollection<SiparisKalip> SiparisKalip { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}