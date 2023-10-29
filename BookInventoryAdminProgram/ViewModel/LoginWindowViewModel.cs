using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookInventoryAdminProgram.ViewModel
{
    public class LoginWindowViewModel : ViewModelBase, INotifyDataErrorInfo
    {
		private string _employeeID;
		public string EmployeeID
		{
			get { return _employeeID; }
			set
			{
				_employeeID = value;

                
                // remove all errors before we add new ones because the old one wont remove and
                // we'll have 2 errors forever and more.
                _errorsViewModel.RemoveError(nameof(EmployeeID));
                if (!int.TryParse(_employeeID, out int intValue) && _employeeID != "")
                    _errorsViewModel.AddError(nameof(EmployeeID), "EmployeeID is only a number");

                if (_employeeID == "")
                    EmployeeIDEmpty = true;
                else if (_employeeID != "")
                    EmployeeIDEmpty = false;

                OnPropertyChanged(nameof(EmployeeID));
                OnPropertyChanged(nameof(FinalCanLogin));
            }
		}

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;

                _errorsViewModel.RemoveError(nameof(Password));
                if(_password == "")
                    _errorsViewModel.AddError(nameof(Password), "Please enter a password before you login");
                else
                    PasswordEmpty = false;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(FinalCanLogin));
            }
        }


        public bool CanLoginErrorInterface => !HasErrors;
        public bool EmployeeIDEmpty = true;
        public bool PasswordEmpty = true;
        public bool FinalCanLogin => CanLoginErrorInterface && !EmployeeIDEmpty && !PasswordEmpty;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ICommand LoginCommand { get; }
        
        // so basically what is happening here is i cant inherit from 2 classes at once.
        // so i made a class we pass into here that has most of the boilerplate for 
        // INotifyDataErrorInfo. We use methods from there here.
        private readonly ErrorsViewModel _errorsViewModel;

        public LoginWindowViewModel(NavigationStore navigationStore, Func<HomeViewModel> createHomeViewModel, MainWindowViewModel mainWindowViewModel, Action openMainWindow, UserInfoStore userInfoStore, DatabaseHashStore _databaseHashStore)
        {
            LoginCommand = new LoginCommand(this, navigationStore, createHomeViewModel, mainWindowViewModel, openMainWindow, userInfoStore, _databaseHashStore);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        }

        private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(FinalCanLogin));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}
