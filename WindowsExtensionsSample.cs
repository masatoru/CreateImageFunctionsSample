using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace Company.Function
{
    public class WindowsExtensionsSample
    {
        [FunctionName("WindowsExtensionsSample")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new FileContentResult(ImageToByteArray(CreateImage()), "image/jpeg");
        }
        Bitmap CreateImage()
        {
            Bitmap canvas = new(500, 200);
            var g = Graphics.FromImage(canvas);
            Font fnt = new("MS UI Gothic", 50);
            g.DrawString("こんにちは .NET6", fnt, Brushes.Blue, 10, 50);
            g.DrawRectangle(new Pen(Color.DarkOrange,10), 
                new Rectangle(0, 0, canvas.Width, canvas.Height));
            return canvas;
        }

        byte[] ImageToByteArray(Image image)
        {
            ImageConverter converter = new();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
    }
}
