using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;


namespace Conto.Data.PdfHelpers
{
    public class ItextObjectExport
    {
        private static readonly BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        private static readonly BaseFont fontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);

        //ITC Avant Garde Gothic

        public static void CreateFirstPdf()
        {
            var _contoData = new ContoData();

            //FileStream fs = new FileStream("Chapter1_example.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //Document doc = new Document();
            //PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            using (FileStream fs = new FileStream("Chapter1_example.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document doc = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {

                doc.Open();

                PdfReader templateReader = new PdfReader("Autofattura_Template.pdf");
                PdfTemplate background = writer.GetImportedPage(templateReader, 1);

                doc.NewPage();
                var pcb = writer.DirectContentUnder;
                pcb.AddTemplate(background, 0, 0);

                //doc.Add(new Paragraph("Hello World"));

                pcb = writer.DirectContent;
                pcb.BeginText();

                //PrintTextCentered(pcb, "Autofattura", 280, 680);

                pcb.SetFontAndSize(font, 10);

                pcb.SetTextMatrix(469,650);
                pcb.ShowText(DateTime.Now.ToString("dd/MM/yyyy"));

                pcb.SetTextMatrix(469, 613);
                pcb.ShowText("6306/13");

                /* ANAGRAFICA CLIENTE AUTOFATTURA */
                SelfInvoiceOwner(pcb, _contoData.GetSettings());


                pcb.SetFontAndSize(font, 10);
                pcb.SetTextMatrix(68, 512);
                pcb.ShowText("Rottame rame");

                pcb.SetTextMatrix(68, 512);
                pcb.ShowText("Rottame rame");

                pcb.SetTextMatrix(285, 512);
                pcb.ShowText("0,18");

                pcb.SetTextMatrix(325, 512);
                pcb.ShowText("€ 4.720,00");

                pcb.SetTextMatrix(480, 512);
                pcb.ShowText("€ 849,60");

                pcb.SetTextMatrix(480, 200);
                pcb.ShowText("€ 849,60");

                pcb.SetFontAndSize(fontBold, 10);

                pcb.SetTextMatrix(480, 140);
                pcb.ShowText("€ 849,60");

                pcb.EndText();


                doc.Close();
                writer.Close();
                fs.Close();
            }
        }

        private static void SelfInvoiceOwner(PdfContentByte pcb, Settings owner)
        {
            pcb.SetTextMatrix(68, 650);
            pcb.ShowText("Nome:");
            pcb.SetFontAndSize(fontBold, 10);
            pcb.SetTextMatrix(100, 650);
            pcb.ShowText(owner.InvoiceOwnerName);

            pcb.SetFontAndSize(font, 10);
            pcb.SetTextMatrix(68, 639);
            pcb.ShowText("Indirizzo:");
            pcb.SetFontAndSize(fontBold, 10);
            pcb.SetTextMatrix(110, 639);
            pcb.ShowText(owner.InvoiceOwnerAddress);

            pcb.SetFontAndSize(font, 10);
            pcb.SetTextMatrix(68, 626);
            pcb.ShowText("Cap. Città:");
            pcb.SetFontAndSize(fontBold, 10);
            pcb.SetTextMatrix(117, 626);
            pcb.ShowText(string.Format("{0} {1}", owner.InvoiceOwnerPostalCode, owner.InvoiceOwnerAddress));

            pcb.SetFontAndSize(font, 10);
            pcb.SetTextMatrix(68, 613);
            pcb.ShowText("C.F:");
            pcb.SetFontAndSize(fontBold, 10);
            pcb.SetTextMatrix(95, 613);
            pcb.ShowText(owner.InvoiceOwnerFiscalCode);

            pcb.SetFontAndSize(font, 10);
            pcb.SetTextMatrix(68, 600);
            pcb.ShowText("P.Iva:");
            pcb.SetFontAndSize(fontBold, 10);
            pcb.SetTextMatrix(95, 600);
            pcb.ShowText(owner.InvoiceOwnerVatCode);
        }










        private static void PrintTextCentered(PdfContentByte pcb, string text, int x, int y)
        {
            pcb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, x, y, 0);
        }

        private static void SetFont7(PdfContentByte pcb)
        {
            pcb.SetFontAndSize(font, 7);
        }

        private static void SetFont18(PdfContentByte pcb)
        {
            pcb.SetFontAndSize(font, 18);
        }

        private static void SetFont36(PdfContentByte pcb)
        {
            pcb.SetFontAndSize(font, 36);
        }

        public static void CreateInMemoryPdf()
        {
            using (MemoryStream ms = new MemoryStream())
            using (Document document = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
            {
                document.Open();
                document.Add(new Paragraph("Hello World"));
                document.Close();
                writer.Close();
                ms.Close();
                //Response.ContentType = "pdf/application";
                //Response.AddHeader("content-disposition", "attachment;filename=First_PDF_document.pdf");
                //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
        }
    }
}
