using EgepakErp.Validator;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EgePakErp.Models
{
    [Validator(typeof(HammaddeHareketValidator))]
    public class HammaddeHaraket
    {
        public int HammaddeHaraketId { get; set; }

        public DateTime KayitTarihi { get; set; }
        public DateTime HammaddeGirisTarihi { get; set; }
        public int HammaddeCinsiId { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public int TedarikciId { get; set; }
        public decimal KdvTutari { get; set; }

        public decimal ToplamTutar { get; set; }
        public int KdvOranı { get; set; }

        public string FaturaNo { get; set; }
        public string BelgeNo { get; set; }
        public string SiraNo { get; set; }

        public int MarkaId { get; set; }

        public int DovizId { get; set; }

        public decimal DolarKuru { get; set; }

        public decimal EuroKuru { get; set; }

        public decimal PoundKuru { get; set; }

        public int HammaddetipiId { get; set; }
        
        public DateTime FaturaTarihi { get; set; }

        public string Aciklama { get; set; }
        //Virtual
        public virtual Marka Marka { get; set; }
        public virtual HammaddeCinsi HammaddeCinsi { get; set; }
        public virtual Doviz Doviz { get; set; }
        public virtual HammaddeTipi HammaddeTipi { get; set; }
        public virtual Cari Tedarikci { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }
    }
}



