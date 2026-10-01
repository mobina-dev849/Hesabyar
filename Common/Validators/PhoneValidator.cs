using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Validators
{
    internal class PhoneValidator
    {
        public static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^09[0-9]{9}$");
        }

    }
}
