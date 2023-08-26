using BookInventoryAdminProgram.Stores;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public class GetUserData
    {

        /// <summary>
        /// Yo i will not lie, this is overcomplicated as hell but works. Now CurrentUserInfomationStore.cs can return 
        /// first and second name from anywhere. All I do is call this once, and the data is stored. This reduces
        /// the ammout of times i need to query the server when i want this data. 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /*public CurrentUserInfomation getUserData(int EmployeeID)
        {
            CurrentUserInfomation output;
            using (SqlConnection connection = new SqlConnection(Helper.CnnVal()))
            {
                output = connection.Query<CurrentUserInfomation>("dbo.spGetEmployeeName @EmployeeID", new { EmployeeID = EmployeeID }).ToList()[0];
            }
            return output;
        }*/
    }
}
