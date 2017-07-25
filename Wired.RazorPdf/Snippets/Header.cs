using iTextSharp.text;
using Wired.RazorPdf.Enums;
using Wired.RazorPdf.Models;

namespace Wired.RazorPdf.Snippets
{
    public class Header : BasePageSnippet
    {
        public Header(ISnippetModel model, string view, float height, float leftMargin, float rightMargin, float topMargin)
            : base(model, view, new Margins(new Rectangle(PageSize.A4.Width, height), leftMargin, rightMargin, topMargin, 0), SnippetAlignment.Top)
        {
        }
    }
}