using System;
using System.Text.RegularExpressions;

namespace Steamboat.Mobile.Validations
{
    public class PasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            var regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{7,25}$");
            var match = regex.Match(str);
            return match.Success;
        }
    }
}
