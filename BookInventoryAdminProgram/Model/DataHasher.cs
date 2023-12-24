using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Model
{
    public class DataHasher
    {
        public enum CollectionType
        {
            PricePerUnit,
            AllTimeSales,
            YearlySales,
            MonthlySales,
            DailySales
        }

        /// <summary>
        /// Returns hash for all books
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CalculateHash(List<BookInfo> data)
        {
            if (data == null)
                throw new Exception("Null data sent to calcualte hash");

            using (SHA256 sha256 = SHA256.Create())
            {
                List<byte> allBytes = new List<byte>();
                foreach (var book in data)
                {
                    allBytes.AddRange(returnBytesForBookObject(book));
                }
                byte[] combinedData = allBytes.ToArray();
                byte[] hashBytes = sha256.ComputeHash(combinedData);
                            
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        /// <summary>
        /// Hashes specific book
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CalculateHash(BookInfo data)
        {
            if (data == null)
                throw new Exception("Null data sent to calcualte hash");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(returnBytesForBookObject(data).ToArray());
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private static List<byte> returnBytesForBookObject(BookInfo book)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Encoding.UTF8.GetBytes(book.ISBN));
            bytes.AddRange(Encoding.UTF8.GetBytes(book.BookID.ToString()));
            bytes.AddRange(Encoding.UTF8.GetBytes(book.Title));
            bytes.AddRange(Encoding.UTF8.GetBytes(book.Price.ToString()));
            bytes.AddRange(Encoding.UTF8.GetBytes(CollectionToString(book.PricePerUnit, CollectionType.PricePerUnit)));
            bytes.AddRange(Encoding.UTF8.GetBytes(book.BookStock.ToString()));
            bytes.AddRange(Encoding.UTF8.GetBytes(book.PublisherName.ToString()));
            if (book.BookCover != null)
                bytes.AddRange(book.BookCover);
            bytes.AddRange(Encoding.UTF8.GetBytes(CollectionToString(book.AllTimeSales, CollectionType.AllTimeSales)));
            bytes.AddRange(Encoding.UTF8.GetBytes(CollectionToString(book.YearlySales, CollectionType.YearlySales)));
            bytes.AddRange(Encoding.UTF8.GetBytes(CollectionToString(book.MonthlySales, CollectionType.MonthlySales)));
            bytes.AddRange(Encoding.UTF8.GetBytes(CollectionToString(book.DailySales, CollectionType.DailySales)));
            return bytes;
        }
        private static string CollectionToString(object collection, CollectionType collectionType)
        {
            if (collection == null)
                return null;

            string stringResult = string.Empty;
            switch (collectionType)
            {
                case CollectionType.PricePerUnit:
                    List<PricePerUnitCollection> ppu = collection as List<PricePerUnitCollection>;
                    foreach (var element in ppu)
                    {
                        stringResult += $"{element.SetDate}-{element.PricePerUnit}-{element.SalePrice}";
                    }
                    break;
                case CollectionType.AllTimeSales:
                    List<AllTimeSales> ats = collection as List<AllTimeSales>;
                    foreach(var element in ats)
                    {
                        stringResult += $"{element.QuantitySold}-{element.Revenue}";
                    }
                    break;
                case CollectionType.YearlySales:
                    List<YearlySales> ys = collection as List<YearlySales>;
                    foreach(var element in ys)
                    {
                        stringResult += $"{element.SalesYear}-{element.QuantitySold}-{element.Revenue}";
                    }
                    break;
                case CollectionType.MonthlySales:
                    List<MonthlySales> ms = collection as List<MonthlySales>;
                    foreach (var element in ms)
                    {
                        stringResult += $"{element.SalesYear}-{element.SalesMonth}-{element.QuantitySold}-{element.Revenue}";
                    }
                    break;
                case CollectionType.DailySales:
                    List<DailySales> ds = collection as List<DailySales>;
                    foreach (var element in ds)
                    {
                        stringResult += $"{element.SalesDate}-{element.QuantitySold}-{element.Revenue}";
                    }
                    break;
            }
            return stringResult.Replace(" ","");
        }
    }
}
