using System;
using System.Drawing;

namespace MvcApp.Core.Imaging
{
    public static class SizeExtensions
    {
        public static Size ResizeTo(this Size origin, Size target, bool crop = true)
        {
            if (target.Width == 0 && target.Height == 0)
            {
                return origin;
            }
            if (target.Width == 0)
            {
                return origin.ResizeByHeight(target.Height);
            }
            if (target.Height == 0)
            {
                return origin.ResizeByWidth(target.Width);
            }
            return crop ? origin.ResizeByShorterSide(target) : origin.ResizeByLongerSide(target);
        }

        public static Size ResizeByHeight(this Size origin, int targetHeight)
        {
            var rate = (float)origin.Height / targetHeight;
            if (rate <= 1)
            {
                return new Size(origin.Width, origin.Height);
            }
            return new Size((int)(origin.Width / rate), targetHeight);
        }

        public static Size ResizeByWidth(this Size origin, int targetWidth)
        {
            var rate = (float)origin.Width / targetWidth;
            if (rate <= 1)
            {
                return new Size(origin.Width, origin.Height);
            }
            return new Size(targetWidth, (int)(origin.Height / rate));
        }

        public static Size ResizeByLongerSide(this Size origin, Size target)
        {
            var size = new Size();
            var r = Math.Max(((float)origin.Width / target.Width), ((float)origin.Height) / target.Height);
            if (r <= 1)
            {
                return new Size(origin.Width, origin.Height);
            }
            size.Width = (int)Math.Round(origin.Width / r);
            size.Height = (int)Math.Round(origin.Height / r);
            if (size.Width == 0)
            {
                size.Width = 1;
            }
            if (size.Height == 0)
            {
                size.Height = 1;
            }
            return size;
        }

        public static Size ResizeByShorterSide(this Size origin, Size target)
        {
            var r = Math.Min(((float)origin.Width / target.Width), ((float)origin.Height) / target.Height);
            if (r <= 1)
            {
                return new Size(Math.Min(target.Width, origin.Width), Math.Min(target.Height, origin.Height));
            }

            var size = new Size();
            size.Width = (int)Math.Round(origin.Width / r);
            size.Height = (int)Math.Round(origin.Height / r);
            if (size.Width == 0)
            {
                size.Width = 1;
            }
            if (size.Height == 0)
            {
                size.Height = 1;
            }
            size.Width = Math.Min(size.Width, target.Width);
            size.Height = Math.Min(size.Height, target.Height);
            return size;
        }

    }
}
