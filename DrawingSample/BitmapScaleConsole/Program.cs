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
                    using (var scaled = BitmapHelper.ScaleImage(bitmap, 540, 360, mode))
                    {
                        scaled.Save($@"{now}\{mode}.jpg", ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}
