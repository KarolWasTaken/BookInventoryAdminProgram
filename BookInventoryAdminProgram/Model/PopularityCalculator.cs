using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Model
{
    public class PopularityCalculator
    {
        private static List<BookInfo> _database;
        public PopularityCalculator(List<BookInfo> database)
        {
            _database = database;
        }

        /// <summary>
        /// Gets Most popular book between specified timespan
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>book (index 0) and total quantity sold in timespan (index 1)</returns>
        /// <exception cref="Exception"></exception>
        public List<object>? getPopularity(DateTime startDate, DateTime endDate)
        {

            if(startDate > endDate)
            {
                throw new Exception("startDate is larger than endDate");
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

    }
}
