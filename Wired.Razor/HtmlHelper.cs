using RazorEngine.Text;

namespace Wired.Razor
{
    public class HtmlHelper
    {
        public IEncodedString Raw(string rawString) => new RawString(rawString);
    }
}