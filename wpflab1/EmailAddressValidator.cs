using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace wpflab1
{
    public class EmailAddressValidator : ValidationRule
    {
        public Regex regex = new Regex(@"^[\w.-]+@(?=[a-z\d][^.]*\.)[a-z\d.-]*(?<![.-])$");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            Match match = regex.Match(str);

            if (!match.Success)
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, String.Format("Email address must have a proper formayt"));
        }
    }
}
