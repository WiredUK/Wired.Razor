using RazorEngine.Templating;

namespace Wired.Razor
{
    public abstract class HtmlSupportTemplateBase<T> : TemplateBase<T>
    {
        protected HtmlSupportTemplateBase()
        {
            Html = new HtmlHelper();
        }

        public HtmlHelper Html { get; set; }
    }
}