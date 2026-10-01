using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class HtmlHelper
    {

        public static string RemoveHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}
