using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.ValidateAttribute
{
    public class IsDecimalPoint2ValidateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            if (!Help.Validate.IsDecimalPoint2(value.ToString(), true))
                return new ValidationResult(this.ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
