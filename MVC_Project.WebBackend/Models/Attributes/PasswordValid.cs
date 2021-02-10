using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC_Project.WebBackend.Utils;

namespace MVC_Project.WebBackend.Models.Attributes
{
    public class PasswordValidAttribute : ValidationAttribute
    {
        public PasswordValidAttribute()
        {
        }

        public string GetErrorMessage() =>
            $"El password debe contener una letra Mayuscula, un número y un caracter especial.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var password  = value?.ToString();
            if (!password.ContainsCapitalLetter() || !password.ContainsNumbers() || !password.ContainsCaractersSpecial())
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}