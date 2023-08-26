using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class StaffViewerNavigateCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly NavigateCommand _navigationCommand;
        public StaffViewerNavigateCommand(MainWindowViewModel mainWindowViewModel, NavigationStore navigationStore, Func<StaffViewerPanelViewModel> createStaffViewerPanelViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _navigationCommand = new NavigateCommand(navigationStore, createStaffViewerPanelViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
