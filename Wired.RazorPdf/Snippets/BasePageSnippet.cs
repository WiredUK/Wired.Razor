using Wired.RazorPdf.Enums;
using Wired.RazorPdf.Models;

namespace Wired.RazorPdf.Snippets
{
    public abstract class BasePageSnippet
    {
        internal readonly string View;
        internal readonly Margins Margins;
        internal readonly SnippetAlignment SnippetAlignment;
        internal readonly ISnippetModel Model;

        internal BasePageSnippet(ISnippetModel model, string view, Margins margins, SnippetAlignment snippetAlignment)
        {
            Model = model;
            View = view;
            Margins = margins;
            SnippetAlignment = snippetAlignment;
        }
    }
}