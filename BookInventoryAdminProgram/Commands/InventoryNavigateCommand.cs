using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class InventoryNavigateCommand : CommandBase
    {
        private readonly NavigateCommand _navigationCommand;
        public InventoryNavigateCommand(NavigationStore navigationStore, Func<InventoryPanelViewModel> createInventoryPanelViewModel)
        {
            _navigationCommand = new NavigateCommand(navigationStore, createInventoryPanelViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
