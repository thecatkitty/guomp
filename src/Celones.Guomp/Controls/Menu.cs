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
            var font = Stylesheet.Default.GetFont();
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                BlendMode = SKBlendMode.Xor
            };

            var itemHeight = Stylesheet.Default.TextSize + 2 * Stylesheet.Default.MenuItemPadding;
            for (var i = 0; i < Items.Count; i++)
            {
                var itemBounds = new SKRect(
                    left: bounds.Left,
                    top: bounds.Top + i * itemHeight,
                    right: bounds.Right,
                    bottom: bounds.Top + (i + 1) * itemHeight);

                if (i == m_selectedIndex)
                {
                    canvas.DrawRect(itemBounds, paint);
                }
                
                var menuItem = (MenuItem)Items[i];
                canvas.DrawText(
                    text: menuItem.Header,
                    x: itemBounds.Left + Stylesheet.Default.MenuItemPadding,
                    y: itemBounds.Top + Stylesheet.Default.TextSize - 1,
                    font: font,
                    paint: paint);
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
