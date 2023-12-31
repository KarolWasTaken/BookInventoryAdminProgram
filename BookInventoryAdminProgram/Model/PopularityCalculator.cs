using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Dapper;
using static BookInventoryAdminProgram.Stores.DatabaseStore;
using static System.Reflection.Metadata.BlobBuilder;
using AdonisUI.Controls;
using System.Reflection;

namespace BookInventoryAdminProgram.Model
{
    public class PopularityCalculator
    {
        private static List<BookInfo> _database;
        public PopularityCalculator(List<BookInfo> database)
        {
            _database = database;
        }
        public class SQLPopularity
        {
            public int ID { get; set; }
            public int TotalSales { get; set; }
            public DateTime PopularityDate { get; set; }
        }
        /// <summary>
        /// Gets the 3 most popular Books, Authors, Genres, and Publishers in the last year.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<SQLPopularity>> GetAllPopulatities()
        {
            List<SQLPopularity> bookPopularity;
            List<SQLPopularity> authorPopularity;
            List<SQLPopularity> genrePopularity;
            List<SQLPopularity> publisherPopularity;
            
            using (IDbConnection dbConnection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                var results = dbConnection.QueryMultiple("spGetSalesPopularities");
                bookPopularity = results.Read<SQLPopularity>().ToList();
                authorPopularity = results.Read<SQLPopularity>().ToList();
                genrePopularity = results.Read<SQLPopularity>().ToList();
                publisherPopularity = results.Read<SQLPopularity>().ToList();
            }
            bool isDataInvalid = bookPopularity == null || authorPopularity == null || genrePopularity == null || publisherPopularity == null;
            if (isDataInvalid)
            {
                MessageBox.Show("Database returned null values.", "Fatal Error", MessageBoxButton.OK);
                throw new Exception("Database returned null values. Fatal error.");
            }

            List<SQLPopularity> bookPopularityWithMaxSalesLastMonth = GetPopularityWithMaxSalesLastMonthPlus(bookPopularity);
            List<SQLPopularity> authorPopularityWithMaxSalesLastMonth = GetPopularityWithMaxSalesLastMonthPlus(authorPopularity);
            List<SQLPopularity> genrePopularityWithMaxSalesLastMonth = GetPopularityWithMaxSalesLastMonthPlus(genrePopularity);
            List<SQLPopularity> publisherPopularityWithMaxSalesLastMonth = GetPopularityWithMaxSalesLastMonthPlus(publisherPopularity);


            return new Dictionary<string, List<SQLPopularity>>()
            {
                {"Book", bookPopularityWithMaxSalesLastMonth},
                {"Author", authorPopularityWithMaxSalesLastMonth},
                {"Genre", genrePopularityWithMaxSalesLastMonth},
                {"Publisher", publisherPopularityWithMaxSalesLastMonth}
            };
        }

        /// <summary>
        /// This method widens it's search area dynamically for up to a year.
        /// </summary>
        /// <param name="popularityList"></param>
        /// <param name="currentDay"></param>
        /// <param name="currentDayLastMonth"></param>
        /// <returns></returns>
        private List<SQLPopularity> GetPopularityWithMaxSalesLastMonthPlus(List<SQLPopularity> popularityList) 
        {
            // Gets current day and current day last month. Datetime automatically solves issues like: March 30 --(minus 1 month)--> Feb 28
            DateTime currentDay = DateTime.Now;
            DateTime TrueCurrentDayLastMonth = currentDay.AddMonths(-1);
            
            DateTime localCurrentDayLastMonth = TrueCurrentDayLastMonth;
            int loopCount = 0;
            List<SQLPopularity> topThreePopularity = new List<SQLPopularity>();

            while (topThreePopularity.Count < 3)
            {
                if (loopCount == 0)
                    localCurrentDayLastMonth = TrueCurrentDayLastMonth;
                if (loopCount >= 13)
                    break;
                var popularity = popularityList
                    .Where(p => ((dynamic)p).PopularityDate >= localCurrentDayLastMonth && ((dynamic)p).PopularityDate <= currentDay) // Applies timespan constraints
                    .OrderByDescending(p => ((dynamic)p).TotalSales)
                    .Where(p => !topThreePopularity.Contains(p)) // Skip elements already added
                    .FirstOrDefault();

                if (popularity != null && !topThreePopularity.Contains(popularity))
                    topThreePopularity.Add(popularity);
                else
                    localCurrentDayLastMonth = localCurrentDayLastMonth.AddMonths(-1); // If no elements was found, add 1 month back more to widen timespan

                loopCount++;
            }

            return topThreePopularity;
        }



