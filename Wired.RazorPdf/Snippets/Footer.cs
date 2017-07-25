using iTextSharp.text;
using Wired.RazorPdf.Enums;
using Wired.RazorPdf.Models;

namespace Wired.RazorPdf.Snippets
{
    public class Footer : BasePageSnippet
    {
        public Footer(ISnippetModel model, string view, float height, float leftMargin, float rightMargin, float bottomMargin)
            : base(model, view, new Margins(new Rectangle(PageSize.A4.Width, height), leftMargin, rightMargin, 0, bottomMargin), SnippetAlignment.Bottom)
        {
        }
    }
}