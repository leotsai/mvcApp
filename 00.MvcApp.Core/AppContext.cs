using System;
using System.Configuration;

namespace MvcApp
{
    public class AppContext
    {
        private static Random _random;

        /// <summary>
        /// D:\wwws\www.DabenPm.com
        /// </summary>
        public static string RootFolder { get; }

        /// <summary>
        /// http://vpm.daben100.com
        /// </summary>
        public static string Host { get; }
        public static string AdminKey { get; }
        public const string SmsUsername = "";
        public const string SmsPassword = "";

        public static int CaptchaExpireMinutes = 10;
        public const string AppName = "大本科技";
        public static string ConnectionString { get; }
        public static int MaxPages = 10;
        public const int LoginExpireMinutes = 600;
        public const string Md5Key = "DB9MbEn";

        static AppContext()
        {
            RootFolder = ConfigurationManager.AppSettings["RootFolder"];
            Host = ConfigurationManager.AppSettings["Host"];
            AdminKey = ConfigurationManager.AppSettings["AdminKey"];
            _random = new Random();
            ConnectionString = ConfigurationManager.ConnectionStrings["DabenPmDbContext"].ConnectionString;
        }


        /// <summary>
        /// returns /_storage/image originals/{userid}/{year}-{month}/
        /// </summary>
        public static string GetImageOriginalFolder(Guid? userId)
        {
            return $"/_storage/image originals/{(userId == null ? "_system" : userId.ToString())}/{DateTime.Now.Year}-{DateTime.Now.Month}/";
        }

        /// <summary>
        /// returns /_storage/image sized/{year}-{month}/{imageId}/
        /// </summary>
        public static string GetSizedImageFolder(Guid imageId)
        {
            return $"/_storage/image sized/{DateTime.Now.Year}-{DateTime.Now.Month}/{imageId}/";
        }

        public class Weixin
        {
            public const string AppId = "wxbf0df56812130d9d";
            public const string AppSecret = "8d592f0d0a2fd7b9bae2e885b4ea8f18";
            public const string MessageToken = "PmBigB3n";
            public const string PayMchId = "1374552602";
            public const string PayKey = "dab3nprm2016wxpaykeydabrmmmdaben";
            public static string PayNotifyUrl => Host + "/app/weixin/paynotify";
            
            public class Redpack
            {
                public const string SendName = "大本科技";
                public const string Wishing = "大本科技红包";
                public const string ActName = "大本科技发红包了";
                public const string Remark = "大本科技微信红包";
                public const string ClientIp = "110.110.110.110";
                public const string CerPath = @"D:\wwws\www.dabenpm.com\_cert\apiclient_cert.p12";
            }
        }

        public class Qq
        {
            public const string AppId = "101330720";
            public const string AppKey = "1616eb2294f395a20ede33c5462ce390";
        }

        public class Urls
        {
            public static string AppProjectDetails(string projectKey)
            {
                return Host + "/app/p/" + projectKey;
            }
            
            public static string AppTaskDetails(string pkey, Guid taskId)
            {
                return Host + "/app/p/" + pkey + "/task/details/" + taskId;
            }
            
            public static string AppPmBatchReview()
            {
                return Host + "/app/pm/PendingCloses";
            }

            public static string AppReadWorkReport(Guid reportId)
            {
                return Host + "/app/workreport/read/" + reportId;
            }

            public static string AppSalaryDetails(Guid salaryId)
            {
                return Host + "/app/salary/read/" + salaryId;
            }
        }

        public static int Random(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
