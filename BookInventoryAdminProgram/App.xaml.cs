using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Navigation;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using BookInventoryAdminProgram.Windows;
using Dapper;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

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
        private readonly DatabaseHashStore _databaseHashStore;
        private LoginWindow loginWindow;
        private MainWindow mainWindow;
        public App()
        {
            _navigationStore = new NavigationStore();
            _userInfoStore = new UserInfoStore();
            _databaseHashStore = new DatabaseHashStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            UserInfoStore userInfoStore = new UserInfoStore();

            OpenLoginWindow();
            //OpenMainWindow();


            // check whether or not the server an be reached.
            try
            {
                using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
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
            loginWindow.DataContext = new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel, OpenMainWindow, _userInfoStore, _databaseHashStore);

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
                DataContext = new MainWindowViewModel(_navigationStore, CreateHomeViewModel, CreateInventoryPanelViewModel, CreateStaffViewerViewModel, CreateLoginWindowViewModel, CreateBookManagerPanelViewModel, CreateSettingsPanelViewModel, CreateSalesPanelViewModel, OpenLoginWindow, _userInfoStore)
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
            return new InventoryPanelViewModel(_databaseHashStore);
        }
        private StaffViewerPanelViewModel CreateStaffViewerViewModel()
        {
            return new StaffViewerPanelViewModel();
        }
        private LoginWindowViewModel CreateLoginWindowViewModel()
        {
            return new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel, OpenMainWindow, _userInfoStore, _databaseHashStore);
        }
        private BookManagerPanelViewModel CreateBookManagerPanelViewModel() 
        {
            return new BookManagerPanelViewModel();
        }
        private SettingsPanelViewModel CreateSettingsPanelViewModel()
        {
            return new SettingsPanelViewModel();
        }
        private SalesPanelViewModel CreateSalesPanelViewModel()
        {
            return new SalesPanelViewModel();
        }
    }
}
