using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookInventoryAdminProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            // Debug to check if connection to server is working. Throw if not.
            /*MessageBox.Show($"{Helper.CnnVal()}"); debug

            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                conn.Open(); // throws if invalid
            }*/






            base.OnStartup(e);
        }
    }
}
