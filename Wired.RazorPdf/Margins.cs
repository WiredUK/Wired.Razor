using iTextSharp.text;

namespace Wired.RazorPdf
{
    public class Margins
    {
        public Margins(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public Margins(Rectangle pageSize, float left, float right, float top, float bottom)
        {
            PageSize = pageSize;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }

        public Rectangle PageSize { get; set; } = iTextSharp.text.PageSize.A4;
    }
}