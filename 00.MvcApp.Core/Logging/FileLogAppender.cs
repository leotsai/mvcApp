using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MvcApp.Core.Helpers;

namespace MvcApp.Core.Logging
{
    public class FileLogAppender : LogAppenderBase
    {
        public const long MAX_BYTES_PER_FILE = 10240000;

        public override void Flush(List<LogItem> items)
        {
            foreach (var area in items.GroupBy(x => x.Area))
            {
                foreach (var group in area.GroupBy(x => x.Group))
                {
                    var folder = $"\\_logs\\{area}\\{group}\\{DateTime.Today:yyyy-MM}";
                    var file = new FileInfo(Path.Combine(AppContext.RootFolder, folder + $"\\{DateTime.Today:yyyy-MM-dd}.log"));
                    IoHelper.CreateDirectoryIfNotExists(file.DirectoryName);
                    var length = file.Length;
                    var path = file.FullName;
                    foreach (var item in group)
                    {
                        if (length + item.Log.Length > MAX_BYTES_PER_FILE)
                        {
                            path = Path.Combine(AppContext.RootFolder, folder + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".log");
                            length = 0;
                        }
                        File.AppendAllText(path, item.Log);
                        length += item.Log.Length;
                    }
                }
            }
        }
    }

}
