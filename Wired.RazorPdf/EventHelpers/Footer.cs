using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Wired.RazorPdf.Models;

namespace Wired.RazorPdf.EventHelpers
{
    public abstract class BaseEventHelper : PdfPageEventHelper
    {
        private readonly string _view;
        private readonly Margins _margins;
        private readonly IHelperModel _model;

        private int _currentPage;

        internal Func<Action<PdfWriter, Document>, IHelperModel, string, List<BaseEventHelper>, Margins, byte[]> PdfGeneratorFunc;

        internal BaseEventHelper(IHelperModel model, string view, Margins margins)
        {
            _model = model;
            _view = view;

            _margins = margins;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            _currentPage++;

            _model.PageNumber = _currentPage;
            
            var snippet = PdfGeneratorFunc(null, _model, _view, null, _margins);

            //Insert the newly generated PDF onto the page
            var footer = writer.GetImportedPage(new PdfReader(snippet), 1);
            writer.DirectContent.AddTemplate(footer, 0, 0);

            base.OnEndPage(writer, document);
        }
    }

    public class Footer : BaseEventHelper
    {
        public Footer(IHelperModel model, string view, float height, float leftMargin, float rightMargin, float bottomMargin)
            : base(model, view, new Margins(new Rectangle(PageSize.A4.Width, height), leftMargin, rightMargin, 0, bottomMargin))
        {
        }
    }
}