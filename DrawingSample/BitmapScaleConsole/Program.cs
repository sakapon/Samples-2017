using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BitmapScaleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press [Enter] key to start.");
            Console.ReadLine();

            ResizeImageTest_Drawing();
            ResizeImageTest_Windows();
        }

        static void ResizeImageTest_Drawing()
        {
            var now = $"{DateTime.Now:yyyyMMdd-HHmmss}";
            Directory.CreateDirectory(now);

            using (var bitmap = BitmapHelper.GetScreenBitmap(200, 100, 1080, 720))
            {
                bitmap.Save($@"{now}\Original.png", ImageFormat.Png);
                bitmap.Save($@"{now}\Original.jpg", ImageFormat.Jpeg);

                var modes = Enum.GetValues(typeof(InterpolationMode))
                    .Cast<InterpolationMode>()
                    .Where(m => m != InterpolationMode.Invalid);
                foreach (var mode in modes)
                {
                    using (var resized = BitmapHelper.ResizeImage(bitmap, bitmap.Width / 2, bitmap.Height / 2, mode))
                    {
                        resized.Save($@"{now}\{mode}.jpg", ImageFormat.Jpeg);
                    }
                }
            }
        }

        static void ResizeImageTest_Windows()
        {
            var now = $"{DateTime.Now:yyyyMMdd-HHmmss}";
            Directory.CreateDirectory(now);

            using (var bitmap = BitmapHelper.GetScreenBitmap(200, 100, 1080, 720))
            using (var memory = BitmapHelper.ToStream(bitmap))
            {
                bitmap.Save($@"{now}\Original.png", ImageFormat.Png);
                bitmap.Save($@"{now}\Original.jpg", ImageFormat.Jpeg);

                var source = System.Windows.Media.Imaging.BitmapFrame.Create(memory);
                var resized = BitmapHelper2.ResizeImage(source, bitmap.Width / 2, bitmap.Height / 2);

                BitmapHelper2.SaveImage($@"{now}\Resized.png", resized);
                BitmapHelper2.SaveImage($@"{now}\Resized.jpg", resized);
            }
        }
    }
}
