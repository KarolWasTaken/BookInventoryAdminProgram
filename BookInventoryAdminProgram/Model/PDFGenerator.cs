using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Model.SalesOperations;
using static BookInventoryAdminProgram.Stores.DatabaseStore;
using System.Windows.Controls;
using System.Windows.Documents;
using BookInventoryAdminProgram.Model;
using static BookInventoryAdminProgram.Model.Formatters;

// i text7 imports (theres a lot)
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Renderer;
using Paragraph = iText.Layout.Element.Paragraph;
using Cell = iText.Layout.Element.Cell;
using Table = iText.Layout.Element.Table;
using iText.Kernel.Colors;
using Div = iText.Layout.Element.Div;
using iText.Layout.Properties;
using iText.Layout.Borders;
using System.Data.Common;
using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using Border = iText.Layout.Borders.Border;
using System.IO.Packaging;
using AdonisUI.Controls;
using BookInventoryAdminProgram.ViewModel;
using System.Runtime.CompilerServices;

namespace BookInventoryAdminProgram.Model
{
    public class PDFGenerator
    {

        
        private string _fileLocation;
        private List<BookInfo> _database;
        private Dictionary<string, double> _expensesDictionary;
        public PDFGenerator(string fileLocation, List<BookInfo> database, Dictionary<string, double> expensesDictionary)
        {
            _fileLocation = fileLocation;
            _database = database;
            _expensesDictionary = expensesDictionary;
        }

