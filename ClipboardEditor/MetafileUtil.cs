using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardEditor
{
    public static class MetafileUtil
    {
        public static MemoryStream ToStream(Metafile wmf)
        {
            var stream = new MemoryStream();

            using (var graphics = Graphics.FromImage(new Bitmap(1, 1, PixelFormat.Format32bppArgb)))
            {
                var hdc = graphics.GetHdc();

                using (var dummy = Graphics.FromImage(new Metafile(stream, hdc)))
                {
                    dummy.DrawImage(wmf, 0, 0, wmf.Width, wmf.Height);
                    dummy.Flush();
                }

                graphics.ReleaseHdc(hdc);
            }

            return stream;
        }

        public static Metafile FromStream(MemoryStream stream)
        {
            using (var bitmap = new Bitmap(stream))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    var hdc = g.GetHdc();
                    var wmf = new Metafile(stream, hdc);
                    g.ReleaseHdc();

                    return wmf;
                }
            }
        }
    }

}
