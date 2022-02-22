using SkiaSharp;

namespace Celones.Guomp.Extensions
{
    public static class SkiaExtensions
    {
        public static float GetLightness(this SKColor color)
        {
            color.ToHsl(out _, out _, out var lightness);
            return lightness;
        }

        public static bool GetMonochromePixel(this SKPixmap pixels, int x, int y) => pixels.GetPixelColor(x, y).GetLightness() > 0.5;
    }
}
