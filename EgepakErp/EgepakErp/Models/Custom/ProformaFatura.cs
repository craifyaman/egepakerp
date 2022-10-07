using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace EgePakErp.Models
{
    [Table("ProformaFatura")]
    public class ProformaFatura
    {
        [Key]
        public int ProformaFaturaId { get; set; }
        public int CariId { get; set; }
        public Cari Cari { get; set; }
        public DateTime KayitTarih { get; set; }
        public string Gonderen { get; set; }
        public string GonderenEposta { get; set; }
        public string GonderenTel { get; set; }

        public string Firma { get; set; }
        public string Adres { get; set; }
        public string Yetkili { get; set; }
        public string AlanEposta { get; set; }
        public string AlanTel { get; set; }


        public DateTime Tarih { get; set; }
        public int DovizId { get; set; }
        public Doviz Doviz { get; set; }

        
        [NotMapped]
        public decimal Toplam
        {
            get
            {
                return ProformaUrun.Sum(x => x.Tutar);
            }
        }

        [Column(TypeName = "money")]
        public decimal OnOdeme { get; set; }

        [NotMapped]
        public decimal ToplamTutar
        {
            get
            {
                return Toplam - OnOdeme;

            }
        }

        public ICollection<ProformaUrun> ProformaUrun { get; set; }

        [AllowHtml]
        public string Aciklama { get; set; }

        [NotMapped]
        public List<string> Include { get; set; }


    }
}