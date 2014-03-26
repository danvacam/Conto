using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Conto.Data.PdfHelpers
{
    public class ItextObjectExport
    {
        private static readonly BaseFont Font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        private static readonly BaseFont FontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);

        //ITC Avant Garde Gothic
        // Print with acrobat reader snippet
        // http://www.codeproject.com/Tips/598424/How-to-Silently-Print-PDFs-using-Adobe-Reader-and

        public static void ExportSelfInvoicesPdf(List<SelfInvoices> selfInvoices)
        {
            var contoData = new ContoData();

            using (var fs = new FileStream("Fatture.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            using (var doc = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();
                // get template and add to page
                var templateReader = new PdfReader(PdfResources.Autofattura_Template);
                PdfTemplate background = writer.GetImportedPage(templateReader, 1);
                foreach (var selfInvoice in selfInvoices)
                {
                    ExportPdfSelfInvoice(selfInvoice, contoData, doc, writer, background);
                }
                
                doc.Close();
                writer.Close();
                fs.Close();
            }
        }

        public static void ExportSelfInvoicesPdf(SelfInvoices selfInvoices)
        {
            var contoData = new ContoData();
            var fileName = string.Format("Fattura_{0}_{1}.pdf", selfInvoices.InvoiceNumber, selfInvoices.InvoiceYear);

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var doc = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();
                // get template and add to page
                var templateReader = new PdfReader(PdfResources.Autofattura_Template);
                PdfTemplate background = writer.GetImportedPage(templateReader, 1);

                ExportPdfSelfInvoice(selfInvoices, contoData, doc, writer, background);

                doc.Close();
                writer.Close();
                fs.Close();
            }

            string path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%");
            const string pathToExecutable = @"\Adobe\Adobe Reader\Reader\AcroRd32.exe";
            //RunExecutable(string.Concat(path, pathToExecutable), @"/p """ + fileName + "");

            RunExecutable(string.Concat(path, pathToExecutable), @"" + fileName + "");
        }

        private static void ExportPdfSelfInvoice(SelfInvoices selfInvoices, ContoData contoData, Document doc, PdfWriter writer,
            PdfTemplate background)
        {
            var material = contoData.MaterialGet(selfInvoices.MaterialId);
            doc.NewPage();
            var pcb = writer.DirectContentUnder;
            pcb.AddTemplate(background, 0, 0);

            pcb = writer.DirectContent;
            pcb.BeginText();
            pcb.SetFontAndSize(Font, 10);

            // invoice date
            pcb.SetTextMatrix(469, 650);
            pcb.ShowText(selfInvoices.InvoiceDate.ToString("dd/MM/yyyy"));
            // invoice number
            pcb.SetTextMatrix(469, 613);
            pcb.ShowText(string.Format("{0}/{1}", selfInvoices.InvoiceNumber,
                selfInvoices.InvoiceYear.ToString(CultureInfo.InvariantCulture).Substring(2)));
            // invoice owner
            SelfInvoiceOwner(pcb, contoData.GetSettings());
            // material description
            SelfInvoiceDescription(pcb, material.Description);


            SelfInvoiceQuantity(pcb, selfInvoices.Quantity);
            SelfInvoiceMaterialPrice(pcb, material.Price.HasValue ? material.Price.Value : 0);
            SelfInvoiceAmount(pcb, selfInvoices.InvoiceCost);

            TaxableAmount(pcb, selfInvoices.InvoiceCost);
            TotalAmount(pcb, selfInvoices.InvoiceCost);

            pcb.EndText();
        }


        public static void CreateFirstPdf()
        {
            var contoData = new ContoData();

            //FileStream fs = new FileStream("Chapter1_example.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //Document doc = new Document();
            //PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            using (FileStream fs = new FileStream("Chapter1_example.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document doc = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {

                doc.Open();

                PdfReader templateReader = new PdfReader(PdfResources.Autofattura_Template);
                PdfTemplate background = writer.GetImportedPage(templateReader, 1);

                doc.NewPage();
                var pcb = writer.DirectContentUnder;
                pcb.AddTemplate(background, 0, 0);

                pcb = writer.DirectContent;
                pcb.BeginText();

                pcb.SetFontAndSize(Font, 10);

                pcb.SetTextMatrix(469,650);
                pcb.ShowText(DateTime.Now.ToString("dd/MM/yyyy"));

                pcb.SetTextMatrix(469, 613);
                pcb.ShowText("6306/13");

                /* ANAGRAFICA CLIENTE AUTOFATTURA */
                SelfInvoiceOwner(pcb, contoData.GetSettings());

                SelfInvoiceDescription(pcb, "Rottame rame");
                SelfInvoiceQuantity(pcb, 0.18M);
                SelfInvoiceMaterialPrice(pcb, 4720.02M);
                SelfInvoiceAmount(pcb, 849.60M);

                TaxableAmount(pcb, 849.60M);
                TotalAmount(pcb, 849.60M);

                pcb.EndText();

                doc.Close();
                writer.Close();
                fs.Close();

                string path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%");
                string pathToExecutable = @"\Adobe\Adobe Reader\Reader\AcroRd32.exe";
                RunExecutable(string.Concat(path, pathToExecutable), @"/p ""Chapter1_example.pdf""");
            }
        }

        private static void SelfInvoiceOwner(PdfContentByte pcb, Settings owner)
        {
            pcb.SetTextMatrix(68, 650);
            pcb.ShowText("Nome:");
            pcb.SetFontAndSize(FontBold, 10);
            pcb.SetTextMatrix(100, 650);
            pcb.ShowText(owner.InvoiceOwnerName);

            pcb.SetFontAndSize(Font, 10);
            pcb.SetTextMatrix(68, 639);
            pcb.ShowText("Indirizzo:");
            pcb.SetFontAndSize(FontBold, 10);
            pcb.SetTextMatrix(110, 639);
            pcb.ShowText(owner.InvoiceOwnerAddress);

            pcb.SetFontAndSize(Font, 10);
            pcb.SetTextMatrix(68, 626);
            pcb.ShowText("Cap. Città:");
            pcb.SetFontAndSize(FontBold, 10);
            pcb.SetTextMatrix(117, 626);
            pcb.ShowText(string.Format("{0} {1}", owner.InvoiceOwnerPostalCode, owner.InvoiceOwnerAddress));

            pcb.SetFontAndSize(Font, 10);
            pcb.SetTextMatrix(68, 613);
            pcb.ShowText("C.F:");
            pcb.SetFontAndSize(FontBold, 10);
            pcb.SetTextMatrix(95, 613);
            pcb.ShowText(owner.InvoiceOwnerFiscalCode);

            pcb.SetFontAndSize(Font, 10);
            pcb.SetTextMatrix(68, 600);
            pcb.ShowText("P.Iva:");
            pcb.SetFontAndSize(FontBold, 10);
            pcb.SetTextMatrix(95, 600);
            pcb.ShowText(owner.InvoiceOwnerVatCode);
        }

        private static void SelfInvoiceDescription(PdfContentByte pcb, string description)
        {
            pcb.SetFontAndSize(Font, 10);
            pcb.SetTextMatrix(68, 512);
            pcb.ShowText(description);
        }

        public static void SelfInvoiceQuantity(PdfContentByte pcb, decimal quantity)
        {
            pcb.ShowTextAligned(Element.ALIGN_RIGHT,
                quantity.ToString("#,##0.00", CultureInfo.GetCultureInfo("it-IT")),
                304, 512, 0);

        }

        public static void SelfInvoiceMaterialPrice(PdfContentByte pcb, decimal price)
        {
            pcb.ShowTextAligned(Element.ALIGN_RIGHT, 
                string.Format("€ {0}", price.ToString("#,##0.00", CultureInfo.GetCultureInfo("it-IT"))),
                375, 512, 0);
        }

        public static void SelfInvoiceAmount(PdfContentByte pcb, decimal amount)
        {
            pcb.ShowTextAligned(Element.ALIGN_RIGHT,
                string.Format("€ {0}", amount.ToString("#,##0.00", CultureInfo.GetCultureInfo("it-IT"))),
                518, 512, 0);
        }

        public static void TaxableAmount(PdfContentByte pcb, decimal amount)
        {
            pcb.ShowTextAligned(Element.ALIGN_RIGHT,
                string.Format("€ {0}", amount.ToString("#,##0.00", CultureInfo.GetCultureInfo("it-IT"))),
                518, 200, 0);
        }

        public static void TotalAmount(PdfContentByte pcb, decimal amount)
        {
            pcb.SetFontAndSize(FontBold, 10);
            pcb.ShowTextAligned(Element.ALIGN_RIGHT,
                string.Format("€ {0}", amount.ToString("#,##0.00", CultureInfo.GetCultureInfo("it-IT"))),
                518, 140, 0);
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



        private static void RunExecutable(string executable, string arguments)
        {
            ProcessStartInfo starter = new ProcessStartInfo(executable, arguments);
            starter.CreateNoWindow = true;
            starter.RedirectStandardOutput = true;
            starter.UseShellExecute = false;
            Process process = new Process();
            process.StartInfo = starter;
            process.Start();
            //StringBuilder buffer = new StringBuilder();
            //using (StreamReader reader = process.StandardOutput)
            //{
            //    string line = reader.ReadLine();
            //    while (line != null)
            //    {
            //        buffer.Append(line);
            //        buffer.Append(Environment.NewLine);
            //        line = reader.ReadLine();
            //        Thread.Sleep(100);
            //    }
            //}
            //if (process.ExitCode != 0)
            //{
            //    //throw new Exception(string.Format(@"""{0}"" exited with ExitCode {1}. Output: {2}",
            //    //    executable, process.ExitCode, buffer.ToString()));
            //}
        }

    }
}
