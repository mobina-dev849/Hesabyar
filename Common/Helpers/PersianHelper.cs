using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{

    public static class PersianHelper
    {
        public static string ConvertArabicToPersian(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text
                .Replace('ي', 'ی')
                .Replace('ى', 'ی')
                .Replace('ك', 'ک');
        }

        public static string ConvertPersianNumbersToEnglish(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            char[] result = text.ToCharArray();

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] >= '۰' && result[i] <= '۹')
                {
                    result[i] = (char)('0' + result[i] - '۰');
                }
            }

            return new string(result);
        }
    }
}

