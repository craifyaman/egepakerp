namespace EgePakErp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using EgePakErp.Models;

    [Table("Personel")]
    public partial class Personel
    {
        [Key]
        public int PersonelId { get; set; }
        public string Adi { get; set; }
        public string Kod { get; set; }
        public string Unvan { get; set; }
        public string Eposta { get; set; }
        public string Telefon { get; set; }
        public string Parola { get; set; }
        [NotMapped]
        public string YeniParola { get; set; }
        public int PersonelTipId { get; set; }
        public PersonelTip PersonelTip { get; set; }
        public int DepartmanId { get; set; }
        public Departman Departman { get; set; }
        public bool Durum { get; set; }

        public virtual ICollection<PersonelArayuzKisitlama> PersonelArayuzKisitlama { get; set; }
        public virtual ICollection<Kisi> Kisi { get; set; }
        public virtual ICollection<Gorusme> Gorusme { get; set; }
        public virtual ICollection<PersonelKisitlamaRelation> PersonelKisitlamaRelation { get; set; }
         


    }

 


    





}
