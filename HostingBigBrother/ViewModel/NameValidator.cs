using System.Globalization;
using System.Windows.Controls;

namespace HostingBigBrother.ViewModel
{
    public class NameValidator : ValidationRule
    {
        public override ValidationResult Validate
            (object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "value cannot be empty.");
            if (value.ToString().Length > 3)
                return new ValidationResult
                    (false, "Name cannot be more than 3 characters long.");
            return ValidationResult.ValidResult;
        }
    }
}