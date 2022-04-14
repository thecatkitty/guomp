using Celones.Guomp.Controls;
using SkiaSharp;

[assembly: Portable.Xaml.Markup.XmlnsDefinition("http://schemas.celones.pl/xaml/2019/onui", "Celones.Guomp.Controls")]

namespace Celones.Guomp
{
    public class Ui
    {
        public Display Display { get; }
        public Page Root { get; }

        public Ui(string file, Display display)
        {
            Display = display;
            Root = (Page)Portable.Xaml.XamlServices.Load(file);
        }

        public void Show()
        {
            Display.Clear();

            var font = new SKFont
            {
                Typeface = SKTypeface.FromFamilyName("Tahoma"),
                Edging = SKFontEdging.Alias,
                Size = 10
            };

            var paint = new SKPaint
            {
                TextAlign = SKTextAlign.Center,
                Typeface = font.Typeface,
                TextSize = font.Size
            };

            Display.Canvas.DrawText(Root.Header, Display.Width / 2, 8, font, paint);

            var barTop = Stylesheet.Default.TextSize / 2;
            var barWidth = (Display.Width - paint.MeasureText(Root.Header)) / 2 - 2;
            Display.Canvas.DrawLine(0, barTop, barWidth, barTop, paint);
            Display.Canvas.DrawLine(Display.Width - barWidth, barTop, Display.Width, barTop, paint);

            foreach (var item in Root.Items)
            {
                Display.Canvas.Save();
                Display.Canvas.ClipRect(new SKRect(
                    left: 0,
                    top: Stylesheet.Default.TextSize + 2 * Stylesheet.Default.MenuItemPadding,
                    right: Display.Width,
                    bottom: Display.Height));
                item.OnRender(Display.Canvas);
                Display.Canvas.Restore();
            }
            Display.Update();
        }
    }
}
