using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands.NavigateCommands
{
    class SalesPanelNavigateCommand : CommandBase
    {
        private readonly NavigateCommand _navigationCommand;
        public SalesPanelNavigateCommand(NavigationStore navigationStore, Func<SalesPanelViewModel> createSalesPanelViewModel)
        {
            _navigationCommand = new NavigateCommand(navigationStore, createSalesPanelViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
