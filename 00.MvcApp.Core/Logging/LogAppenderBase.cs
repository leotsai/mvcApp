using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using MvcApp.Core.Extensions;

namespace MvcApp.Core.Logging
{
    public abstract class LogAppenderBase : ILogAppender
    {
        public abstract void Flush(List<LogItem> items);

        public virtual string TryGetRawLog(string log)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString());
                this.AppendRequest(sb);
                sb.Append("\r\n" + log);
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual string TryGetRawLog(Exception exception)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString());
                this.AppendRequest(sb);
                sb.AppendLine("Error Text: " + exception.GetAllMessages());
                sb.AppendLine("Error Full: \r\n" + exception);
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected void AppendRequest(StringBuilder sb)
        {
            if (HttpContext.Current == null )
            {
                return;
            }
            var request = HttpContext.Current.Request;
            sb.AppendLine($"[{request.HttpMethod}] {request.RawUrl}");
            sb.AppendLine("IP: " + request.UserHostAddress);
            foreach (var header in request.Headers.AllKeys)
            {
                sb.AppendLine($"{header}:{request.Headers.Get(header)}");
            }
        }

    }
}
