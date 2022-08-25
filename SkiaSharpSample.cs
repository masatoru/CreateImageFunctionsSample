using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Company.Function
{
    public class SkiaSharpSample
    {
        [FunctionName("SkiaSharp")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new FileContentResult(GetCaptcha(), "image/jpeg");
        }
        internal static byte[] GetCaptcha()
        {
            SKPaint drawStyle = new();
            drawStyle.TextSize = 30;
            drawStyle.Typeface = SKTypeface.FromFamilyName("MS UI Gothic");
            // drawStyle.Typeface = SKTypeface.FromFamilyName("IPAexMincho");
            drawStyle.Color = SKColors.Blue;

            var compensateDeepCharacters = (int)drawStyle.TextSize / 5;
            SKSize size = new (500, 200);
            var image2d_x = (int)size.Width + 10;
            var image2d_y = (int)size.Height + 10 + compensateDeepCharacters;

            using SKBitmap image2d = new(image2d_x, image2d_y, SKColorType.Bgra8888, SKAlphaType.Premul);
            using SKCanvas canvas = new(image2d);

            canvas.Clear(new SkiaSharp.SKColor(255, 255, 255, 255));
            canvas.DrawText("こんにちは .NET6 SkiaSharp", 10, 50, drawStyle);
            using SKImage img = SKImage.FromBitmap(image2d);
            using SKData p = img.Encode(SKEncodedImageFormat.Jpeg, 100);
            var imageBytes = p.ToArray();
            return imageBytes;
        }
    }
}