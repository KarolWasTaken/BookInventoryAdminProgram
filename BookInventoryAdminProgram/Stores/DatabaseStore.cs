using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BookInventoryAdminProgram.Stores
{
    public class DatabaseStore
    {
        /// <summary>
        /// Class that stores database data
        /// </summary>
        public class BookInfo
        {
            public string ISBN { get; set; }
            public int BookID { get; set; }
            public string Title { get; set; }
            public double Price { get; set; }
            public int BookStock { get; set; }
            public List<string> Authors { get; set; }
            public List<string> Genres { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public string PublisherName { get; set; }
            public byte[] BookCover { get; set; }
            public List<AllTimeSales> AllTimeSales { get; set; }
            public List<YearlySales> YearlySales { get; set; }
            public List<MonthlySales> MonthlySales { get; set; }
            public List<DailySales> DailySales { get; set; }
        }
        public class DailySales
        {
            public DateTime SalesDate { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class MonthlySales
        {
            public int SalesYear { get; set; }
            public int SalesMonth { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class YearlySales
        {
            public int SalesYear { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class AllTimeSales
        {
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class BookAuthor
        {
            public int BookID { get; set; }
            public string AuthorName { get; set; }
        }

        public class BookGenre
        {
            public int BookID { get; set; }
            public string GenreName { get; set; }
        }
        public class DailySalesSQL
        {
            public DateTime SalesDate { get; set; }
            public int BookID { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class MonthlySalesSQL
        {
            public int SalesYear { get; set; }
            public int SalesMonth { get; set; }
            public int BookID { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class YearlySalesSQL
        {
            public int SalesYear { get; set; }
            public int BookID { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class AllTimeSalesSQL
        {
            public int SalesYear { get; set; }
            public int BookID { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }

        /// <summary>
        /// Updates variable that holds database store. 
        /// </summary>
        /// <returns></returns>
        public static List<BookInfo> updateDatastore()
        {
            // ^^^ static so object doesnt need to be instantiated to be called.
            List<BookInfo> mainDataSet;
            List<BookAuthor> authorList;
            List<BookGenre> genreList;
            List<DailySalesSQL> dailySalesSQL;
            List<MonthlySalesSQL> monthlySalesSQL;
            List<YearlySalesSQL> yearlySalesSQL;
            List<AllTimeSalesSQL> allTimeSalesSQL;

            using (IDbConnection dbConnection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                mainDataSet = dbConnection.Query<BookInfo>("spGetDatabaseTestProcedure").ToList();
                authorList = dbConnection.Query<BookAuthor>("spGetBookAuthor").ToList();
                genreList = dbConnection.Query<BookGenre>("spGetBookGenre").ToList();

                var results = dbConnection.QueryMultiple("spGetSalesAggregates");
                yearlySalesSQL = results.Read<YearlySalesSQL>().ToList();
                monthlySalesSQL = results.Read<MonthlySalesSQL>().ToList();
                dailySalesSQL = results.Read<DailySalesSQL>().ToList();
                allTimeSalesSQL = results.Read<AllTimeSalesSQL>().ToList();
            }

            // this bit perfroms a JOIN-like operation on genreList and authorList
            // it takes info like List:
            //    authorList[1]: AuthorName "Edwin Muir" BookID 2
            //    authorList[2]: AuthorName "Franz Kafka" BookID 2
            // and turns it into a dictionary of
            //    Key: BookID, Value: List of authors/genres
            Dictionary<int, List<string>> bookAuthorDictionary = authorList
            .GroupBy(x => x.BookID)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.AuthorName).ToList()
            );
            Dictionary<int, List<string>> bookGenreDictionary = genreList
            .GroupBy(x => x.BookID)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.GenreName).ToList()
            );


            // i wrote this ages ago i literally forgot how this works
            foreach (var bookInfo in mainDataSet)
            {
                int bookID = bookInfo.BookID;


                var allTimeSales = allTimeSalesSQL.Where(s => s.BookID == bookID && s.QuantitySold > 0).ToList();
                bookInfo.AllTimeSales = allTimeSales.Select(s => new AllTimeSales
                {
                    QuantitySold = s.QuantitySold,
                    Revenue = s.Revenue
                }).ToList();

                // Join YearlySalesSQL data
                var yearlySales = yearlySalesSQL.Where(s => s.BookID == bookID).ToList();
                bookInfo.YearlySales = yearlySales.Select(s => new YearlySales
                {
                    SalesYear = s.SalesYear,
                    QuantitySold = s.QuantitySold,
                    Revenue = s.Revenue
                }).ToList();

                // Join MonthlySalesSQL data
                var monthlySales = monthlySalesSQL.Where(s => s.BookID == bookID).ToList();
                bookInfo.MonthlySales = monthlySales.Select(s => new MonthlySales
                {
                    SalesYear = s.SalesYear,
                    SalesMonth = s.SalesMonth,
                    QuantitySold = s.QuantitySold,
                    Revenue = s.Revenue
                }).ToList();

                // Join DailySalesSQL data
                // daily sales does this annoying thing that even if it has no sales, because there is a datetime,
                // it'll still display on the datagrid as datetime: 0001 12am, quanSold 0, reven 0 instead of not having
                // anything like the others
                var dailySales = dailySalesSQL
                .Where(s => s.BookID == bookID && s.QuantitySold > 0) // Filter out entries with QuantitySold == 0
                .ToList();
                bookInfo.DailySales = dailySales.Select(s => new DailySales
                {
                    SalesDate = s.SalesDate,
                    QuantitySold = s.QuantitySold,
                    Revenue = s.Revenue
                }).ToList();

                // Matches up authors to their coresponding books. Likewise for genre.
                if (bookAuthorDictionary.ContainsKey(bookInfo.BookID))
                {
                    bookInfo.Authors = bookAuthorDictionary[bookInfo.BookID];
                    bookInfo.Genres = bookGenreDictionary[bookInfo.BookID];
                }
            }
            return mainDataSet;
        }
    }
}
