using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Conto.Data.ExcelHelpers
{
    //public class NpoiObjectExport : IDisposable
    //{
    //    const int MaximumNumberOfRowsPerSheet = 65500;
    //    const int MaximumSheetNameLength = 25;
    //    protected Workbook Workbook { get; set; }

    //    public NpoiObjectExport()
    //    {
    //        this.Workbook = new HSSFWorkbook();
    //    }

    //    protected string EscapeSheetName(string sheetName)
    //    {
    //        var escapedSheetName = sheetName
    //                                    .Replace("/", "-")
    //                                    .Replace("\\", " ")
    //                                    .Replace("?", string.Empty)
    //                                    .Replace("*", string.Empty)
    //                                    .Replace("[", string.Empty)
    //                                    .Replace("]", string.Empty)
    //                                    .Replace(":", string.Empty);

    //        if (escapedSheetName.Length > MaximumSheetNameLength)
    //            escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

    //        return escapedSheetName;
    //    }

    //    public void ExportSelfInvoice(SelfInvoiceDataObject selfInvoice)
    //    {
    //        var sheet = CreateSelfInvoiceSheet(string.Join("autofatt.", selfInvoice.InvoiceNumber));



    //        var row = sheet.CreateRow(4);
    //        row.HeightInPoints = 18f;
    //        var cell = row.CreateCell(3);
    //        //cellStyle.BorderBottom = CellBorderType.THIN;
    //        cell.SetCellValue("Autofattura");
    //        cell.CellStyle = CellStyle("ITC Zapf Chancery", 14);

    //        CreateInvoiceHeaderSquare(sheet, selfInvoice);

    //        CreateBody(sheet, selfInvoice);
    //    }


    //    private void CreateInvoiceHeaderSquare(Sheet sheet, SelfInvoiceDataObject selfInvoice)
    //    {
    //        var row = sheet.CreateRow(5);
    //        var cell = row.CreateCell(0);
    //        cell.SetCellValue("Cliente");
    //        cell.CellStyle = CellStyle(bold: true, alignment: HorizontalAlignment.CENTER);

    //        // ROW 6
    //        row = sheet.CreateRow(6);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("Nome:O.S. Trading S.r.l Soc. Unipersonale");
    //        cell.CellStyle = CellStyle(cellBorders: new[] {"top", "left"});
    //        cell.RichStringCellValue.ApplyFont(5,41, GetBoldFont());
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "top", "right" });

    //        cell = row.CreateCell(3);
    //        cell.SetCellValue("Data ft.");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "left" });

    //        cell = row.CreateCell(4);
    //        cell.SetCellValue(selfInvoice.InvoiceDate.ToString("dd/MM/yyyy"));
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "top", "right" }, alignment: HorizontalAlignment.RIGHT);

    //        // ROW 7
    //        row = sheet.CreateRow(7);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("Indirizzo:Via Mascagni snc");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left" });
    //        cell.RichStringCellValue.ApplyFont(10, 26, GetBoldFont());
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" });

    //        cell = row.CreateCell(3);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left" });

    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" });

    //        // ROW 8
    //        row = sheet.CreateRow(8);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("Cap. Città:20040 Usmate Velate");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left" });
    //        cell.RichStringCellValue.ApplyFont(11, 30, GetBoldFont());
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" });

    //        cell = row.CreateCell(3);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left" });

    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" });


    //        // ROW 9
    //        row = sheet.CreateRow(9);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("C.F:  05962770961");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left" });
    //        cell.RichStringCellValue.ApplyFont(4, 17, GetBoldFont());
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" });

    //        cell = row.CreateCell(3);
    //        cell.SetCellValue("Numero ft");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "left" });

    //        cell = row.CreateCell(4);
    //        cell.SetCellValue(string.Format("{0}/{1}",selfInvoice.InvoiceNumber.ToString(CultureInfo.InvariantCulture),
    //            selfInvoice.InvoiceYear.ToString(CultureInfo.InvariantCulture).Substring(2)));
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "right" }, alignment: HorizontalAlignment.RIGHT);

    //        // ROW 10
    //        row = sheet.CreateRow(10);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("P.Iva:05962770961");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left" });
    //        cell.RichStringCellValue.ApplyFont(6, 17, GetBoldFont());
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "right" });

    //        cell = row.CreateCell(3);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left" });

    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "right" });

    //    }

    //    private void CreateBody(Sheet sheet, SelfInvoiceDataObject selfInvoice)
    //    {
    //        // ROW 12
    //        var row = sheet.CreateRow(12);

    //        var cell = row.CreateCell(0);
    //        cell.SetCellValue("Descrizione");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "bottom", "left", "right" }, alignment: HorizontalAlignment.CENTER);
    //        cell = row.CreateCell(1);
    //        cell.SetCellValue("Quantità ton.");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "bottom", "left", "right" }, alignment: HorizontalAlignment.CENTER);
    //        cell = row.CreateCell(2);
    //        cell.SetCellValue("Prezzo");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "bottom", "left", "right" }, alignment: HorizontalAlignment.CENTER);
    //        cell = row.CreateCell(3);
    //        cell.SetCellValue("IVA");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "bottom", "left", "right" }, alignment: HorizontalAlignment.CENTER);
    //        cell = row.CreateCell(4);
    //        cell.SetCellValue("Importo in € ");
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "top", "bottom", "left", "right" }, alignment: HorizontalAlignment.CENTER);
            
    //        AddBodyEmptyRow(sheet, 13);
    //        AddBodyEmptyRow(sheet, 14);
    //        AddBodyEmptyRow(sheet, 15);
    //        AddBodyEmptyRow(sheet, 16);

    //        // ROW 17
    //        row = sheet.CreateRow(17);

    //        cell = row.CreateCell(0);
    //        cell.SetCellValue(selfInvoice.Material.Name);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(1);
    //        cell.SetCellValue((double)selfInvoice.Quantity);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" }, alignment: HorizontalAlignment.RIGHT);
    //        cell = row.CreateCell(2);
    //        cell.SetCellValue("€ " + selfInvoice.Material.Price.ToString("N2"));
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" }, alignment: HorizontalAlignment.RIGHT);
    //        cell = row.CreateCell(3);
    //        //cell.SetCellValue("");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" }, alignment: HorizontalAlignment.RIGHT);
    //        cell = row.CreateCell(4);
    //        cell.SetCellValue("€ " + (selfInvoice.Quantity * selfInvoice.Material.Price).ToString("N2"));
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" }, alignment: HorizontalAlignment.RIGHT);

    //        AddBodyEmptyRow(sheet, 18);
    //        AddBodyEmptyRow(sheet, 19);
    //        AddBodyEmptyRow(sheet, 20);
    //        AddBodyEmptyRow(sheet, 21);
    //        AddBodyEmptyRow(sheet, 22);
    //        AddBodyEmptyRow(sheet, 23);
    //        AddBodyEmptyRow(sheet, 24);
    //        AddBodyEmptyRow(sheet, 25);
    //        AddBodyEmptyRow(sheet, 26);
    //        AddBodyEmptyRow(sheet, 27);
    //        AddBodyEmptyRow(sheet, 28);
    //        AddBodyEmptyRow(sheet, 29);
    //        AddBodyEmptyRow(sheet, 30);
    //        AddBodyEmptyRow(sheet, 31);
    //        AddBodyEmptyRow(sheet, 32);
    //        AddBodyEmptyRow(sheet, 33);
    //        AddBodyEmptyRow(sheet, 34);
    //        AddBodyEmptyRow(sheet, 35);
    //        AddBodyEmptyRow(sheet, 36);
    //        AddBodyEmptyRow(sheet, 37);
    //        AddBodyEmptyRow(sheet, 38);
    //        AddBodyEmptyRow(sheet, 39);
    //        AddBodyEmptyRow(sheet, 40);

    //        row = sheet.CreateRow(41);
    //        cell = row.CreateCell(0);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });
    //        cell = row.CreateCell(2);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });
    //        cell = row.CreateCell(3);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });
    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });

    //        row = sheet.CreateRow(42);
    //        cell = row.CreateCell(2);
    //        cell.SetCellValue("Imponibile");

    //        cell = row.CreateCell(4);
    //        cell.SetCellValue("€ " + (selfInvoice.Quantity * selfInvoice.Material.Price).ToString("N2"));
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" }, alignment: HorizontalAlignment.RIGHT);

    //        row = sheet.CreateRow(43);
    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("Modalità di pagamento ");
    //        cell = row.CreateCell(1);
    //        cell.SetCellValue("Pag 1");
    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
            
    //        row = sheet.CreateRow(44);
    //        cell = row.CreateCell(2);
    //        cell.SetCellValue("Iva");
    //        cell = row.CreateCell(4);
    //        cell.SetCellValue("esente art. 74");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });

    //        row = sheet.CreateRow(45);
    //        cell = row.CreateCell(0);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "top", "left", "right" });
    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });

    //        row = sheet.CreateRow(46);
    //        cell = row.CreateCell(0);
    //        cell.SetCellValue("Rimessa diretta per contanti");
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });

    //        row = sheet.CreateRow(47);
    //        cell = row.CreateCell(0);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "bottom", "left", "right" });
    //        cell = row.CreateCell(2);
    //        cell.SetCellValue("TOTALE");
    //        cell = row.CreateCell(4);
    //        cell.SetCellValue("€ " + (selfInvoice.Quantity * selfInvoice.Material.Price).ToString("N2"));
    //        cell.CellStyle = CellStyle(bold: true, cellBorders: new[] { "bottom", "left", "right" }, alignment: HorizontalAlignment.RIGHT);
    //    }

    //    private void AddBodyEmptyRow(Sheet sheet, int rowIndex)
    //    {
    //        var row = sheet.CreateRow(rowIndex);
    //        var cell = row.CreateCell(0);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(1);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(2);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(3);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //        cell = row.CreateCell(4);
    //        cell.CellStyle = CellStyle(cellBorders: new[] { "left", "right" });
    //    }

    //    private Font GetBoldFont()
    //    {
    //        var labelFont = this.Workbook.CreateFont();
    //        labelFont.Boldweight = (short)FontBoldWeight.BOLD;

    //        return labelFont;
    //    }

    //    private CellStyle CellStyle(string fontName = "", short fontHeight = 0, bool bold = false, IEnumerable<string> cellBorders = null, HorizontalAlignment alignment = HorizontalAlignment.GENERAL)
    //    {
    //        var cellStyle = this.Workbook.CreateCellStyle();
    //        var labelFont = this.Workbook.CreateFont();
    //        if(bold)
    //            labelFont.Boldweight = (short) FontBoldWeight.BOLD;
    //        if(!string.IsNullOrEmpty(fontName))
    //            labelFont.FontName = fontName;
    //        if(fontHeight > 0)
    //            labelFont.FontHeightInPoints = fontHeight;

    //        if (cellBorders != null)
    //        {
    //            foreach (var cellBorderType in cellBorders)
    //            {
    //                switch (cellBorderType)
    //                {
    //                    case "top":
    //                        cellStyle.BorderTop = CellBorderType.THIN;
    //                        break;
    //                    case "bottom":
    //                        cellStyle.BorderBottom = CellBorderType.THIN;
    //                        break;
    //                    case "right":
    //                        cellStyle.BorderRight = CellBorderType.THIN;
    //                        break;
    //                    case "left":
    //                        cellStyle.BorderLeft = CellBorderType.THIN;
    //                        break;
    //                }
    //            }
    //        }

    //        cellStyle.Alignment = alignment;

    //        cellStyle.SetFont(labelFont);
    //        return cellStyle;
    //    }

    //    private Sheet CreateSelfInvoiceSheet(string sheetName)
    //    {
    //        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));
    //        // Columns sizes
    //        SetColumnWidth(sheet, 0, 28.43d);
    //        SetColumnWidth(sheet, 1, 13.71d);
    //        SetColumnWidth(sheet, 2, 12.29d);
    //        SetColumnWidth(sheet, 3, 8.71d);
    //        SetColumnWidth(sheet, 4, 15.86d);

    //        // Row sizes
    //        var row = sheet.CreateRow(0);
    //        row.HeightInPoints = 31.5f;
    //        row = sheet.CreateRow(1);
    //        row.HeightInPoints = 20.25f;
    //        row = sheet.CreateRow(2);
    //        row.HeightInPoints = 18f;
           
    //        return sheet;
    //    }

    //    public void SetColumnWidth(Sheet sheet, int colIdx, double widthInTwips)
    //    {
    //        sheet.SetColumnWidth(colIdx, (int)(441.3793d + 256d * (widthInTwips - 1d)));
    //    } 

    //    public void ExportCollectionToWorkbook<T>(IList<T> collList, string sheetName)
    //    {
    //        var headerLabelCellStyle = this.Workbook.CreateCellStyle();
    //        headerLabelCellStyle.BorderBottom = CellBorderType.THIN;
    //        var headerLabelFont = this.Workbook.CreateFont();
    //        headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
    //        headerLabelCellStyle.SetFont(headerLabelFont);

    //        var sheet = CreateExportSheetAndHeaderRow(collList, sheetName, headerLabelCellStyle);
    //        var currentNpoiRowIndex = 1;
    //        var sheetCount = 1;

    //        for (int rowIndex = 0; rowIndex < collList.Count; rowIndex++)
    //        {
    //            if (currentNpoiRowIndex >= MaximumNumberOfRowsPerSheet)
    //            {
    //                sheetCount++;
    //                currentNpoiRowIndex = 1;

    //                sheet = CreateExportSheetAndHeaderRow(collList,
    //                                                      sheetName + " - " + sheetCount,
    //                                                      headerLabelCellStyle);
    //            }

    //            var row = sheet.CreateRow(currentNpoiRowIndex++);

    //            int colIndex = 0;
    //            Type collType = typeof(T);
    //            foreach (var propertyInfo in collType.GetProperties())
    //            {
    //                Type cellType = typeof (object);                   
    //                object[] attrs = propertyInfo.GetCustomAttributes(typeof (ExcelTypeAttribute), true);
    //                foreach (var attr in attrs)
    //                {
    //                    var excelAttribute = attr as ExcelTypeAttribute;
    //                    if (excelAttribute != null)
    //                    {
    //                        cellType = excelAttribute.DestinationType;
    //                    }
    //                }

    //                var cell = row.CreateCell(colIndex);
    //                switch (cellType.ToString())
    //                {
    //                    case "System.Int32":
    //                        cell.SetCellValue(GetPropValue<int>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                    case "System.Int64":
    //                        cell.SetCellValue(GetPropValue<long>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                    case "System.Double":
    //                        cell.SetCellValue(GetPropValue<double>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                    case "System.DateTime":
    //                        cell.SetCellValue(GetPropValue<DateTime>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                    case "System.Boolean":
    //                        cell.SetCellValue(GetPropValue<bool>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                    default:
    //                        cell.SetCellValue(GetPropValue<string>(collList[rowIndex], propertyInfo.Name));
    //                        break;
    //                }

    //                colIndex++;
    //            }
    //        }

    //    }

    //    protected static T GetPropValue<T>(object src, string propName)
    //    {
    //        try
    //        {
    //            return (T) src.GetType().GetProperty(propName).GetValue(src, null);
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception(string.Format("Cast exception - Type {0} - Prop {1}", typeof(T), propName));
    //        }
    //    }

    //    protected Sheet CreateExportSheetAndHeaderRow<T>(IList<T> collList, string sheetName, CellStyle headerRowStyle)
    //    {
    //        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

    //        // Create the header row
    //        var row = sheet.CreateRow(0);

    //        Type collType = typeof(T);
    //        int colIndex = 0;
    //        foreach (var propertyInfo in collType.GetProperties())
    //        {
    //            var cell = row.CreateCell(colIndex);
    //            cell.SetCellValue(propertyInfo.Name);

    //            if (headerRowStyle != null)
    //                cell.CellStyle = headerRowStyle;

    //            colIndex++;
    //        }

    //        return sheet;
    //    }

    //    public byte[] GetBytes()
    //    {
    //        using (var buffer = new MemoryStream())
    //        {
    //            this.Workbook.Write(buffer);
    //            return buffer.GetBuffer();
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (this.Workbook != null)
    //            this.Workbook.Dispose();
    //    }
    //}
}
