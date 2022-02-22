using SkiaSharp;

namespace Celones.Guomp.Extensions
{
    public static class SKColorExtensions
    {
        public static float GetLightness(this SKColor color)
        {
            color.ToHsl(out _, out _, out float lightness);
            return lightness;
        }
    }
}
