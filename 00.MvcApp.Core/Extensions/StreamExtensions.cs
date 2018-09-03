using System.IO;

namespace MvcApp.Core.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadAll(this Stream stream)
        {
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
    }
}
