﻿using AdonisUI.Controls;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookInventoryAdminProgram.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly NavigateCommand _navigationCommand;
        public LogoutCommand(MainWindowViewModel mainWindowViewModel, NavigationStore navigationStore, Func<LoginWindowViewModel> createLoginWindow)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _navigationCommand = new NavigateCommand(navigationStore, createLoginWindow);
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
                    _navigationCommand.ChangeViewModel();
                    break;
                case AdonisUI.Controls.MessageBoxResult.No:
                    return;
            }
        }
    }
}
