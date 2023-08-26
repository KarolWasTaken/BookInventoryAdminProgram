using BookInventoryAdminProgram.Commands;
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
        //public ICommand SalesReporterNavigateCommand { get; }
        //public ICommand GraphViewerNavigateCommand { get; }
        public ICommand StaffViewerNavigateCommand { get; }
        public ICommand LogoutCommand { get; }
        
        public MainWindowViewModel(NavigationStore navigationStore, Func<HomeViewModel> createHomeViewModel, Func<InventoryPanelViewModel> createInventoryPanelViewModel, 
            Func<StaffViewerPanelViewModel> createStaffViewerPanelViewModel, Func<LoginWindowViewModel> createLoginWindowViewModel, Action openLoginWindow)
        {

            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;


            HomeNavigateCommand = new HomeNavigateCommand(this, navigationStore, createHomeViewModel);
            InventoryNavigateCommand = new InventoryNavigateCommand(this, navigationStore, createInventoryPanelViewModel);
            StaffViewerNavigateCommand = new StaffViewerNavigateCommand(this, navigationStore, createStaffViewerPanelViewModel);
            LogoutCommand = new LogoutCommand(this, navigationStore, createLoginWindowViewModel, openLoginWindow);
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
