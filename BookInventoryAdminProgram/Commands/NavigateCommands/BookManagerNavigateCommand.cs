using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class BookManagerNavigateCommand : CommandBase
    {
        private readonly NavigateCommand _navigationCommand;
        public BookManagerNavigateCommand(NavigationStore navigationStore, Func<BookManagerPanelViewModel> createBookManagerPanelViewModel)
        {
            _navigationCommand = new NavigateCommand(navigationStore, createBookManagerPanelViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
