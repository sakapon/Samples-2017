using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitmapScaleConsole
{
    public static class BitmapHelper2
    {
        public static BitmapSource ResizeImage(ImageSource source, Size size) =>
            ResizeImage(source, (int)size.Width, (int)size.Height);

        public static BitmapSource ResizeImage(ImageSource source, int width, int height)
        {
            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                context.DrawImage(source, new Rect(0, 0, width, height));
            }

            var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);
            return bitmap;
        }

        public static void SaveImage(string filePath, BitmapSource bitmap)
        {
            var fullPath = Path.GetFullPath(filePath);

            var encoder = CreateBitmapEncoder(fullPath);
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = File.Create(fullPath))
            {
                encoder.Save(stream);
            }
        }

        static BitmapEncoder CreateBitmapEncoder(string filePath)
        {
            // To inherit BitmapEncoder class, internal SealObject method must be overrided.
            // So, we can not create IcoBitmapEncoder class which inherits BitmapEncoder class in custom libraries.
            switch (Path.GetExtension(filePath).ToLowerInvariant())
            {
                case ".bmp":
                    return new BmpBitmapEncoder();
                case ".gif":
                    return new GifBitmapEncoder();
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                    return new JpegBitmapEncoder();
                case ".png":
                    return new PngBitmapEncoder();
                case ".tiff":
                case ".tif":
                    return new TiffBitmapEncoder();
                case ".wdp":
                case ".hdp":
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentException("Can not encode bitmaps for the specified file extension.", "filePath");
            }
        }

        public static Stream ToStream(BitmapSource bitmap)
        {
            var memory = new MemoryStream();

            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(memory);

            memory.Position = 0;
            return memory;
        }
    }
}
