﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("UretimEmirAksiyon")]
    public class UretimEmirAksiyon
    {
        public int UretimEmirAksiyonId { get; set; }
        public int UretimEmirAksiyonTypeId { get; set; }
        public UretimEmirAksiyonType UretimEmirAksiyonType { get; set; }
        public int UretimEmirId { get; set; }
        public UretimEmir UretimEmir { get; set; }
        public int BitenAdet { get; set; }
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public DateTime KayitTarih { get; set; }

        public int? MakineId { get; set; }
        public Makine Makine { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }

    }
}