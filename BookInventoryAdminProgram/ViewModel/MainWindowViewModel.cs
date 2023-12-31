﻿using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Commands.NavigateCommands;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookInventoryAdminProgram.ViewModel
{
    public class MainWindowViewModel : ViewModelBase 
    {

        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        private string _employeeFistnameWelcome;
        public string EmployeeFirstnameWelcome
        {
            get
            {
                return $"Welcome {_employeeFistnameWelcome}";
            }
            set
            {
                _employeeFistnameWelcome = value;
                OnPropertyChanged(nameof(EmployeeFirstnameWelcome));
            }
        }


        public ICommand HomeNavigateCommand { get; }
        public ICommand InventoryNavigateCommand { get; }
        public ICommand BookManagerPanelNavigateCommand { get; }
        public ICommand SalesPanelNavigateCommand { get; }
        public ICommand StaffViewerNavigateCommand { get; }
        public ICommand SettingsPanelNavigateCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainWindowViewModel(NavigationStore navigationStore,
            Func<HomeViewModel> createHomeViewModel,
            Func<InventoryPanelViewModel> createInventoryPanelViewModel,
            Func<StaffViewerPanelViewModel> createStaffViewerPanelViewModel,
            Func<LoginWindowViewModel> createLoginWindowViewModel,
            Func<BookManagerPanelViewModel> createBookManagerPanelViewModel,
            Func<SettingsPanelViewModel> createSettingsPanelViewModel,
            Func<SalesPanelViewModel> createSalesPanelViewModel,
            Action openLoginWindow, UserInfoStore userInfoStore)
        {

            HomeNavigateCommand = new HomeNavigateCommand(navigationStore, createHomeViewModel);
            InventoryNavigateCommand = new InventoryNavigateCommand(navigationStore, createInventoryPanelViewModel);
            SalesPanelNavigateCommand = new SalesPanelNavigateCommand(navigationStore, createSalesPanelViewModel);
            StaffViewerNavigateCommand = new StaffViewerNavigateCommand(navigationStore, createStaffViewerPanelViewModel);
            LogoutCommand = new LogoutCommand(this, navigationStore, createLoginWindowViewModel, openLoginWindow);
            BookManagerPanelNavigateCommand = new BookManagerNavigateCommand(navigationStore, createBookManagerPanelViewModel);
            SettingsPanelNavigateCommand = new SettingsPanelNavigateCommand(navigationStore, createSettingsPanelViewModel);


            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            EmployeeFirstnameWelcome = userInfoStore.FirstName;
        }


        /// <summary>
        /// reminds model to update, on the ui thread, the CurrentViewModel property - which updates the view and datacontext
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
