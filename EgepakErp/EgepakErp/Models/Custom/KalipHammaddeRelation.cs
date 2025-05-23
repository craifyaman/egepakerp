﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("KalipHammaddeRelation")]
    public class KalipHammaddeRelation
    {
        [Key]
        public int KalipHammaddeRelationId { get; set; }
        public int  KalipId{ get; set; }
        public Kalip Kalip { get; set; }
        public int HammaddeCinsiId { get; set; }
        public HammaddeCinsi HammaddeCinsi { get; set; }
        [NotMapped]
        public List<string> Include { get; set; }

    }
}