﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgepakErp.Validator
{
    public class FluentValidatorFactory : ValidatorFactoryBase
    {
        private static Dictionary<Type, IValidator> validators = new Dictionary<Type, IValidator>();

        static FluentValidatorFactory()
        {
            validators.Add(typeof(IValidator<HammaddeHareketValidator>), new HammaddeHareketValidator());
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            IValidator validator;
            if (validators.TryGetValue(validatorType, out validator))
            {
                return validator;
            }
            return validator;
        }
    }
}