using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC_Project.WebBackend.CustomAttributes.Validations;
using MVC_Project.WebBackend.Utils;

namespace MVC_Project.WebBackend.Models.Attributes
{
    public class ValidTypeAttribute : ValidationAttribute
    {
        private readonly ValidType Type;
        public ValidTypeAttribute(ValidType Type)
        {
            this.Type = Type;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var word  = value?.ToString();
            var campo = "El campo " + validationContext.DisplayName;
            if (word != null && word.Trim().Length > 0)
            {
                switch (Type)
                {
                    case ValidType.ALPHABETICAL:
                        if (!word.ContainsOnlyLetters())
                        {
                            return new ValidationResult(campo +" Solo puede Contener letras");
                        }
                        break;
                    case ValidType.ALPHABETICAL_WITH_SPACES:
                        if (!word.ContainsOnlyLettersWithSpaces())
                        {
                            return new ValidationResult(campo + " Solo puede Contener letras");
                        }
                        break;
                    case ValidType.NUMERICS:
                        if (!word.ContainsOnlyNumbers())
                        {
                            return new ValidationResult(campo + " Solo puede Contener Números");
                        }
                        break;
                    case ValidType.ALPHANUMERIC:
                        if (!word.ContainsOnlyLetterAndNumbers())
                        {
                            return new ValidationResult(campo + " Solo puede Contener letras y Números");
                        }
                        break;
                    case ValidType.ALPHANUMERIC_WITH_SPACES:
                        if (!word.ContainsOnlyLetterAndNumbersWithSpaces())
                        {
                            return new ValidationResult(campo + " Solo puede Contener letras y Números");
                        }
                        break;

                }
            }

            return ValidationResult.Success;
        }
    }

}