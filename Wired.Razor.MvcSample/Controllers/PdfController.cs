using System.Collections.Generic;
using System.Web.Mvc;
using iTextSharp.text;
using Wired.Razor.MvcSample.Models;
using Wired.RazorPdf;
using Wired.RazorPdf.Snippets;
using Header = Wired.RazorPdf.Snippets.Header;

namespace Wired.Razor.MvcSample.Controllers
{
    public class PdfController : Controller
    {
        public ActionResult PdfWithoutLayout()
        {
            var generator = new MvcGenerator(ControllerContext, Server.MapPath("~"));

            var model = new FooterModel
            {
                Name = "Unicorns Rule"
            };

            var helper = new Footer(model, "~/Views/Pdf/_Footer.cshtml", 50, 5, 5, 10);
            generator.AddPageEndEventHelper(helper);

            var pdf = generator.GeneratePdf(GetModel(), "PdfWithoutLayout");

            generator.AddPageEndEventHelper(helper);

            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult PdfWithLayout()
        {
            var generator = new MvcGenerator(ControllerContext, Server.MapPath("~"));

            var model = new FooterModel
            {
                Name = "Unicorns Rule"
            };

            var helper = new Footer(model, "~/Views/Pdf/_Footer.cshtml", 50, 5, 5, 10);
            generator.AddPageEndEventHelper(helper);

            var pdf = generator.GeneratePdf(GetModel(), "PdfWithLayout");

            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult ControllerlessPdfWithoutLayout()
        {
            var generator = new StandaloneGenerator(new Parser(), Server.MapPath("~"));

            var model = new FooterModel
            {
                Name = "Unicorns Rule"
            };

            var helper = new Footer(model, Server.MapPath("~/Views/Pdf/_Footer.cshtml"), 50, 5, 5, 10);
            generator.AddPageEndEventHelper(helper);

            var pdf = generator.GeneratePdf(GetModel(), Server.MapPath("~/Views/Pdf/ControllerlessPdfWithoutLayout.cshtml"));
            
            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult LandscapeControllerlessPdfWithoutLayout()
        {
            var generator = new StandaloneGenerator(new Parser(), Server.MapPath("~"));

            var footerModel = new FooterModel
            {
                Name = "Unicorns Rule"
            };

            var headerModel = new HeaderModel
            {
                Name = "Unicorns Rule"
            };

            var footerHelper = new Footer(footerModel, Server.MapPath("~/Views/Pdf/_Footer.cshtml"), 50, 5, 5, 10);
            var headerHelper = new Header(headerModel, Server.MapPath("~/Views/Pdf/_Header.cshtml"), 50, 5, 5, 10);
            generator.AddPageEndEventHelper(footerHelper);
            generator.AddPageEndEventHelper(headerHelper);

            var pdf = generator.GeneratePdf(
                (writer, document) =>
                {
                    document.SetPageSize(PageSize.A4.Rotate());
                }, 
                GetModel(), 
                Server.MapPath("~/Views/Pdf/ControllerlessPdfWithoutLayout.cshtml"));

            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult ControllerlessPdfWithLayout()
        {
            var generator = new StandaloneGenerator(new Parser(), Server.MapPath("~"))
            {
                Templates = new List<Template>
                {
                    new Template
                    {
                        Name = "~/Views/Shared/_PdfLayout.cshtml",
                        Source = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/_PdfLayout.cshtml"))
                    }
                },
                Margins = new Margins(10, 10, 100, 10)
            };

            var footerModel = new FooterModel
            {
                Name = "Unicorns Rule"
            };

            var headerModel = new HeaderModel
            {
                Name = "Unicorns Rule"
            };

            var footerHelper = new Footer(footerModel, Server.MapPath("~/Views/Pdf/_Footer.cshtml"), 80, 0, 0, 0);
            var headerHelper = new Header(headerModel, Server.MapPath("~/Views/Pdf/_Header.cshtml"), 80, 0, 0, 0);
            generator.AddPageEndEventHelper(footerHelper);
            generator.AddPageEndEventHelper(headerHelper);

            var pdf = generator.GeneratePdf(GetModel(), Server.MapPath("~/Views/Pdf/ControllerlessPdfWithLayout.cshtml"));
            return new FileContentResult(pdf, "application/pdf");
        }

        private DemonstrationModel GetModel()
        {
            return new DemonstrationModel
            {
                People = new List<string> { "Alice", "Bob", "Charlie" }
            };
        }
    }
}