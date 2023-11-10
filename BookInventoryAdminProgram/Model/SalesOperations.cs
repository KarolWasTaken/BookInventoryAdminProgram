using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Model
{
    public class SalesOperations
    {
        public enum SalesQuarter
        {
            Q1,
            Q2,
            Q3,
            Q4
        }
        public enum BookPPU
        {
            PricePerUnit,
            SalePrice
        }
        private enum Date
        {
            StartDate,
            EndDate
        }


        /// <summary>
        /// Returns a dictionary of the corresponding quarter start and end dates
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Dictionary<Date, DateTime> GetQuarterDates(int year, SalesQuarter salesQuarter)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (salesQuarter)
            {
                case SalesQuarter.Q1:
                    startDate = new DateTime(year, 1, 1);
                    endDate = new DateTime(year, 3, 31);
                    break;
                case SalesQuarter.Q2:
                    startDate = new DateTime(year, 4, 1);
                    endDate = new DateTime(year, 6, 30);
                    break;
                case SalesQuarter.Q3:
                    startDate = new DateTime(year, 7, 1);
                    endDate = new DateTime(year, 9, 30);
                    break;
                case SalesQuarter.Q4:
                    startDate = new DateTime(year, 10, 1);
                    endDate = new DateTime(year, 12, 31);
                    break;
            }
            return new Dictionary<Date, DateTime>()
            {
                {Date.StartDate, startDate },
                {Date.EndDate, endDate }
            };
        }
        /// <summary>
        /// Gets total price spent on units in the provided year for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double GetPriceOnUnitsSpent(int year, BookInfo book)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            return GetPriceOnUnitsSpent(new DateTime(year, 1, 1), new DateTime(year, 12, 31), book);
        }
        /// <summary>
        /// Gets total price spent on units in the provided quarter for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetPriceOnUnitsSpent(int year, SalesQuarter salesQuarter, BookInfo book)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            return GetPriceOnUnitsSpent(dates[Date.StartDate], dates[Date.EndDate], book);
        }
        /// <summary>
        /// Gets total price spent on units in the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetPriceOnUnitsSpent(DateTime date, BookInfo book)
        {
            return GetPriceOnUnitsSpent(date, date, book);
        }
        /// <summary>
        /// Gets total price spent on units within the provided dates for the provided book
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetPriceOnUnitsSpent(DateTime startDate, DateTime endDate, BookInfo book)
        {
            // not the best way to do this but it works.
            double totalRevenue = GetRevenue(startDate, endDate, book);
            double totalProfit = GetProfit(startDate, endDate, book);

            return totalRevenue - totalProfit;
        }
        /// <summary>
        /// Returns true if sales exist in the provided year for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool SalesExists(int year, BookInfo book)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            return SalesExists(new DateTime(year, 1, 1), new DateTime(year, 12, 31), book);
        }
        /// <summary>
        /// Returns true if sales exist in the provided quarter for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static bool SalesExists(int year, SalesQuarter salesQuarter, BookInfo book)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            return SalesExists(dates[Date.StartDate], dates[Date.EndDate], book);
        }
        /// <summary>
        /// Returns true if sales exist in the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static bool SalesExists(DateTime date, BookInfo book)
        {
            return SalesExists(date, date, book);
        }
        /// <summary>
        /// Returns true if sales exist within the provded dates for the provided book
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static bool SalesExists(DateTime startDate, DateTime endDate, BookInfo book)
        {
            if (book.DailySales == null)
            {
                return false;
            }
            if (GetQuantitySold(startDate, endDate, book) > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Gets profits of sales in the provided year for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double GetProfit(int year, BookInfo book)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            return GetProfit(new DateTime(year, 1, 1), new DateTime(year, 12, 31), book);
        }
        /// <summary>
        /// Gets profits of sales in the provided quarter for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetProfit(int year, SalesQuarter salesQuarter, BookInfo book)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            return GetProfit(dates[Date.StartDate], dates[Date.EndDate], book);
        }
        /// <summary>
        /// Gets profits of sales in the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetProfit(DateTime date, BookInfo book)
        {
            return GetProfit(date, date, book);
        }
        /// <summary>
        /// Gets profits of sales within the provided dates for the provided book
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetProfit(DateTime startDate, DateTime endDate, BookInfo book)
        {
            double totalRevenue = GetRevenue(startDate, endDate, book);
            //double totalPricePerUnit = 0;
            int totalQuantitySold = 0;
            double totalProfit = 0;
            // Loop through each day from start to end date
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                int quantitySold = GetQuantitySold(date, date, book);
                if (quantitySold > 0)
                {
                    totalProfit += GetRevenue(date, book) - (GetPPUAttribute(date, BookPPU.PricePerUnit, book) * quantitySold);
                    totalQuantitySold += quantitySold;
                }
                // Access and print the current date
                //Console.WriteLine("Current Date: " + date.ToString("yyyy-MM-dd"));
            }
            if (totalQuantitySold > 0)
            {
                return Math.Round(totalProfit, 2);
            }
            return 0;
        }
        /// <summary>
        /// Gets revenue in the provided year for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double GetRevenue(int year, BookInfo book)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            return GetRevenue(new DateTime(year, 1, 1), new DateTime(year, 12, 31), book);
        }
        /// <summary>
        /// Gets revenue in the provided quarter for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetRevenue(int year, SalesQuarter salesQuarter, BookInfo book)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            return GetRevenue(dates[Date.StartDate], dates[Date.EndDate], book);
        }
        /// <summary>
        /// Gets revenue in the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetRevenue(DateTime date, BookInfo book)
        {
            return GetRevenue(date, date, book);
        }
        /// <summary>
        /// Gets revenue within the provided dates for the provided book
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static double GetRevenue(DateTime startDate, DateTime endDate, BookInfo book)
        {
            if (book.DailySales == null)
            {
                return 0;
            }
            double totalRevenue = 0;


            foreach (var dailySale in book.DailySales)
            {
                // Check if the SalesDate is within the specified date range
                if (dailySale.SalesDate >= startDate && dailySale.SalesDate <= endDate)
                {
                    // Add the revenue for this DailySale to the total revenue
                    totalRevenue += dailySale.Revenue;
                }
            }

            return totalRevenue;
        }
        /// <summary>
        /// Gets quantity sold in the provided year for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetQuantitySold(int year, BookInfo book)
        {
            if (year.ToString().Length != 4)
                throw new Exception("Year is invalid");
            return GetQuantitySold(new DateTime(year, 1, 1), new DateTime(year, 12, 31), book);
        }
        /// <summary>
        /// Gets quantity sold in the provided quarter for the provided book
        /// </summary>
        /// <param name="year"></param>
        /// <param name="salesQuarter"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static int GetQuantitySold(int year, SalesQuarter salesQuarter, BookInfo book)
        {
            Dictionary<Date, DateTime> dates = GetQuarterDates(year, salesQuarter);
            return GetQuantitySold(dates[Date.StartDate], dates[Date.EndDate], book);
        }
        /// <summary>
        /// Gets quantity sold in the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static int GetQuantitySold(DateTime date, BookInfo book)
        {
            return GetQuantitySold(date, date, book);
        }
        /// <summary>
        /// Gets quantity sold within the provided dates for the provided book
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public static int GetQuantitySold(DateTime startDate, DateTime endDate, BookInfo book)
        {
            if (book.DailySales == null)
            {
                return 0;
            }
            int totalQuantitySold = 0;


            foreach (var dailySale in book.DailySales)
            {
                // Check if the SalesDate is within the specified date range
                if (dailySale.SalesDate >= startDate && dailySale.SalesDate <= endDate)
                {
                    // Add the revenue for this DailySale to the total revenue
                    totalQuantitySold += dailySale.QuantitySold;
                }
            }

            return totalQuantitySold;
        }
        /// <summary>
        /// Gets <see cref="BookPPU"/> attribute at the provided date for the provided book
        /// </summary>
        /// <param name="date"></param>
        /// <param name="attributeType"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double GetPPUAttribute(DateTime date, BookPPU attributeType, BookInfo book)
        {
            if (attributeType != BookPPU.PricePerUnit && attributeType != BookPPU.SalePrice)
                throw new Exception("attributeType not recognised");

            double result = 0;
            // Extract the date part of the input date
            DateTime dateWithoutTime = date.Date;

            foreach (var pricePerUnit in book.PricePerUnit)
            {
                // Extract the date part of SetDate for comparison
                DateTime setDateWithoutTime = pricePerUnit.SetDate.Date;

                // Check if the SetDate is on or after the specified date
                if (setDateWithoutTime >= dateWithoutTime)
                {
                    switch (attributeType)
                    {
                        case BookPPU.PricePerUnit:
                            result = pricePerUnit.PricePerUnit;
                            return result;
                        case BookPPU.SalePrice:
                            result = pricePerUnit.SalePrice;
                            return result;
                    }
                    break; // I prolly dont need this here but ill keep it anyway
                }
                else if (dateWithoutTime >= setDateWithoutTime)
                {
                    // here there exists no value at the time because the ppu price was set before the date we entered. 
                    // in that case, we return the most up to date price
                    switch (attributeType)
                    {
                        case BookPPU.PricePerUnit:
                            return book.PricePerUnit.OrderByDescending(ppu => ppu.SetDate).FirstOrDefault().PricePerUnit;
                        case BookPPU.SalePrice:
                            return book.PricePerUnit.OrderByDescending(ppu => ppu.SetDate).FirstOrDefault().SalePrice;
                    }
                }
            }

            throw new Exception($"{attributeType} value doesnt exist at {dateWithoutTime.ToString("dd/MM/yyyy")}");
        }
    }
}   
