using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace MvcApp.Core.Helpers
{
    public class Qr
    {
        public static Image Create(string text)
        {
            using (var ms = new MemoryStream())
            {
                var encoder = new QrEncoder(ErrorCorrectionLevel.H);
                var qr = encoder.Encode(text);
                var render = new GraphicsRenderer(new FixedModuleSize(6, QuietZoneModules.Four));

                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                return Image.FromStream(ms);
            }
        }
    }
}
