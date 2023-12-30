using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class RemoveExpenseListItem : CommandBase
    {
        private SalesPanelViewModel _salesPanelViewModel;
        private string _selectedExpenseFromListOfExpenses;
        public RemoveExpenseListItem(SalesPanelViewModel salesPanelViewModel, string selectedExpenseFromListOfExpenses)
        {
            _salesPanelViewModel = salesPanelViewModel;
        }
        public override void Execute(object? parameter)
        {
            string key;
            bool deleteAll = false;
            

            if (parameter.ToString() == "All")
                deleteAll = true;


            if ((_salesPanelViewModel.SelectedExpenseFromListOfExpenses == null || _salesPanelViewModel.SelectedExpenseFromListOfExpenses == "") && !deleteAll)
                return;
            else if (deleteAll)
                { 
                    _salesPanelViewModel.ListOfExpenses.Clear();
                    _salesPanelViewModel.ExpensesDictionary.Clear();
                }
            else
            {
                string selectedString = _salesPanelViewModel.SelectedExpenseFromListOfExpenses;
                _salesPanelViewModel.ListOfExpenses.Remove(_salesPanelViewModel.SelectedExpenseFromListOfExpenses);
                int dashIndex = selectedString.IndexOf('-');
                if (dashIndex != -1) // Check if the dash exists in the string
                {     
                    string extractedString = selectedString.Substring(0, dashIndex).Trim();
                    _salesPanelViewModel.ExpensesDictionary.Remove(extractedString);
                }
                else
                {
                    throw new Exception("Literally impossible exception idk how you hit this");
                }
            }
        }
    }
}
