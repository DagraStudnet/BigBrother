using System.Globalization;
using System.Windows.Controls;

namespace BigBrotherViewer.ViewModel
{
    public class NameValidator : ValidationRule
    {
        public override ValidationResult Validate
            (object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Name can't be empty.");
            if (value.ToString().Length < 3)
                return new ValidationResult
                    (false, "Name must be more than 3 characters long.");
            return ValidationResult.ValidResult;
        }
    }
}