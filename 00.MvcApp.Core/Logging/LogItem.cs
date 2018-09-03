namespace MvcApp.Core.Logging
{
    public class LogItem
    {
        public LogArea Area { get; }
        public string Group { get; }
        public string Log { get; }

        public LogItem(LogArea area, string group, string log)
        {
            this.Area = area;
            this.Group = group;
            this.Log = log;
        }
    }
}
