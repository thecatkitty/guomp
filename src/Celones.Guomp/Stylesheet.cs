using SkiaSharp;

namespace Celones.Guomp
{
    internal class Stylesheet
    {
        public string FontFamily { get; set; } = "Tahoma";
        public SKFontStyleWeight FontWeight { get; set; } = SKFontStyleWeight.Bold;
        public SKFontStyleWidth FontWidth { get; set; } = SKFontStyleWidth.Normal;
        public SKFontStyleSlant FontSlant { get; set; } = SKFontStyleSlant.Upright;
        public int TextSize { get; set; } = 10;

        public string HeaderFontFamily { get; set; } = "Tahoma";
        public SKFontStyleWeight HeaderFontWeight { get; set; } = SKFontStyleWeight.Normal;
        public SKFontStyleWidth HeaderFontWidth { get; set; } = SKFontStyleWidth.Normal;
        public SKFontStyleSlant HeaderFontSlant { get; set; } = SKFontStyleSlant.Upright;
        public int HeaderTextSize { get; set; } = 10;

        public int HeaderPadding { get; set; } = 1;
        public int MenuItemPadding { get; set; } = 1;

        public SKFont GetFont() =>
            new(SKTypeface.FromFamilyName(FontFamily, FontWeight, FontWidth, FontSlant), TextSize)
            {
                Edging = SKFontEdging.Alias
            };

        public SKFont GetHeaderFont() =>
            new(SKTypeface.FromFamilyName(HeaderFontFamily, HeaderFontWeight, HeaderFontWidth, HeaderFontSlant), HeaderTextSize)
            {
                Edging = SKFontEdging.Alias
            };

        private static Stylesheet? s_default;
        public static Stylesheet Default => s_default ??= new Stylesheet();
    }
}
