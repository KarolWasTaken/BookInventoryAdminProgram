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

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            // set default window on startup to be LoginWindow
            _navigationStore.CurrentViewModel = new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel);
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore, CreateHomeViewModel, CreateInventoryPanelViewModel, CreateStaffViewerViewModel, CreateLoginWindowViewModel)
            };
            MainWindow.Show();

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
            return new LoginWindowViewModel(_navigationStore, CreateHomeViewModel, _mainWindowViewModel);
        }
    }
}
