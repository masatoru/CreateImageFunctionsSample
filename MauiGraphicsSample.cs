using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace Company.Function
{
    public class MauiGraphicsSample
    {
        [FunctionName("MauiGraphics")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            BitmapExportContext bmpContext = new SkiaBitmapExportContext(500, 200, 1.0f);

            SizeF bmpSize = new(bmpContext.Width, bmpContext.Height);
            var canvas = bmpContext.Canvas;

            // Draw on the canvas with abstract methods that are agnostic to the renderer
            ClearBackground(canvas, bmpSize, Colors.Navy);
            // DrawRandomLines(canvas, bmpSize, 1000);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.DarkOrange;

            canvas.DrawRectangle(0, 0, bmpContext.Width, bmpContext.Height);

            DrawBigTextWithShadow(canvas, "こんにちは .NET6 Maui Graphics");

            return new FileContentResult(ImageToByteArray(bmpContext), "image/jpeg");
        }

        static void ClearBackground(ICanvas canvas, SizeF bmpSize, Color bgColor)
        {
            canvas.FillColor = Colors.Navy;
            canvas.FillRectangle(0, 0, bmpSize.Width, bmpSize.Height);
        }

        static void DrawBigTextWithShadow(ICanvas canvas, string text)
        {
            canvas.Font = new Font("MS UI Gothic", 50);
            canvas.FontSize = 30;
            canvas.FontColor = Colors.White;
            // canvas.SetShadow(offset: new SizeF(2, 2), blur: 1, color: Colors.Black);
            canvas.DrawString(text, 10, 50, HorizontalAlignment.Left);
        }

        byte[] ImageToByteArray(BitmapExportContext bmp)
        {
            MemoryStream ms = new();
            bmp.WriteToStream(ms);
            return ms.ToArray();
        }
    }
}
