using System;
using System.Text.RegularExpressions;

namespace MvcApp.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool Eq(this string input, string toCompare, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (input == null)
            {
                return toCompare == null;
            }
            return input.Equals(toCompare, comparison);
        }
        
        public static Guid? ToGuid(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            Guid id;
            if (Guid.TryParse(str, out id))
            {
                return id;
            }
            return null;
        }

        public static int? ToInt32(this string str)
        {
            int value;
            if (int.TryParse(str, out value))
            {
                return value;
            }
            return null;
        }
        
        public static bool IsEmail(this string value)
        {
            var reg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return string.IsNullOrEmpty(value) == false && reg.IsMatch(value);
        }

        public static bool IsPhone(this string value)
        {
            var reg = new Regex("1[3|5|7|8|][0-9]{9}");
            return string.IsNullOrEmpty(value) == false && reg.IsMatch(value);
        }

        public static string ToHtml(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            if (input.IndexOf("<script", StringComparison.OrdinalIgnoreCase) > -1)
            {
                throw new KnownException("不能输入脚本");
            }
            return input.Replace(">", "&gt;").Replace("<", "&lt;").Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        }

        public static string ToPlainText(this string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }
            return html.Replace("<br/>", "\n");
        }
    }
}
