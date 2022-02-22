using SkiaSharp;

namespace Celones.Guomp.Extensions
{
    public static class SKPixmapExtensions
    {
        public static bool GetMonochromePixel(this SKPixmap pixmap, int x, int y) => pixmap.GetPixelColor(x, y).GetLightness() > 0.5;
    }
}
