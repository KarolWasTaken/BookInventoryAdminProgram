using AdonisUI.Controls;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace BookInventoryAdminProgram.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly NavigateCommand _navigationCommand;
        private Action _openLoginWindow;
        public LogoutCommand(MainWindowViewModel mainWindowViewModel, NavigationStore navigationStore, Func<LoginWindowViewModel> createLoginWindow, Action openLoginWindow)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _navigationCommand = new NavigateCommand(navigationStore, createLoginWindow);
            _openLoginWindow = openLoginWindow;
        }

        public override void Execute(object? parameter)
        {
            var messageBox = new MessageBoxModel
            {
                Text = "Are you sure you want to logout?",
                Caption = "Info",
                Icon = AdonisUI.Controls.MessageBoxImage.Question,
                Buttons = MessageBoxButtons.YesNo(),
            };
            AdonisUI.Controls.MessageBox.Show(messageBox);

            
            switch (messageBox.Result)
            {
                case AdonisUI.Controls.MessageBoxResult.Yes:
                    _openLoginWindow?.Invoke();
                    _navigationCommand.ChangeViewModel();
                    break;
                case AdonisUI.Controls.MessageBoxResult.No:
                    return;
            }
        }
    }
}
