using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
            var fileName = $"{now}.png";
            var fileName_scaled = $"{now}-Scaled.jpg";

            using (var bitmap = BitmapHelper.GetScreenBitmap(200, 100, 1200, 800))
            using (var scaled = BitmapHelper.ScaleImage(bitmap, 600, 400))
            {
                bitmap.Save(fileName, ImageFormat.Png);
                scaled.Save(fileName_scaled, ImageFormat.Jpeg);
            }
        }
    }
}
