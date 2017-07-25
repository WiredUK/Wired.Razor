using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using Wired.RazorPdf.EventHelpers;
using Wired.RazorPdf.Snippets;

namespace Wired.RazorPdf
{
    public class MvcGenerator : IGenerator
    {
        public ControllerContext ControllerContext;
        public string ImageBasePath { get; set; }
        public Margins Margins;

        private List<BasePageSnippet> _pageSnippets;

        public MvcGenerator(ControllerContext controllerContext)
        {
            ControllerContext = controllerContext;
        }

        public MvcGenerator(ControllerContext controllerContext, string imageBasePath, Margins margins)
        {
            ControllerContext = controllerContext;
            ImageBasePath = imageBasePath;
            Margins = margins;
        }

        public MvcGenerator(ControllerContext controllerContext, string imageBasePath)
        {
            ControllerContext = controllerContext;
            ImageBasePath = imageBasePath;
        }

        public MvcGenerator(ControllerContext controllerContext, Margins margins)
        {
            ControllerContext = controllerContext;
            Margins = margins;
        }

        public void AddPageSnippet(BasePageSnippet snippet)
        {
            if (_pageSnippets == null)
            {
                _pageSnippets = new List<BasePageSnippet>();
            }

            _pageSnippets.Add(snippet);
        }

        public byte[] GeneratePdf<T>(T model = null, string viewName = null) where T : class
        {
            return InternalGeneratePdf(null, model, viewName, _pageSnippets, Margins);
        }

        public byte[] GeneratePdf<T>(Action<PdfWriter, Document> configureSettings, T model = null, string viewName = null) where T : class
        {
            return InternalGeneratePdf(configureSettings, model, viewName, _pageSnippets, Margins);
        }

        private byte[] InternalGeneratePdf<T>(Action<PdfWriter, Document> configureSettings, T model = null,
            string viewName = null, List<BasePageSnippet> pageSnippets = null, Margins margins = null)
            where T : class
        {
            ControllerContext.Controller.ViewData.Model = model;

            byte[] output;

            var document = margins == null
                ? new Document()
                : new Document(margins.PageSize, margins.Left, margins.Right, margins.Top, margins.Bottom);

            using (document)
            {
                using (var workStream = new MemoryStream())
                {
                    var writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    if (pageSnippets != null)
                    {
                        var aggregateHelper = new AggregateHelper(pageSnippets, InternalGeneratePdf);
                        writer.PageEvent = aggregateHelper;
                    }

                    configureSettings?.Invoke(writer, document);
                    document.Open();

                    using (var reader = new StringReader(RenderRazorView(viewName)))
                    {
                        var workerInstance = XMLWorkerHelper.GetInstance();

                        var tagProcessors = Tags.GetHtmlTagProcessorFactory();
                        if (!string.IsNullOrEmpty(ImageBasePath))
                        {
                            tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                            tagProcessors.AddProcessor(new ImageTagProcessor(ImageBasePath), new[] { HTML.Tag.IMG });
                        }

                        var htmlContext = new HtmlPipelineContext(null);

                        htmlContext.SetTagFactory(tagProcessors);

                        var cssResolver = workerInstance.GetDefaultCssResolver(true);
                        var pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));

                        var worker = new XMLWorker(pipeline, true);
                        var parser = new XMLParser(worker);
                        parser.Parse(reader);

                        document.Close();
                        output = workStream.ToArray();
                    }
                }
            }

            return output;
        }

        private string RenderRazorView(string viewName)
        {
            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null).View;
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                var viewContext = new ViewContext(ControllerContext, viewEngineResult, ControllerContext.Controller.ViewData,
                    ControllerContext.Controller.TempData, writer);
                viewEngineResult.Render(viewContext, writer);
            }
            return sb.ToString();
        }
    }
}