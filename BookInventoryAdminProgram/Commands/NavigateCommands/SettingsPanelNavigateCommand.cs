using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands.NavigateCommands
{
    public class SettingsPanelNavigateCommand : CommandBase
    {
        private readonly NavigateCommand _navigationCommand;
        public SettingsPanelNavigateCommand(NavigationStore navigationStore, Func<SettingsPanelViewModel> createSettingsPanelViewModel)
        {
            _navigationCommand = new NavigateCommand(navigationStore, createSettingsPanelViewModel);
        }
        public override void Execute(object? parameter)
        {
            _navigationCommand.ChangeViewModel();
        }
    }
}
