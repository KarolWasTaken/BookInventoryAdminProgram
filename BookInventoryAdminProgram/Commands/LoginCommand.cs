using AdonisUI.Controls;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.View;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginWindowViewModel loginWindowViewModel;

        public LoginCommand(LoginWindowViewModel _loginWindowViewModel)
        {
            loginWindowViewModel = _loginWindowViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!(int.TryParse(loginWindowViewModel.EmployeeID, out int intValue)))
            {
                // input is not in ID
                // throw error on textbox with Inotifterrorchanged

                ThrowLoginError(loginWindowViewModel, usernameFail:true);
                return;
            }


            PasswordFunctions pf = new PasswordFunctions();
            bool result = pf.VerifyPassword(int.Parse(loginWindowViewModel.EmployeeID), loginWindowViewModel.Password);

            if (result == false) 
            {
                ThrowLoginError(loginWindowViewModel, usernameFail:false, passwordFail:true);
                return;
            }
            MessageBox.Show("Login Successful", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            // navigation logic here. Replace msg box
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
