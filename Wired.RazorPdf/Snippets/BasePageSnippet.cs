using Wired.RazorPdf.Enums;
using Wired.RazorPdf.Models;

namespace Wired.RazorPdf.Snippets
{
    public abstract class BasePageSnippet
    {
        internal readonly string View;
        internal readonly Margins Margins;
        internal readonly HelperAlignment HelperAlignment;
        internal readonly ISnippetModel Model;

        internal BasePageSnippet(ISnippetModel model, string view, Margins margins, HelperAlignment helperAlignment)
        {
            Model = model;
            View = view;
            Margins = margins;
            HelperAlignment = helperAlignment;
        }
    }
}