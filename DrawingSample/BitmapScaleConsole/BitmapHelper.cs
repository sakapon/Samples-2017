using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BitmapScaleConsole
{
    public static class BitmapHelper
    {
        public static Bitmap GetScreenBitmap(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(x, y, 0, 0, bitmap.Size);
            }
            return bitmap;
        }

        // Bilinear (Default) or HighQualityBilinear.
        public static Bitmap ScaleImage(Image source, int width, int height, InterpolationMode interpolationMode = InterpolationMode.HighQualityBilinear)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage(source, 0, 0, width, height);
            }
            return bitmap;
        }
    }
}
