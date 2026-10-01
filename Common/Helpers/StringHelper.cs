using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public static class StringHelper
    {
        public static string ToNumeric(this decimal amount)
        {
            return amount.ToString("N0");
        }
    }
}
