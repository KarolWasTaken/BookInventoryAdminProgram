using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class AddExpensesToDictCommand : CommandBase
    {
        private SalesPanelViewModel _salesPanelViewModel;

        public AddExpensesToDictCommand(SalesPanelViewModel salesPanelViewModel)
        {
            _salesPanelViewModel = salesPanelViewModel;
        }

        public override void Execute(object? parameter)
        {
            string expenseName = _salesPanelViewModel.ExpenseNameTextBoxField.Trim();
            string expenseCost = _salesPanelViewModel.ExpenseCostTextBoxField;

            if (!double.TryParse(expenseCost, out double number)) // Tries to parse the input as a double
            {
                return;
            }
            double expenseCostDouble = double.Parse(expenseCost);
            _salesPanelViewModel.ExpenseNameTextBoxField = null;
            _salesPanelViewModel.ExpenseCostTextBoxField = null;
            if(_salesPanelViewModel.ExpensesDictionary.ContainsKey(expenseName))
            {
                // handle error shit here later
                return;
            }
            _salesPanelViewModel.ExpensesDictionary.Add(expenseName, expenseCostDouble);
            _salesPanelViewModel.ListOfExpenses.Add($"{expenseName} - £{expenseCostDouble}");
        }
    }
}
