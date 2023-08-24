using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookInventoryAdminProgram.View
{
    /// <summary>
    /// Interaction logic for StaffViewerPanel.xaml
    /// </summary>
    public partial class StaffViewerPanel : UserControl
    {
        public StaffViewerPanel()
        {
            InitializeComponent();
            var employees = new List<Employees>();
            employees.Add(new Employees() { EmployeeID = 1, FirstName = "Karol", SecondName = "Paluch", Admin = "Admin"});
            employees.Add(new Employees() { EmployeeID = 2, FirstName = "Andre", SecondName = "Abreu", Admin = "Admin" });
            employees.Add(new Employees() { EmployeeID = 3, FirstName = "Rory", SecondName = "Fisken", Admin = "Staff" });
            employees.Add(new Employees() { EmployeeID = 4, FirstName = "Jack", SecondName = "Kormitt", Admin = "Staff" });
            employees.Add(new Employees() { EmployeeID = 5, FirstName = "John", SecondName = "Doe", Admin = "Admin" });
            employees.Add(new Employees() { EmployeeID = 6, FirstName = "Jane", SecondName = "Doe", Admin = "Staff" });
            employees.Add(new Employees() { EmployeeID = 7, FirstName = "Beff", SecondName = "Beezos", Admin = "Staff" });
            employees.Add(new Employees() { EmployeeID = 8, FirstName = "Tim", SecondName = "Correy", Admin = "Staff" });
            employees.Add(new Employees() { EmployeeID = 9, FirstName = "Aaron", SecondName = "Musk", Admin = "Staff" });

            EmployeesDatagrid.ItemsSource = employees;

        }
    }

    public class Employees
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Admin { get; set; }
    }
}
