using System;

namespace MvcApp.Core.Extensions
{
    public static class DecimalExtentions
    {
        public static bool IsNullOrZero(this decimal? value)
        {
            return value == null || value.Value == 0;
        }

        public static string ToText(this decimal value)
        {
            return Math.Round(value, 2).ToString("g0");
        }

        public static string ToMaxP1(this decimal? value)
        {
            return value == null ? "--" : value.Value.ToMaxP1();
        }

        public static string ToMaxP1(this decimal value)
        {
            return Math.Round(value*100, 1).ToString("g0") + "%";
        }
    }
}
