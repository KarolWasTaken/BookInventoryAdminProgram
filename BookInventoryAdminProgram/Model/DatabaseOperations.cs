using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookInventoryAdminProgram.Stores;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Model
{

    /// <summary>
    /// Holds common operations like return staff, get book with BookID, etc
    /// </summary>
    public class DatabaseOperations
    {
        private List<BookInfo> _database;
        private Dictionary<string, List<CommonValues>> _junctionValuesDictionary;
        public DatabaseOperations()
        {
            _database = DatabaseStore.MainDataset;
            _junctionValuesDictionary = DatabaseStore.JunctionValuesDictionary;
        }


        public string GetPropertyByID(string property, int id)
        {
            string name = _junctionValuesDictionary[property].Where(n => n.ID == id).First().Name;
            if(name.EndsWith(" "))
                name = name.Remove(name.Length - 1);
            return name;
        }






        // I dunno if its bad practise or not but I like RS being self-contained
        public class ReturnStaff
        {
            public int EmployeeID { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public Boolean Administrator { get; set; }


            public List<ReturnStaff> GetStaff()
            {
                using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
                {
                    List<ReturnStaff> staffList = connection.Query<ReturnStaff>("dbo.spGetStaff").ToList();
                    return staffList;
                }
            }
        }
    }
}
