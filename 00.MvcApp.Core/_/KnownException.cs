using System;

namespace MvcApp.Core
{
    public class KnownException : Exception
    {
        public string Code { get; set; }

        public KnownException(string message) : this(message, null)
        {

        }

        public KnownException(string message, string code) : base(string.IsNullOrEmpty(message) ? "Unknown Exception" : message)
        {
            this.Code = code;
        }
    }

    public class FuckException : KnownException
    {
        public FuckException() : base("系统出现严重错误即将爆炸，请抱头钻到桌子底下，听到爆炸后再出来")
        {

        }
    }
}
