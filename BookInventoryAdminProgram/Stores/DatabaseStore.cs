using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Net;

namespace BookInventoryAdminProgram.Stores
{
    public class DatabaseStore
    {



        private static Dictionary<string, List<CommonValues>>? junctionValuesDictionary;
        /// <summary>
        /// Dictionary that maps the IDs of properties <strong>(like AuthorID)</strong> to their values <strong>(like AuthorName)</strong>
        /// <para>Setter runs <seealso cref="UpdateDatastore"/></para>
        /// </summary>
        public static Dictionary<string, List<CommonValues>>? JunctionValuesDictionary 
        {
            get
            {
                UpdateDatastore();
                return junctionValuesDictionary;
            }
            set { junctionValuesDictionary = value; }
        }

        private static List<BookInfo>? mainDataset;
        /// <summary>
        /// Property that holds all <see cref="BookInfo"/> values for all books.
        /// <para>Setter runs <seealso cref="UpdateDatastore"/></para>
        /// </summary>
        public static List<BookInfo>? MainDataset 
        {
            get 
            {
                UpdateDatastore();
                return mainDataset; 
            }
            set { mainDataset = value; }
        } 

        /// <summary>
        /// Class that stores database data for books
        /// </summary>
        public class BookInfo
        {
            public string ISBN { get; set; }
            public int BookID { get; set; }
            public string Title { get; set; }
            public double Price { get; set; }
            public List<PricePerUnitCollection> PricePerUnit { get; set; }
            public int BookStock { get; set; }
            public List<string> Authors { get; set; }
            public List<string> Genres { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public string PublisherName { get; set; }
            public byte[]? BookCover { get; set; }
            public List<AllTimeSales>? AllTimeSales { get; set; }
            public List<YearlySales>? YearlySales { get; set; }
            public List<MonthlySales>? MonthlySales { get; set; }
            public List<DailySales>? DailySales { get; set; }
        }
        public class PricePerUnitCollection
        {
            public DateTime SetDate { get; set; }
            public double PricePerUnit { get; set; }
            public double SalePrice { get; set; }
        }
        public class SalesJunctionBase
        {
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
        public class DailySales : SalesJunctionBase
        {
            public DateTime SalesDate { get; set; }
        }
        public class MonthlySales : SalesJunctionBase
        {
            public int SalesYear { get; set; }
            public int SalesMonth { get; set; }
        }
        public class YearlySales : SalesJunctionBase
        {
            public int SalesYear { get; set; }
        }
        public class AllTimeSales : SalesJunctionBase { }





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

        public class PricePerUnitCollectionSQL
        {
            public int BookID { get; set; }
            public DateTime SetDate { get; set; }
            public double PricePerUnit { get; set; }
            public double SalePrice { get; set; }
        }
        public class SalesJunctionSQLBase
        {
            public int BookID { get; set; }
            public int QuantitySold { get; set; }
            public double Revenue { get; set; }
        }
            
        public class DailySalesSQL : SalesJunctionSQLBase
        {
            public DateTime SalesDate { get; set; }
        }
        public class MonthlySalesSQL : SalesJunctionSQLBase
        {
            public int SalesYear { get; set; }
            public int SalesMonth { get; set; }
        }
        public class YearlySalesSQL : SalesJunctionSQLBase
        {
            public int SalesYear { get; set; }
        }
        public class AllTimeSalesSQL : SalesJunctionSQLBase
        {
            public int SalesYear { get; set; }
        }

        /// <summary>
        /// <para><strong>spGetJunctionValueTables</strong> returns 3 tables that all have values 'ID', and 'Name'</para>
        /// <para>These are tables for Author, Genre, and Publisher (which are all keys)</para>
        /// </summary>
        public class CommonValues
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }


        /// <summary>
        /// Updates <strong><see cref="JunctionValuesDictionary"/></strong> and <strong><see cref="MainDataset"/></strong> with new up-to-date database infomation. 
        /// </summary>
        /// <returns></returns>
        private static void UpdateDatastore()
        {
            // ^^^ static so object doesnt need to be instantiated to be called.
            List<BookInfo> mainDataSet;
            List<PricePerUnitCollectionSQL> pricePerUnitCollectionSQL;
            List<BookAuthor> authorList;
            List<BookGenre> genreList;
            Dictionary<string, List<CommonValues>> junctionValuesDictionary;
            List<DailySalesSQL> dailySalesSQL;
            List<MonthlySalesSQL> monthlySalesSQL;
            List<YearlySalesSQL> yearlySalesSQL;
            List<AllTimeSalesSQL> allTimeSalesSQL;

            using (IDbConnection dbConnection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                mainDataSet = dbConnection.Query<BookInfo>("spGetDatabaseTestProcedure").ToList();
                pricePerUnitCollectionSQL = dbConnection.Query<PricePerUnitCollectionSQL>("spGetBookPPU").ToList();
                authorList = dbConnection.Query<BookAuthor>("spGetBookAuthor").ToList();
                genreList = dbConnection.Query<BookGenre>("spGetBookGenre").ToList();

                var results2 = dbConnection.QueryMultiple("spGetJunctionValueTables");
                junctionValuesDictionary = new Dictionary<string, List<CommonValues>>()
                {
                    {"Author", results2.Read<CommonValues>().ToList() },
                    {"Genre", results2.Read<CommonValues>().ToList() },
                    {"Publisher", results2.Read<CommonValues>().ToList() },
                    {"Book", results2.Read<CommonValues>().ToList() }
                };

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
            // Create a dictionary to map BookID to a list of PricePerUnitCollection
            Dictionary<int, List<PricePerUnitCollection>> pricePerUnitDictionary = pricePerUnitCollectionSQL
            .GroupBy(ppu => ppu.BookID)
            .ToDictionary(
                group => group.Key,
                group => group.Select(ppu => new PricePerUnitCollection
                {
                    SetDate = ppu.SetDate,
                    PricePerUnit = ppu.PricePerUnit,
                    SalePrice = ppu.SalePrice
                }).ToList()
            );


            foreach (var bookInfo in mainDataSet)
            {
                int bookID = bookInfo.BookID;

                // Iterate through mainDataSet and assign the PricePerUnitCollection

                if (pricePerUnitDictionary.TryGetValue(bookInfo.BookID, out List<PricePerUnitCollection> pricePerUnitList))
                {
                    bookInfo.PricePerUnit = pricePerUnitList;
                }

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
            JunctionValuesDictionary = junctionValuesDictionary;
            MainDataset = mainDataSet;
            //return mainDataSet; relic
        }
    }
}
