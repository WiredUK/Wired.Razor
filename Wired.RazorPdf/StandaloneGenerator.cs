using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using Wired.Razor;

namespace Wired.RazorPdf
{
    public class StandaloneGenerator : IGenerator
    {
        public IParser Parser;
        public string ImageBasePath;
        public Margins Margins;

        public StandaloneGenerator(IParser parser, string imageBasePath)
        {
            Parser = parser;
            ImageBasePath = imageBasePath;
        }

        public StandaloneGenerator(IParser parser, string imageBasePath, Margins margins)
        {
            Parser = parser;
            ImageBasePath = imageBasePath;
            Margins = margins;
        }

        public IEnumerable<Template> Templates { get; set; }

        public byte[] GeneratePdf<T>(T model = null, string viewName = null) where T : class
        {
            return GeneratePdf(null, model, viewName);
        }

        public byte[] GeneratePdf<T>(Action<PdfWriter, Document> configureSettings, T model = null, string viewName = null) where T : class
        {
            byte[] output;

            var document = Margins == null
                ? new Document()
                : new Document(Margins.PageSize, Margins.Left, Margins.Right, Margins.Top, Margins.Bottom);

            using (document)
            {
                using (var workStream = new MemoryStream())
                {
                    var writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    configureSettings?.Invoke(writer, document);
                    document.Open();

                    var renderedView = Parser.RenderView(viewName, model, Templates);

                    using (var reader = new StringReader(renderedView))
                    {
                        var workerInstance = XMLWorkerHelper.GetInstance();
                        
                        var tagProcessors = Tags.GetHtmlTagProcessorFactory();
                        if (!string.IsNullOrEmpty(ImageBasePath))
                        {
                            tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                            tagProcessors.AddProcessor(new ImageTagProcessor(ImageBasePath), new[] {HTML.Tag.IMG});
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
    }
}