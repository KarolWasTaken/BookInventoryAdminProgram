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
using static BookInventoryAdminProgram.Model.PopularityCalculator;

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

        /// <summary>
        /// Takes an Author/Genre/Publisher ID and returns the corresponding Name.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetPropertyByID(string property, int id)
        {
            string name = _junctionValuesDictionary[property].Where(n => n.ID == id).First().Name;
            if(name.EndsWith(" "))
                name = name.Remove(name.Length - 1);
            if(name.StartsWith(" "))
                name = name.Remove(0, 1);
            return name;
        }
        /// <summary>
        /// Gets Books that are below <see cref="Settings.LowBookStockWarningCount"/>
        /// </summary>
        /// <returns></returns>
        public List<BookInfo> GetBooksLowInStock()
        {
            List<BookInfo> results = _database.Where(n => n.BookStock <= Helper.ReturnSettings().LowBookStockWarningCount).ToList();
            return results;
        }
        /// <summary>
        /// Gets books low in stock that have the specified genres
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        public List<BookInfo> GetBooksLowInStock(string filterBy, List<string> source)
        {
            List<BookInfo> booksWithPropertyLowInStock = new List<BookInfo>();
            // Lambda function to choose the property to filter by - based on the filterBy parameter
            Func<BookInfo, IEnumerable<string>> filterPropertySelector;
            if (filterBy == "Genre")
            {
                filterPropertySelector = book => book.Genres;
            }
            else if (filterBy == "Author")
            {
                filterPropertySelector = book => book.Authors;
            }
            else
            {
                throw new ArgumentException("filterBy must be 'Genre' or 'Author'");
            }

            foreach (string filterValue in source) 
            {
                var results = _database
                    .Where(n => filterPropertySelector(n).Contains(filterValue))
                    .Where(n => n.BookStock <= Helper.ReturnSettings().LowBookStockWarningCount)
                    .ToList();
                booksWithPropertyLowInStock.AddRange(results);
            }
            booksWithPropertyLowInStock = booksWithPropertyLowInStock
                .Distinct()
                .OrderBy(n => n.BookStock)
                .ToList();
            return booksWithPropertyLowInStock;
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
