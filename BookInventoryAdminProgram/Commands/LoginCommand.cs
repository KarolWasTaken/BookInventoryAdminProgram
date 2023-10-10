using AdonisUI.Controls;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.View;
using BookInventoryAdminProgram.ViewModel;
using Dapper;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginWindowViewModel _loginWindowViewModel;
        private readonly UserInfoStore _userInfoStore;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly NavigateCommand _navigationCommand;
        public Action _openMainWindow;
        /// <summary>
        /// Logins in our user, grabs his first and second name, and changes viewmodel to home view
        /// </summary>
        /// <param name="loginWindowViewModel">Need this to access inputs</param>
        /// <param name="navigationStore">Need this to change viewmodels</param>
        /// <param name="createHomeViewModel">func that changes the viewmodel. Made in app.xaml.cs and passed over here to make _navigationCommand</param>
        /// <param name="mainWindowViewModel">needed to change Welcome <Firstname> msg after login</param>
        public LoginCommand(LoginWindowViewModel loginWindowViewModel, NavigationStore navigationStore, Func<HomeViewModel> createHomeViewModel, MainWindowViewModel mainWindowViewModel, Action openMainWindow, UserInfoStore userInfoStore)
        {
            _loginWindowViewModel = loginWindowViewModel;
            _navigationCommand = new NavigateCommand(navigationStore, createHomeViewModel);
            _openMainWindow = openMainWindow;
            _userInfoStore = userInfoStore;
        }

        public override void Execute(object? parameter)
        {
            PasswordFunctions pf = new PasswordFunctions();
            if (!(int.TryParse(_loginWindowViewModel.EmployeeID, out int intValue)))
            {
                // input is not in ID
                ThrowLoginError(_loginWindowViewModel, usernameFail:true);
                return;
            }
            int EmployeeID = int.Parse(_loginWindowViewModel.EmployeeID);
            bool result = pf.VerifyPassword(EmployeeID, _loginWindowViewModel.Password);


            if (result == false) 
            {
                ThrowLoginError(_loginWindowViewModel, usernameFail:false, passwordFail:true);
                return;
            }

            UserInfoStore userNames;
            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                userNames = connection.QuerySingle<UserInfoStore>("dbo.spGetEmployeeName @EmployeeID", new { EmployeeID = EmployeeID });
            }
            _userInfoStore.StoreUserName(userNames);



            // this action closes login window and opens mainwindow.
            _openMainWindow?.Invoke();
            // command responsible for chaning viewmodel which changes the view and datacontext
            _navigationCommand.ChangeViewModel();
            // grabs maindata from our database and stores it in our maindataset store
            //_mainDataSetStore = md.GetMainDataset(); RELIC
        }


        /// <summary>
        /// MessageBox notifying error login. Wipe input boxes.
        /// </summary>
        /// <param name="loginWindowViewModel"></param>
        private static void ThrowLoginError(LoginWindowViewModel loginWindowViewModel, bool usernameFail = false, bool passwordFail = false)
        {
            if (usernameFail == true)
                loginWindowViewModel.EmployeeID = "";
            if (passwordFail == true)
                loginWindowViewModel.Password = "";
            MessageBox.Show("Error: login failed.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }  
    }
}
