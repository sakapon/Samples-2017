using System;
using System.Collections.Generic;
using System.Drawing;
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

            var dirName = $"{DateTime.Now:yyyyMMdd-HHmmss}";
            Directory.CreateDirectory(dirName);

            using (var bitmap = BitmapHelper.GetScreenBitmap(200, 100, 1080, 720))
            {
                bitmap.Save($@"{dirName}\Original.png", ImageFormat.Png);
                bitmap.Save($@"{dirName}\Original.jpg", ImageFormat.Jpeg);

                ResizeImageTest_Drawing(dirName, bitmap);
                ResizeImageTest_Windows(dirName, bitmap);
            }
        }

        static void ResizeImageTest_Drawing(string dirName, Bitmap bitmap)
        {
            var modes = Enum.GetValues(typeof(InterpolationMode))
                .Cast<InterpolationMode>()
                .Where(m => m != InterpolationMode.Invalid);
            foreach (var mode in modes)
            {
                using (var resized = BitmapHelper.ResizeImage(bitmap, bitmap.Width / 2, bitmap.Height / 2, mode))
                {
                    resized.Save($@"{dirName}\{mode}.jpg", ImageFormat.Jpeg);
                }
            }
        }

        static void ResizeImageTest_Windows(string dirName, Bitmap bitmap)
        {
            using (var memory = BitmapHelper.ToStream(bitmap))
            {
                var source = System.Windows.Media.Imaging.BitmapFrame.Create(memory);
                var resized = BitmapHelper2.ResizeImage(source, bitmap.Width / 2, bitmap.Height / 2);

                BitmapHelper2.SaveImage($@"{dirName}\WPF.png", resized);
                BitmapHelper2.SaveImage($@"{dirName}\WPF.jpg", resized);
            }
        }
    }
}
