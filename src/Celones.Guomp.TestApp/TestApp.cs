using SkiaSharp;

var pcd = new Celones.Device.Pcd8544(null, null, 0, 0);
var display = new Celones.Guomp.Display.Nokia5110(pcd, null);

var paint = new SKPaint
{
    IsAntialias = false
};

var font = new SKFont
{
    Typeface = SKTypeface.FromFamilyName("Tahoma", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
    Edging = SKFontEdging.Alias,
    Size = 10
};

display.Clear();
display.Canvas.DrawLine(new SKPoint(5, 5), new SKPoint(60, 20), paint);
display.Canvas.DrawText("Hello World!", 5, 30, font, paint);

display.Capture("capture.png");
