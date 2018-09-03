using System;

namespace MvcApp.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri AppendQuery(this Uri uri, string query)
        {
            var q = query.StartsWith("?") ? query.Substring(1) : query;
            var resultUrl = uri.OriginalString + (string.IsNullOrEmpty(uri.Query) ? "?" : "&") + q;
            return new Uri(resultUrl);
        }
    }
}
