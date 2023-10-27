using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class HomeNavigateCommand : CommandBase
    {

        private readonly NavigateCommand _navigationCommand;
        public HomeNavigateCommand(NavigationStore navigationStore, Func<HomeViewModel> createHomeViewModel)
        {
            _navigationCommand = new NavigateCommand(navigationStore, createHomeViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
