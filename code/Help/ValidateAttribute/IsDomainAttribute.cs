using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.ValidateAttribute
{
    public class IsDomainAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            if (!Help.Validate.IsDomain(value.ToString()))
                return new ValidationResult(this.ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
