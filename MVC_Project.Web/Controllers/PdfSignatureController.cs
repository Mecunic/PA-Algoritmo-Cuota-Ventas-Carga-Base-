using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.IO;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class PdfSignatureController : Controller
    {
        // GET: PdfSignature
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GeneratePdf(string signature)
        {
            //Create PDF Document
            Document document = new Document();

            //Define document styles
            Style style = document.Styles["Normal"];
            style.Font.Name = "sans-serif";
            style.Font.Size = 12;
            style.Font.Bold = false;

            style = document.Styles["Heading1"];
            style.Font.Size = 24;
            style.Font.Bold = true;
            style.ParagraphFormat.SpaceBefore = 0;
            style.ParagraphFormat.SpaceAfter = 15;

            style = document.Styles["Heading3"];
            style.Font.Size = 13.5;
            style.Font.Bold = true;
            style.ParagraphFormat.SpaceBefore = 13.5;
            style.ParagraphFormat.SpaceAfter = 13.5;

            //Create document section
            Section section = document.AddSection();

            //Add elements to section
            Paragraph paragraph = section.AddParagraph("Lorem Ipsum", "Heading1");
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut sagittis nulla tempus mi sodales ultricies. " +
                "Nulla hendrerit, justo quis ultricies pellentesque, est urna tincidunt nunc, vel faucibus ante leo id metus. Duis tempus " +
                "eleifend venenatis. Suspendisse faucibus nibh non est molestie, eu pretium ligula facilisis. Etiam laoreet venenatis eros " +
                "vitae dictum. Proin faucibus eget magna non accumsan. Proin cursus congue velit imperdiet tempor. Donec eu diam ac sem facilisis " +
                "tristique vitae et arcu. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.");
            paragraph.Format.SpaceAfter = "13cm";

            paragraph = section.AddParagraph("Firma", "Heading3");
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Format signature image data and add to section
            var encodedImage = signature.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1];
            var image = section.AddImage("base64:" + encodedImage);
            image.Width = "10cm";
            image.Left = "3cm";            

            //Render PDF document
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            //Download PDF file
            MemoryStream stream = new MemoryStream();
            pdfRenderer.PdfDocument.Save(stream, false);
            return File(stream.ToArray(), "application/pdf", "signed.pdf");

        }
    }
}