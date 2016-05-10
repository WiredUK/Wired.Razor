using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;

namespace Wired.RazorPdf
{
    public class ImageTagProcessor : iTextSharp.tool.xml.html.Image
    {
        private readonly string _imageBasePath;

        public ImageTagProcessor(string imageBasePath)
        {
            _imageBasePath = imageBasePath;
        }

        public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
        {
            var attributes = tag.Attributes;
            string src;
            if (!attributes.TryGetValue(HTML.Attribute.SRC, out src))
                return new List<IElement>(1);

            if (string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            attributes[HTML.Attribute.SRC] = _imageBasePath + src.Replace('/', '\\');

            return base.End(ctx, tag, currentContent);
        }
    }
}