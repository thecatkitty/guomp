using Portable.Xaml.Markup;
using SkiaSharp;

namespace Celones.Guomp.Controls
{
    public class Menu : ItemsControl
    {
        private int m_selectedIndex = 0;

        public Menu()
        {
            Items = new List<Control>();
        }

        public override void OnRender(SKCanvas canvas)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                BlendMode = SKBlendMode.Xor
            };

            var font = new SKFont
            {
                Typeface = SKTypeface.FromFamilyName("Tahoma", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
                Edging = SKFontEdging.Alias,
                Size = 10
            };

            for (var i = 0; i < Items.Count; i++)
            {
                paint.Color = SKColors.Black;

                if (i == m_selectedIndex)
                {
                    canvas.DrawRect(new SKRect(0, i * 12, 84, i * 12 + 12), paint);
                }
                
                var menuItem = (MenuItem)Items[i];
                canvas.DrawText(menuItem.Header, 1, i * 12 + 9, font, paint);
            }
        }

        public MenuItem SelectedItem
        {
            get => (MenuItem)Items[m_selectedIndex];
            set => m_selectedIndex = Items.IndexOf(value);
        }
    }

    [ContentProperty("Header")]
    public class MenuItem : ItemsControl, IHeaderedControl
    {
        public MenuItem()
        {
            Items = new List<Control>();
            Header = string.Empty;
        }

        public System.Windows.Input.ICommand Command { get; set; }
        public bool IsCheckable { get; set; }
        public string Header { get; set; }

        public event EventHandler Checked;
        public event EventHandler Unchecked;
        public event EventHandler Click;
    }
}
