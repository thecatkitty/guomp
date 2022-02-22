using Portable.Xaml.Markup;
using SkiaSharp;

namespace Celones.Guomp.Controls
{
    public class Control
    {
        public virtual void OnRender(SKCanvas canvas) { }
    }

    [ContentProperty("Content")]
    public class ContentControl : Control
    {
        public object Content { get; set; }
    }

    [ContentProperty("Items")]
    public class ItemsControl : Control
    {
        public IList<Control> Items { get; set; }
    }

    public interface IHeaderedControl
    {
        string Header { get; set; }
    }
}
