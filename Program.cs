using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using IronPdf;

class VirtualPrinter
{
    private static string ghostscriptPath = @"C:\Program Files\gs\gs10.04.0\bin\gswin64c.exe";

    static void Main()
    {
        Console.WriteLine("Press Ctrl + P to print...");

        do
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.P)
                {
                    Console.WriteLine("\nPrinting initiated...");
                    HandlePrintJob();
                    break;
                }
            }
            Thread.Sleep(100);
        } while (true);
    }

    private static void HandlePrintJob()
    {
        string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoSaved_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

        var renderer = new ChromePdfRenderer();
        var pdf = renderer.RenderHtmlAsPdf("<h1>Sample Document for Printing</h1>");
        pdf.SaveAs(pdfFilePath);
        Console.WriteLine("PDF automatically saved: " + pdfFilePath);

        PrintDocument(pdf, "Microsoft Print to PDF");
    }

    private static void PrintDocument(PdfDocument pdf, string printerName)
    {
        try
        {
            var settings = new System.Drawing.Printing.PrinterSettings()
            {
                PrinterName = printerName,
                Copies = 1,
                FromPage = 1,
                ToPage = pdf.PageCount
            };

            var document = pdf.GetPrintDocument(settings);
            document.Print();
            Console.WriteLine("Document sent to " + printerName + " for printing.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error printing document: " + ex.Message);
        }
    }
}
