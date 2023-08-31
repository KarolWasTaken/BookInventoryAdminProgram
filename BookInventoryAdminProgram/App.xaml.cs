using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Navigation;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using BookInventoryAdminProgram.Windows;
using Dapper;


namespace BookInventoryAdminProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly NavigationStore _navigationStore;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly UserInfoStore _userInfoStore;
        private LoginWindow loginWindow;
        private MainWindow mainWindow;
        public App()
        {
            _navigationStore = new NavigationStore();
            _userInfoStore = new UserInfoStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            UserInfoStore userInfoStore = new UserInfoStore();



            OpenLoginWindow();

            // check whether or not the server an be reached.
            try
            {
                using (SqlConnection connection = new SqlConnection(Helper.CnnVal()))
                {
                    connection.Open(); // Try to open the connection
                }
            }
            catch (SqlException ex)
            {
                // Handle connection failure
                MessageBox.Show("Connection failed: " + ex.Message);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An error occurred: " + ex.Message);
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }

        /// <summary>
        /// Opens Login window
        /// </summary>
        private void OpenLoginWindow()
        {
            loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel, OpenMainWindow, _userInfoStore);

            if (IsMainWindowOpen())
                mainWindow.Close();
            loginWindow.ShowDialog(); // Show the login window as a modal dialog
        }
        /// <summary>
        /// Opens Main Window
        /// </summary>
        private void OpenMainWindow()
        {
            _navigationStore.CurrentViewModel = new HomeViewModel();
            mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore, CreateHomeViewModel, CreateInventoryPanelViewModel, CreateStaffViewerViewModel, CreateLoginWindowViewModel, OpenLoginWindow, _userInfoStore)
            };

            loginWindow.Close();
            mainWindow.Show();
        }
        /// <summary>
        /// Check if MainWindow is open.
        /// </summary>
        /// <returns></returns>
        bool IsMainWindowOpen()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    return true; // Main window is open
                }
            }
            return false; // Main window is not open
        }

        private HomeViewModel CreateHomeViewModel()
        {
            return new HomeViewModel();
        }
        private InventoryPanelViewModel CreateInventoryPanelViewModel()
        {
            return new InventoryPanelViewModel();
        }
        private StaffViewerPanelViewModel CreateStaffViewerViewModel()
        {
            return new StaffViewerPanelViewModel();
        }
        private LoginWindowViewModel CreateLoginWindowViewModel()
        {
            return new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel, OpenMainWindow, _userInfoStore);
        }
    }
}
