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
            Display.Canvas.Clear(SKColors.Empty);

            var paint = new SKPaint
            {
                TextAlign = SKTextAlign.Center
            };

            var font = new SKFont
            {
                Typeface = SKTypeface.FromFamilyName("Tahoma"),
                Edging = SKFontEdging.Alias,
                Size = 8
            };
            
            Display.Canvas.DrawText(Root.Header, Display.Width / 2, -2, font, paint);

            foreach (var item in Root.Items)
            {
                if (item is Menu menu)
                {
                    if (menu.Items.Count > 0)
                    {
                        menu.SelectedItem = (MenuItem)menu.Items[0];
                    }
                }
            }

            foreach (var item in Root.Items)
            {
                item.OnRender(Display.Canvas);
            }
            Display.Update();
        }
    }
}
