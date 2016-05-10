using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Wired.RazorPdf
{
    public interface IGenerator
    {
        byte[] GeneratePdf<T>(T model = null, string viewName = null)
            where T : class;

        byte[] GeneratePdf<T>(Action<PdfWriter, Document> configureSettings, T model = null, string viewName = null)
            where T : class;

    }
}