        /// <summary>
        /// returns most popular book between specified timespan
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>book (index 0) and total quantity sold in timespan (index 1)</returns>
        /// <exception cref="Exception"></exception>
        public List<object>? GetMostPopularBook(DateTime startDate, DateTime endDate)
        {
            if(startDate > endDate)
            {
                MessageBox.Show("startDate is larger than endDate.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("startDate is larger than endDate. Fatal Error.");
            }

            
            //_database = updateDatastore(); debug
            // Get books that have experienced any sales in that time period
            List<BookInfo> filteredBooks = _database.Where(book => book.DailySales
            .Any(dailySale => dailySale.SalesDate >= startDate && dailySale.SalesDate <= endDate)).ToList();

            // First, calculate the maximum quantity sold for each book in the specified time period.
            var booksWithMaxQuantity = filteredBooks.Select(book => new
            {
                Book = book,
                MaxQuantitySold = book.DailySales
                    .Where(dailySale =>
                        dailySale.SalesDate >= startDate && dailySale.SalesDate <= endDate
                    )
                    .Max(dailySale => dailySale.QuantitySold)
            });

            // Then, find the book with the highest maximum quantity sold.

            // i dunno if  wanna have a list of most popular books or just grab firstordefault
            //var maxQuantity = booksWithMaxQuantity.Max(x => x.MaxQuantitySold);
            //var booksWithHighestQuantity = booksWithMaxQuantity.Where(x => x.MaxQuantitySold == maxQuantity).ToList();
            
            var bookWithHighestQuantity = booksWithMaxQuantity.OrderByDescending(x => x.MaxQuantitySold).FirstOrDefault();
            // Check if there's a book with the highest quantity sold and access it.
            if (bookWithHighestQuantity != null)
            {
                BookInfo book = bookWithHighestQuantity.Book;
                int maxQuantitySold = bookWithHighestQuantity.MaxQuantitySold;

                //MessageBox.Show($"Book with the highest quantity sold:\n{book.Title}\nQuantity Sold: {maxQuantitySold}");
                return new List<object>() {book, maxQuantitySold};
            }
            else
            {
                //MessageBox.Show("No books found in the specified date range.");
                return null;
            }
        }






                                                                                        
                                                                                
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@.@@%,.  #%#@%@@@@@@@@@#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@ @&%%,/.@@((@&.(&&%@@@&*/@@*@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@ @ .& .@ %@   ,. / .*( @%&.@@@@%((&@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@ @ ,#*(, @@%&@. @. @.   & @@@@@@@@@@@,@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@( ,/,,,.#*/ ,% @..  @  @ .&%&@@@@@@@@@@@(@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@*.@(*#(/ @( &( ,  %@@&%&( @@@@@@@@@@@@@@@@ @@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@&(,##,  (,%.% @, . ,% #, @@@@@@@@@@@@@@@@@@@&.@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@..#@..@,#./.# ,#%/  ( @@@@@@@@@@@@@@@@@@@@&%@&%@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@%,.  / ,&@ @ *(. %@ (@ @@@@@@@@@@@@@@@@@@@@@.@@@ @@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@(@ % .@@.@*%%  @@ *@ @  @@@@@@@@@@@@@@@@@@@/@@@@*@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@ &@, ,(@(@. *.# @*.@%#.@ /#&@@@@@@@@@&(@@@@@@@@@%@@@@@@@@@@@@@@@@@@@@@@     i forgot why i made this
        //@@@@@@@@%%* @ %#. .@( @  @&#,.&  . @#&& @@. ,.@,.@&(@@@@@&#@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@& .*. .%&# @ @ & , .  . # ,.*/,%%/.@  @,&.,@@@@@ #@&@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@&.   @ %@ /*, *,(  ,    ,.  ,.# ##. @, &*&& @@@@(, *@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@   @ &# .# %. . %#.   @  . (*,,, #*,,&*,. .#@@@@#(&.%@@@@@@@@@@@@@@@@@@ 
        //@@@@@@@@   #    . % ./%& ..,%&  ,. ,@@@@ #.,*(, /@@@.@@@@./. &@@@@@@@@@@@@@@@@@@
        //@@@@@@# @ ,   .     %.*.*.%  /,*,.. @@@@@@@@@&@@@@@@@@@@..*# @@@@@@@@@@@@@@@@@@@
        //@@@@@@@/  #    *   , #/, /( # &,,.,/@@@@@@@@@@@@@@@@@@@  /*( @@@@@@@@@@@@@@@@@@@
        //@@@@@@@@  ,  &,  %  (.,   , & .* ..&@@@@@@@@@@@@@@@@@. %,,/,*@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@.%@.  @&&&/ % @@ @ *@ & .   ,(,(@@@@@@@@@@ @@ &.(,&@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@(..@   %*. ,, & .@ @ #*(& % &@@@@@@@@@@@#&(* %%/@@#@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@#, (@ @, .*.# ,/..%,, .@. @@@@@@@@@@@@%&%@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@ @ , @ @ ..*/..#@ #.*#,.@%.@@@@@@@@@@@@&@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@%./ , @&. @& /% ,..% @@@@@@@@@@@@@@#@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@&.%,  @& & &/ @(( @@@@@@@@@@@@@@@@@(@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@ &@@@   @/*@ @*,*@@@@@@@,*@@@@@(.%#@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@ @@@@@(. #* ,/# , @@@%@@@@@%.&@&,@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@%(/@(@.  *@##@@@**,. @&(,%&@,/&%%%%,@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@   @.  @@(.@@@ ,*,.,  #@@(#@@&@//@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@..  &/. @.@*&    .,%@@@@/(@@@@@**&@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@*(.@ ..#* &.&.  @@*,&@@@%(&@@@@@@@,@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@*. @,.,.*/@%,&@@@@@@@@@@@@@@@@@&@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@ @@@  @...%@ @* &@@@@@@@@@@@@@@@@@% @@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@* @ *.@@,# @@ @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@/ @@ @@@*%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                                        

        public Dictionary<BookInfo, int> GetQuantitySoldLastMonth()
        {
            // Assuming MonthlySales contains the sales data for the last month.
            var popularityDict = new Dictionary<BookInfo, int>();

            for (int i = 0; i < _database.Count; i++)
            {
                // Get Last Month's sales
                var lastMonthSales = _database[i].MonthlySales?.LastOrDefault();

                // Calculate popularity based on total sales for the last month.
                int popularity = lastMonthSales != null ? lastMonthSales.QuantitySold : 0;
                popularityDict[_database[i]] = popularity;
            }

            // Sort the dictionary by popularity in numerical order.
            var sortedDict = popularityDict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return sortedDict;
        }

        public Dictionary<BookInfo, int> GetPopularityListLastMonth()
        {
            var sortedDict = GetQuantitySoldLastMonth();
            // Creates final dict of the actual popularity list
            var finalDict = new Dictionary<BookInfo, int>();
            for (int i = 0; i <= sortedDict.Count - 1; i++)
            {
                finalDict[sortedDict.Keys.ElementAt(i)] = i + 1;
            }

            return finalDict;
        }


    }
}
