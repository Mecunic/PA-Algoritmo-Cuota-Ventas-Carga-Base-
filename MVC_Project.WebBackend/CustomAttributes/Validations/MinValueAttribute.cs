using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.CustomAttributes.Validations
{
    public class MinValueAttribute : ValidationAttribute, IClientValidatable
    {
        private int minValue;

        public MinValueAttribute(int minValue)
        {
            this.minValue = minValue;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "min",
                ErrorMessage = string.Format(this.ErrorMessage, metadata.GetDisplayName(), minValue)
            };

            yield return modelClientValidationRule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int valueInt = Convert.ToInt32(value);
            if (valueInt >= minValue)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(this.ErrorMessage, validationContext.DisplayName, minValue));
        }
    }
}