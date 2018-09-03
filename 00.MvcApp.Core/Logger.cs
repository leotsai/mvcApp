using System;
using System.Collections.Generic;
using System.Timers;
using MvcApp.Core;
using MvcApp.Core.Logging;

namespace MvcApp
{
    public class Logger
    {
        public const int FLUSH_INTERVAL_SECONDS = 5;

        private static ILogAppender _appender;
        private static readonly Timer _timer;
        private static readonly List<LogItem> _items;
        private static bool _busy;

        public static Logger Debug { get; }
        public static Logger Info { get; }
        public static Logger Warning { get; }
        public static Logger Error { get; }
        public static Logger Fatal { get; }


        static Logger()
        {
            _items = new List<LogItem>();

            Debug = new Logger(LogArea.Debug);
            Info = new Logger(LogArea.Info);
            Warning = new Logger(LogArea.Warning);
            Error = new Logger(LogArea.Error);
            Fatal = new Logger(LogArea.Fatal);
            
            _timer = new Timer(FLUSH_INTERVAL_SECONDS * 1000);
            _timer.Elapsed += TimerElapsed;
        }
        
        public static void Start(ILogAppender appender)
        {
            _appender = appender;
            _timer.Start();
        }

        public static void Stop()
        {
            _timer.Stop();
        }
        
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_busy)
            {
                return;
            }
            try
            {
                _busy = true;
                List<LogItem> items;
                lock (_items)
                {
                    items = new List<LogItem>(_items);
                    _items.Clear();
                }
                _appender.Flush(items);
            }
            finally
            {
                _busy = false;
            }
        }
        

        #region logger instance members

        private readonly LogArea _area;

        private Logger(LogArea area)
        {
            this._area = area;
        }
        
        public void Entry(string group, string log)
        {
            var logRaw = _appender?.TryGetRawLog(log);
            if (logRaw != null)
            {
                this.EntryRaw(group, logRaw);
            }
        }

        public void Entry(string group, Exception exception)
        {
            var logRaw = _appender?.TryGetRawLog(exception);
            if (logRaw != null)
            {
                this.EntryRaw(group, logRaw);
            }
        }

        public void EntryRaw(string group, string logRaw)
        {
            if (_appender == null)
            {
                return;
            }
            lock (_items)
            {
                _items.Add(new LogItem(this._area, group, logRaw));
            }
        }


        #endregion



    }
}