        /// <summary>
        /// Generate PDF sales report for given sales quarter
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="database"></param>
        /// <param name="showNulls">Optional</param>
        public void GeneratePDF(int year, SalesQuarter salesQuarter)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            DateTime startDate = dates[Date.StartDate];
            DateTime endDate = dates[Date.EndDate];
            string reportTite = $"{year}-{salesQuarter}";
            string fileName = $"SalesReport_{year}_{salesQuarter}";
            GeneratePDF(startDate, endDate, _database, _fileLocation, fileName, reportTite);
        }
        /// <summary>
        /// Generate PDF sales report for given year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="database"></param>
        /// <param name="showNulls">Optional</param>
        /// <exception cref="Exception">"Year is invalid" exception</exception>
        public void GeneratePDF(int year)
        {
            if (year.ToString().Length != 4)
            {
                MessageBox.Show("Year is invalid");
                throw new Exception("Year is invalid");
            }
            string reportTitle = $"{year}";
            string fileName = $"SalesReport_{year}";
            GeneratePDF(new DateTime(year, 1, 1), new DateTime(year, 12, 31), _database, _fileLocation, fileName, reportTitle);
        }
        /// <summary>
        /// Generate PDF sales report for given date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="database"></param>
        /// <param name="showNulls">Optional</param>
        public void GeneratePDF(DateTime date)
        {
            string reportTitle = $"{date.ToString("yyyy/MM/dd")}";
            string fileName = $"SalesReport_{date.ToString("yyyy-MM-dd")}";
            GeneratePDF(date, date, _database, _fileLocation, fileName, reportTitle);
        }
        public void GeneratePDF(DateTime startDate, DateTime endDate, List<BookInfo> database, string reportFileLocation, string filename, string reportTitle, bool showNulls = true)
        {
            //string fileName;
            // handles if sales report is for specific day
            /*if (startDate == endDate)
                fileName = $"{startDate.ToString("yyyy/MM/dd")}";
            else
                fileName = $"{startDate.ToString("yyyy/MM/dd")}-{endDate.ToString("yyyy/MM/dd")}";*/


            //if (reportTitle == null)
            //    reportTitle = $"{startDate.ToString("yyyy/MM/dd")}-{endDate.ToString("yyyy/MM/dd")}";

            // Create a new PDF document
            //string fileLocation = @"C:\Users\User\source\repos\BookInventoryAdminProgram\BookInventoryAdminProgram"; debug
            try
            {

                using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(@$"{reportFileLocation}/{filename}.pdf")))
                {

                    // Create a document
                    using (var document = new iText.Layout.Document(pdfDoc))
                    {
                        double totalRevenue = 0;
                        double totalProfit = 0;

                        // Add a header above the chart
                        Paragraph header = new Paragraph($"Sales Report: [{reportTitle}]\n{GetMonthName(startDate.Month)} {startDate.Day}{GetNumberExtension(startDate.Day)} - {GetMonthName(endDate.Month)} {endDate.Day}{GetNumberExtension(endDate.Day)}"); // Replace "Sales Report" with your desired header text
                        header.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        header.SetFontSize(25);
                        document.Add(header);
                        // Create a table
                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(9);

                        List<string> headers = new List<string>()
                        {"BookID", "Title", "Price (£)", "Latest PPU (£)", "Quantity Sold", "Revenue (£)", "COS (£)", "Gross Profit (£)", "CPU (£)"};


                        Cell headerCell;
                        foreach (string headerTitle in headers)
                        {
                            headerCell = new Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                            //headerCell = new Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                            headerCell.Add(new Paragraph(headerTitle));
                            table.AddHeaderCell(headerCell);
                        }

                        // Add data rows
                        foreach (var book in database)
                        {
                            if (SalesExists(startDate, endDate, book)) //book.MonthlySales.Count > 0
                            {
                                table.AddCell(book.BookID.ToString());
                                table.AddCell(book.Title);
                                table.AddCell($"{GetPPUAttribute(endDate, BookPPU.SalePrice, book).ToFinancialNegative()}");
                                table.AddCell($"{GetPPUAttribute(endDate, BookPPU.PricePerUnit, book).ToFinancialNegative()}");
                                table.AddCell(GetQuantitySold(startDate, endDate, book).ToString());
                                totalRevenue += GetRevenue(startDate, endDate, book);
                                table.AddCell($"{GetRevenue(startDate, endDate, book).ToFinancialNegative()}");
                                table.AddCell($"{GetPriceOnUnitsSpent(startDate, endDate, book)}");
                                totalProfit += GetProfit(startDate, endDate, book);
                                table.AddCell($"{GetProfit(startDate, endDate, book).ToFinancialNegative()}");
                                table.AddCell($"{(GetPPUAttribute(endDate, BookPPU.SalePrice, book) - GetPPUAttribute(endDate, BookPPU.PricePerUnit, book)).ToFinancialNegative()}");
                            }
                            else
                            {
                                if (showNulls)
                                {
                                    //var errorCell = new Cell().SetFontColor(iText.Kernel.Colors.ColorConstants.RED).Add(new Paragraph("N/A"));

                                    table.AddCell(book.BookID.ToString());
                                    table.AddCell(book.Title);
                                    table.AddCell($"{GetPPUAttribute(endDate, BookPPU.SalePrice, book)}");
                                    table.AddCell($"{GetPPUAttribute(endDate, BookPPU.PricePerUnit, book)}");
                                    Cell NullText;
                                    for (int i = 4; i >= 0; i--)
                                    {
                                        NullText = new Cell().SetFontColor(iText.Kernel.Colors.ColorConstants.RED).Add(new Paragraph("N/A"));
                                        table.AddCell(NullText);
                                    }

                                }
                            }
                        }
                        document.Add(table);
                        Paragraph totalProfitText = CreateFormattedParagraph(
                            $"Total Revenue: £{totalRevenue.ToFinancialNegative()}\n" +
                            $"Total Profit: £{totalProfit.ToFinancialNegative()}",
                            12,
                            iText.Layout.Properties.TextAlignment.LEFT
                        );
                        document.Add(totalProfitText);



                        // add header for expenses section
                        // Add a header above the chart
                        Paragraph expensesSectionHeader = new Paragraph($"Expenses");
                        expensesSectionHeader.SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
                        expensesSectionHeader.SetFontSize(20);
                        document.Add(expensesSectionHeader);

                        Table divider = new Table(2);
                        Cell leftSide = new Cell();
                        Cell rightSide = new Cell();

                        // Create a table with two columns
                        Table expensesTable = new Table(new float[] { 1, 3 }); // Adjust the column widths as needed


                        List<string> expensesHeaders = _expensesDictionary.Keys.ToList();
                        expensesHeaders.Add("Total Expenses");

                        var test = _expensesDictionary;
                        Dictionary<string, double> expensesDictionary = test;
                        double totalExpenses = 0.0;
                        foreach (var values in _expensesDictionary.Values)
                        {
                            totalExpenses += values;
                        }
                        if (expensesDictionary.ContainsKey("Total Expenses"))
                            expensesDictionary.Remove("Total Expenses");
                    
                        expensesDictionary.Add("Total Expenses", totalExpenses);

                        foreach (string expheader in expensesHeaders)
                        {
                            Cell expensesHeader = new Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                                .Add(new Paragraph(expheader + " (£)"));
                            expensesTable.AddCell(expensesHeader);
                            expensesTable.AddCell(new Paragraph(expensesDictionary[expheader].ToFinancialNegative()));
                        }
                        leftSide.Add(expensesTable);
                        //div.Add(expensesTable);
                        //document.Add(expensesTable);
                        // Create a Div element to hold the label and the original table


                        Paragraph totalExpensesText = CreateFormattedParagraph(
                            $"Total expenses: £{expensesDictionary["Total Expenses"].ToFinancialNegative()}\n" +
                            $"Profit (before tax) after expenses: £{(totalProfit - expensesDictionary["Total Expenses"]).ToFinancialNegative()}",
                            15,
                            iText.Layout.Properties.TextAlignment.LEFT
                        );
                        rightSide.Add(totalExpensesText);

                        leftSide.SetBorder(Border.NO_BORDER);
                        leftSide.SetWidth(155);
                        rightSide.SetBorder(Border.NO_BORDER);

                        divider.AddCell(leftSide);
                        divider.AddCell(rightSide);
                        document.Add(divider);




                    }
                }
                MessageBox.Show($"Generates Sales report successfully!\nFileLocation: {@$"{reportFileLocation}\{filename}.pdf"}",
                "SUCCESS",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("PDF generator failed: \n" + ex.Message, "ERROR", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Error);
            }
        }
        private static Paragraph CreateFormattedParagraph(string text, float fontSize, iText.Layout.Properties.TextAlignment alignment)
        {
            Paragraph paragraph = new Paragraph(text);
            paragraph.SetFontSize(fontSize);
            paragraph.SetTextAlignment(alignment);
            return paragraph;
        }

    }
}
