using System.Collections.Generic;
using System.Web.Mvc;
using Wired.Razor.MvcSample.Models;
using Wired.RazorPdf;

namespace Wired.Razor.MvcSample.Controllers
{
    public class PdfController : Controller
    {
        public ActionResult PdfWithoutLayout()
        {
            var generator = new MvcGenerator(ControllerContext, Server.MapPath("~"));
            var pdf = generator.GeneratePdf(GetModel(), "PdfWithoutLayout");
            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult PdfWithLayout()
        {
            var generator = new MvcGenerator(ControllerContext, Server.MapPath("~"));
            var pdf = generator.GeneratePdf(GetModel(), "PdfWithLayout");
            return new FileContentResult(pdf, "application/pdf");
        }

        public ActionResult ControllerlessPdfWithoutLayout()
        {
            var generator = new StandaloneGenerator(new Parser(), Server.MapPath("~"));
            var pdf = generator.GeneratePdf(GetModel(), Server.MapPath("~/Views/Pdf/ControllerlessPdfWithoutLayout.cshtml"));
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
                }
            };

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