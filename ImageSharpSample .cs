using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Company.Function
{
    public class ImageSharpSample
    {
        [FunctionName("ImageSharp")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new FileContentResult(ImageToByteArray(CreateImage()), "image/jpeg");
        }
        Image CreateImage()
        {
            Image img = new Image<Rgba32>(500, 200);
            Font fnt = SystemFonts.CreateFont("MS UI Gothic", 30);
            // Font fnt = SystemFonts.CreateFont("IPAexMincho", 50);

            TextOptions options = new(fnt)
            {
                Origin = new SixLabors.ImageSharp.PointF(10, 50), // Set the rendering origin.
                HorizontalAlignment = HorizontalAlignment.Left
            };

            PathBuilder pathBuilder = new();
            pathBuilder.SetOrigin(new PointF(0, 0));
            pathBuilder.AddLines(new PointF(0, 0), new PointF(500, 0), new PointF(500, 200), new PointF(0, 200), new PointF(0, 0));
            var path = pathBuilder.Build();

            IBrush brush = Brushes.Horizontal(Color.Blue, Color.Blue);
            IPen pen = SixLabors.ImageSharp.Drawing.Processing.Pens.Solid(Color.Blue, 1);

            img.Mutate(ctx => ctx
                .DrawText(options, "こんにちは .NET6 ImageSharp", brush, pen)
                .Draw(Color.DarkOrange, 5, path));

            return img;
        }

        byte[] ImageToByteArray(Image image)
        {
            MemoryStream ms = new();
            image.SaveAsJpeg(ms, new JpegEncoder() { Quality = 100 });
            return ms.ToArray();
        }
    }
}
