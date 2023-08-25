using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using BookInventoryAdminProgram.Model;
using Dapper;


namespace BookInventoryAdminProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

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
    }
}
