using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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

        public static Bitmap ResizeImage(Image source, Size size, InterpolationMode interpolationMode = InterpolationMode.Bilinear) =>
            ResizeImage(source, size.Width, size.Height, interpolationMode);

        // Default = Low = Bilinear
        // High = HighQualityBicubic
        public static Bitmap ResizeImage(Image source, int width, int height, InterpolationMode interpolationMode = InterpolationMode.Bilinear)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage(source, 0, 0, width, height);
            }
            return bitmap;
        }

        public static void SaveImage(string filePath, Image image)
        {
            var fullPath = Path.GetFullPath(filePath);
            var format = ToImageFormat(fullPath);

            image.Save(fullPath, format);
        }

        static ImageFormat ToImageFormat(string filePath)
        {
            switch (Path.GetExtension(filePath).ToLowerInvariant())
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tiff":
                case ".tif":
                    return ImageFormat.Tiff;
                case ".ico":
                    return ImageFormat.Icon;
                case ".wmf":
                    return ImageFormat.Wmf;
                case ".emf":
                    return ImageFormat.Emf;
                default:
                    throw new ArgumentException("Can not encode bitmaps for the specified file extension.", nameof(filePath));
            }
        }

        public static Stream ToStream(Bitmap bitmap)
        {
            var memory = new MemoryStream();

            bitmap.Save(memory, ImageFormat.Bmp);

            memory.Position = 0;
            return memory;
        }
    }
}
