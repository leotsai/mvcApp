using System;

namespace MvcApp.Core.Helpers
{
    public class KeyGenerator
    {
        private static readonly Random _random;

        static KeyGenerator()
        {
            _random = new Random();
        }

        public static string Generate(int length)
        {
            var chars = "234578QWERTYUPKJHFDSAZXCVNM".ToCharArray();
            var result = string.Empty;
            for (var i = 0; i < length; i++)
            {
                result += chars[_random.Next(0, chars.Length)];
            }
            return result;
        }

        public static string NewCaptcha(int length)
        {
            var chars = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            var result = string.Empty;
            for (var i = 0; i < length; i++)
            {
                result += chars[_random.Next(0, chars.Length)];
            }
            return result;
        }
    }
}
