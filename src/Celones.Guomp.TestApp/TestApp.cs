using SkiaSharp;

Celones.Device.Pcd8544 pcd = new(null, null, 0, 0);
Celones.Guomp.Display.Nokia5110 display = new(pcd, null);

var paint = new SKPaint
{
    IsAntialias = false,
    IsAutohinted = true
};

display.Clear();
display.Canvas.DrawLine(new SKPoint(5, 5), new SKPoint(60, 20), paint);
display.Canvas.DrawText("Lorem Ipsum", new SKPoint(5, 30), paint);

display.SaveScreenshot("screenshot.png");
