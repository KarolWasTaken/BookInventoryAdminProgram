using AdonisUI.Controls;
using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.ViewModel;
using iText.IO.Util;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Model.SalesOperations;

namespace BookInventoryAdminProgram.Commands
{
    public class GeneratePDFCommand : CommandBase
    {
        private SalesPanelViewModel _salesPanelViewModel;
        private Dictionary<string, double> _expensesDictionary;
        public GeneratePDFCommand(SalesPanelViewModel salesPanelViewModel, Dictionary<string, double> expensesDictionary)
        {
            _salesPanelViewModel = salesPanelViewModel;
            _expensesDictionary = expensesDictionary;
        }
        public override void Execute(object? parameter)
        {
            if(_salesPanelViewModel.FileLocation == string.Empty)
            {
                //MessageBox.Show("Error: File location unknown.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                ThrowError("Error: File location unknown.");
                return;
            }
            DateTime startDate;
            DateTime endDate;
            Dictionary<string, double> copyDict = new Dictionary<string, double>();
            foreach (var kvp in _expensesDictionary)
            {
                copyDict.Add(kvp.Key, kvp.Value);
            }
            PDFGenerator pdfg = new PDFGenerator(_salesPanelViewModel.FileLocation, _salesPanelViewModel.Database, copyDict);
            // could probably implement and enum here for readability. Do this later future karol
            switch(_salesPanelViewModel.SelectedInputType)
            {
                // Year
                case 0:
                    if(_salesPanelViewModel.SelectedYear == 0)
                    {
                        ThrowError("Year is Invalid\nPlease selected a Year.");
                        return;
                    }
                    pdfg.GeneratePDF(_salesPanelViewModel.SelectedYear);
                    break;
                // Quarter
                case 1:
                    if(_salesPanelViewModel.SelectedYear == 0 || _salesPanelViewModel.SelectedSalesQuarter == null)
                    {
                        ThrowError("Year or Quarter is Invalid\nPlease selected a Quarter or Year.");
                        return;
                    }
                    SalesQuarter quarter = ConvertStringQuarterToEnumQuarter(_salesPanelViewModel.SelectedSalesQuarter);
                    pdfg.GeneratePDF(_salesPanelViewModel.SelectedYear, quarter);
                    break;
                // Date
                case 2:
                    if (!(_salesPanelViewModel.SelectedDate >= _salesPanelViewModel.MinimumDate && _salesPanelViewModel.SelectedDate <= _salesPanelViewModel.MaximumDate))
                    {
                        ThrowError("Date is invalid.\nPlease selected a Date.");
                        return;
                    }
                    pdfg.GeneratePDF(_salesPanelViewModel.SelectedDate);
                    break;
                default:
                    throw new Exception("Unknown SelectedInputType");
            }
        }

        private static void ThrowError(string ErrorMessage)
        {
            MessageBox.Show(ErrorMessage, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private static SalesQuarter ConvertStringQuarterToEnumQuarter(string quarter)
        {
            switch (quarter)
            {
                case "Q1":
                    return SalesQuarter.Q1;
                case "Q2":
                    return SalesQuarter.Q2;
                case "Q3":
                    return SalesQuarter.Q3;
                case "Q4":
                    return SalesQuarter.Q4;
                default: throw new Exception("How did you even hit this exception");
            }
        }
    }
}
