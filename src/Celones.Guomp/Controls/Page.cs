namespace Celones.Guomp.Controls
{
    public class Page : ItemsControl, IHeaderedControl
    {
        public Page()
        {
            Items = new List<Control>();
            Header = GetType().Name;
        }

        public string Header { get; set; }
    }
}
