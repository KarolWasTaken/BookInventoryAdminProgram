using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public static class Formatters
    {
        /// <summary>
        /// Converts negatives to the stardard financial format: ([number])
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToFinancialNegative(this double number)
        {
            return NegativeParser(number).ToString();
        }
        /// <summary>
        /// Converts negatives to the stardard financial format: ([number])
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToFinancialNegative(this int number)
        {
            return NegativeParser(Convert.ToDouble(number)).ToString();
        }

        private static string NegativeParser(double number)
        {
            if (number < 0) // in finance, you write (120) instead of -120
            {
                return $"({Math.Round(number * -1, 2)})";
            }
            else
            {
                return Math.Round(number, 2).ToString();
            }
        }

        public static string GetMonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    throw new Exception("Invalid month number");
            }
        }
        public static string GetNumberExtension(int number)
        {
            switch (number)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        public static int Quarter(this DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }
    }
}
