using Portable.Xaml.Markup;
using SkiaSharp;

namespace Celones.Guomp.Controls
{
    public class Menu : ItemsControl
    {
        public Menu()
        {
            Items = new List<Control>();
        }

        public override void OnRender(SKCanvas canvas)
        {
            var paint = new SKPaint();
            var font = new SKFont
            {
                Typeface = SKTypeface.FromFamilyName("Tahoma"),
                Edging = SKFontEdging.Alias,
                Size = 8
            };
            
            var i = 1;
            foreach (var item in Items)
            {
                paint.Color = SKColors.Black;

                if (item == SelectedItem)
                {
                    paint.Style = SKPaintStyle.Fill;
                    canvas.DrawRect(new SKRect(0, i * 12, 84, 12), paint);
                    paint.Color = SKColor.Empty;
                }

                paint.Style = SKPaintStyle.Stroke;
                var menuItem = (MenuItem)item;
                canvas.DrawText(menuItem.Header, 0, i * 12 - 2, font, paint);
                i++;
            }
        }

        public MenuItem SelectedItem { get; set; }
    }

    [ContentProperty("Header")]
    public class MenuItem : ItemsControl, IHeaderedControl
    {
        public MenuItem()
        {
            Items = new List<Control>();
        }

        public System.Windows.Input.ICommand Command { get; set; }
        public bool IsCheckable { get; set; }
        public string Header { get; set; }

        public event EventHandler Checked;
        public event EventHandler Unchecked;
        public event EventHandler Click;
    }
}
