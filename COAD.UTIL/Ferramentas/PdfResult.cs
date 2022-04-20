using System.IO;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;

namespace COAD.UTIL.Ferramentas
{
    public class PdfResult : ActionResult
    {

        public PdfResult()
        {
       
        }
        public PdfResult(string texto) 
        {
            this.texto = texto;
        }
        public PdfResult(string texto, string css)
        {
            this.texto = texto;
            this.css = css;
        }
        public string texto { get; set; }
        public string css {get; set;}
        public override void ExecuteResult(ControllerContext context)
        {
            HtmlToPdfBuilder builder = new HtmlToPdfBuilder(PageSize.LETTER);

            builder.ImportStylesheet(this.css + "\\Content\\themes\\base\\relatorio.css");
          
            HtmlPdfPage page1 = builder.AddPage();
            page1.AppendHtml(this.texto);
            
            byte[] file = builder.RenderPdf();
            byte[] buffer = new byte[4096];

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/pdf";

            MemoryStream pdfStream = new MemoryStream(file);

            while (true)
            {
                int read = pdfStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;
                response.OutputStream.Write(buffer, 0, read);
            }

            response.End();
        }
        public void Execute(HttpResponseBase context)
        {
            HtmlToPdfBuilder builder = new HtmlToPdfBuilder(PageSize.LETTER);

            builder.ImportStylesheet(this.css + "\\Content\\themes\\base\\relatorio.css");

            HtmlPdfPage page1 = builder.AddPage();
            page1.AppendHtml(this.texto);

            byte[] file = builder.RenderPdf();
            byte[] buffer = new byte[4096];

            HttpResponseBase response = context;
            response.ContentType = "application/pdf";

            MemoryStream pdfStream = new MemoryStream(file);

            while (true)
            {
                int read = pdfStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;
                response.OutputStream.Write(buffer, 0, read);
            }

            response.End();
        }

    }
}