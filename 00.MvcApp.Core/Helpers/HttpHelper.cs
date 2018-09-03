using System;
using System.Net;
using System.Text;

namespace MvcApp.Core.Helpers
{
    public class HttpHelper
    {
        public static T Get<T>(string url) where T : class 
        {
            var html = DownloadString(url);
            return Serializer.FromJson<T>(html);
        }

        public static T Post<T>(string url, string data)
        {
            string html;
            using (var client = new WebClient())
            {
                var result = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(data ?? string.Empty));
                html = Encoding.UTF8.GetString(result);
            }
            return Serializer.FromJson<T>(html);
        }

        public static string DownloadString(string url)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.DownloadString(url);
            }
        }

        public static string UploadData(string url, string data)
        {
            using (var client = new WebClient())
            {
                var result = client.UploadData(url, Encoding.UTF8.GetBytes(data));
                return Encoding.UTF8.GetString(result);
            }
        }

        public static T UploadFile<T>(string url, string filePath)
        {
            string html;
            using (var client = new WebClient())
            {
                var result = client.UploadFile(url, "POST", filePath);
                html = Encoding.UTF8.GetString(result);
            }
            return Serializer.FromJson<T>(html);
        }

    }
}
