using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.ValidateAttribute
{
    public class IsIDCardAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
                return ValidationResult.Success;
            if(!Help.Validate.IsIDCard(value.ToString()))
                return new ValidationResult(this.ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
