using System;
using System.Collections.Generic;

namespace MvcApp.Core.Logging
{
    public interface ILogAppender
    {
        string TryGetRawLog(string log);
        string TryGetRawLog(Exception exception);
        void Flush(List<LogItem> items);
    }
}
