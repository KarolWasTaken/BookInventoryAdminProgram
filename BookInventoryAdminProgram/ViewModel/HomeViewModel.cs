using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Printing;
using System.Transactions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private byte[] _popularBookCover;
        public byte[] PopularBookCover
        {
            get
            {
                return _popularBookCover;
            }
            set
            {
                _popularBookCover = value;
                OnPropertyChanged(nameof(PopularBookCover));
            }
        }
        public Dictionary<string, bool> MoreInfoRestock { get; set; }
        public Dictionary<string, object> PopularBookInfo { get; set; }

        public Dictionary<string, string> NotiflicationPanelMessage { get; set; }

        public string LowInStockMessage { get; set; }
        public Dictionary<string, string> PopularPropertyLowInStockMessage { get; set; }

        public PopularityCalculator pc;
        public HomeViewModel()
        {
            pc = new PopularityCalculator(DatabaseStore.MainDataset);
            MoreInfoRestock = new Dictionary<string, bool>()
            {
                { "Genre", false }, {"Author", false}, { "Stock", false}
            };
            PopularPropertyLowInStockMessage = new Dictionary<string, string>()
            {
                {"Genre", null }, {"Author", null}
            };
            InitiliseBestSellerPanel();
            InitialiseNotiflicationPanel();
        }

        private void InitiliseBestSellerPanel()
        {
            // this prolly breaks mvvm principles but idc
            var bestSeller = pc.GetMostPopularBook(new DateTime(2023, 9, 12), new DateTime(2023, 10, 12));
            BookInfo popularBook = (BookInfo)bestSeller[0];
            PopularBookInfo = new Dictionary<string, object>()
            {
                { "Title", popularBook.Title},
                { "Authors", popularBook.Authors},
                { "Genres",  popularBook.Genres},
                { "ReleaseDate",  popularBook.ReleaseDate},
                { "Publisher",  popularBook.PublisherName},
                { "ISBN",  popularBook.ISBN},
                { "Sales", $"{bestSeller[1]} units sold this month"}
            };

            byte[] imageBytes;
            using (IDbConnection dbConnection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                var book = dbConnection.QueryFirst<BookInfo>("spGetBookCover @BookID", new { BookID = popularBook.BookID });
                imageBytes = book.BookCover;
            }
            if (imageBytes == null)
            {

                BitmapImage defaultNoCoverImageBitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/BookImages/NoCoverDefaultTwo.png"));
                byte[] defaultNoCoverImage = BitmapImageToByteArrayConverter.Convert(defaultNoCoverImageBitmap);
                PopularBookCover = defaultNoCoverImage;
            }
            else { PopularBookCover = imageBytes; }
            // Use the imageBytes to display the image or save it to a file.
        }
        private void InitialiseNotiflicationPanel()
        {
            var listOfMostPopularProperties = pc.GetAllPopulatities();
            DatabaseOperations dbo = new DatabaseOperations();


            NotiflicationPanelMessage = new Dictionary<string, string>()
            {
                { "Genre", $"• {dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][0].ID)}\n• {dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][1].ID)}\n• {dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][2].ID)}"},
                { "Author", $"• {dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][0].ID)}\n• {dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][1].ID)}\n• {dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][2].ID)}"},
                { "Publisher", $"• {dbo.GetPropertyByID("Publisher",listOfMostPopularProperties["Publisher"][0].ID)}\n• {dbo.GetPropertyByID("Publisher", listOfMostPopularProperties["Publisher"][1].ID)}\n• {dbo.GetPropertyByID("Publisher",listOfMostPopularProperties["Publisher"][2].ID)}"}
            };


            var booksLowInStock = dbo.GetBooksLowInStock();
            if (booksLowInStock != null)
            {
                booksLowInStock = booksLowInStock.Take(10).ToList();
                foreach (BookInfo book in booksLowInStock)
                {
                    LowInStockMessage += $"{book.Title}: {book.BookStock}\n";
                }
                LowInStockMessage += "\nConsider restocking!";
                MoreInfoRestock["Stock"] = true;
            }
            List<BookInfo> genresWithBooksLowInStock = dbo.GetBooksLowInStock("Genre", new List<string>()
            {
                dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][0].ID),
                dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][1].ID),
                dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][2].ID)
            });
            List<BookInfo> authorsWithBooksLowInStock = dbo.GetBooksLowInStock("Author", new List<string>()
            {
                dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][0].ID),
                dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][1].ID),
                dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][2].ID)
            });

            UpdateCategoryInfo(genresWithBooksLowInStock, "Genre");
            UpdateCategoryInfo(authorsWithBooksLowInStock, "Author");
        }
        private void UpdateCategoryInfo(List<BookInfo> booksLowInStock, string category)
        {
            if (booksLowInStock != null && booksLowInStock.Count > 0)
            {
                PopularPropertyLowInStockMessage[category] = $"[{booksLowInStock[0].Title}] is low and has an above {category}!";
                MoreInfoRestock[category] = true;
            }
        }

    }
}
