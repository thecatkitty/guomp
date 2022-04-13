using Portable.Xaml.Markup;
using SkiaSharp;

namespace Celones.Guomp.Controls
{
    public class Menu : ItemsControl
    {
        private int m_selectedIndex;

        public Menu()
        {
            Items = new List<Control>();
        }

        public override void OnRender(SKCanvas canvas)
        {
            var bounds = canvas.DeviceClipBounds;

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
                var itemBounds = new SKRect(bounds.Left, bounds.Top + i * 12, bounds.Right, bounds.Top + i * 12 + 12);
                paint.Color = SKColors.Black;

                if (i == m_selectedIndex)
                {
                    canvas.DrawRect(itemBounds, paint);
                }
                
                var menuItem = (MenuItem)Items[i];
                canvas.DrawText(menuItem.Header, itemBounds.Left + 1, itemBounds.Top + 9, font, paint);
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
