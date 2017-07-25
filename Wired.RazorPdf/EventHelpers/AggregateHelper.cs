using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Wired.RazorPdf.Enums;
using Wired.RazorPdf.Models;
using Wired.RazorPdf.Snippets;

namespace Wired.RazorPdf.EventHelpers
{
    internal class AggregateHelper : PdfPageEventHelper
    {
        private readonly List<BasePageSnippet> _pageSnippets;
        private readonly Func<Action<PdfWriter, Document>, ISnippetModel, string, List<BasePageSnippet>, Margins, byte[]> _pdfGeneratorFunc;

        private int _currentPage;

        public AggregateHelper(List<BasePageSnippet> pageSnippets, Func<Action<PdfWriter, Document>, ISnippetModel, string, List<BasePageSnippet>, Margins, byte[]> pdfGeneratorFunc)
        {
            _pageSnippets = pageSnippets;
            _pdfGeneratorFunc = pdfGeneratorFunc;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            _currentPage++;

            foreach (var snippet in _pageSnippets)
            {
                snippet.Model.PageNumber = _currentPage;

                var snippetData = _pdfGeneratorFunc(null, snippet.Model, snippet.View, null, snippet.Margins);

                //Insert the newly generated PDF onto the page
                var snippetTemplate = writer.GetImportedPage(new PdfReader(snippetData), 1);

                var yPosition = snippet.SnippetAlignment == SnippetAlignment.Bottom
                    ? 0F
                    : document.PageSize.Height - snippetTemplate.Height;

                writer.DirectContent.AddTemplate(snippetTemplate, 0, yPosition);
            }
            
            base.OnEndPage(writer, document);
        }
    }
}