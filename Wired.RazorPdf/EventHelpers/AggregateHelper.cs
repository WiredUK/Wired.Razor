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
        private readonly List<BasePageSnippet> _pageEndEventHelpers;
        private readonly Func<Action<PdfWriter, Document>, ISnippetModel, string, List<BasePageSnippet>, Margins, byte[]> _pdfGeneratorFunc;

        private int _currentPage;

        public AggregateHelper(List<BasePageSnippet> pageEventHelpers, Func<Action<PdfWriter, Document>, ISnippetModel, string, List<BasePageSnippet>, Margins, byte[]> pdfGeneratorFunc)
        {
            _pageEndEventHelpers = pageEventHelpers;
            _pdfGeneratorFunc = pdfGeneratorFunc;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            _currentPage++;

            foreach (var helper in _pageEndEventHelpers)
            {
                helper.Model.PageNumber = _currentPage;

                var snippet = _pdfGeneratorFunc(null, helper.Model, helper.View, null, helper.Margins);

                //Insert the newly generated PDF onto the page
                var helperTemplate = writer.GetImportedPage(new PdfReader(snippet), 1);

                var yPosition = helper.HelperAlignment == HelperAlignment.Bottom
                    ? 0F
                    : document.PageSize.Height - helperTemplate.Height;

                writer.DirectContent.AddTemplate(helperTemplate, 0, yPosition);
            }
            
            base.OnEndPage(writer, document);
        }
    }
}