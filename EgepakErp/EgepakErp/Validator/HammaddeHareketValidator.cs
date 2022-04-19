using EgePakErp.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgepakErp.Validator
{
     

    public class HammaddeHareketValidator : AbstractValidator<HammaddeHaraket>
    {
        public HammaddeHareketValidator()
        {
            //RuleFor(h => h.BirimFiyat).Null().WithMessage("Birim Fiyat Boş Bırakılamaz");
        }
    }
}