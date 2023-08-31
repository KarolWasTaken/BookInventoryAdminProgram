using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public class ReturnStaff
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public Boolean Administrator { get; set; }


        public List<ReturnStaff> GetStaff()
        {
            using (SqlConnection connection = new SqlConnection(Helper.CnnVal()))
            {
                List<ReturnStaff> staffList = connection.Query<ReturnStaff>("dbo.spGetStaff").ToList();
                return staffList;
            }
        }
    }
}
