using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public Dictionary<string, object> PopularBookInfo { get; set; }

        public Dictionary<string, string> NotiflicationPanelMessage { get; set; }


        public PopularityCalculator pc;
        public HomeViewModel()
        {
            pc = new PopularityCalculator(DatabaseStore.MainDataset);
            InitiliseBestSellerPanel();
            var listOfMostPopularProperties = pc.GetAllPopulatities();
            DatabaseOperations dbo = new DatabaseOperations();

            //var test2 = new DatabaseOperations();
            //test2.GetBookByID(5);
            //var test2 = listOfMostPopularProperties["Author"][0].ID;
            //System.Windows.MessageBox.Show(dbo.GetPropertyByID("Author", test2));

            //var test3 = test2.

            NotiflicationPanelMessage = new Dictionary<string, string>()
            {
                { "Genre", $"{dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][0].ID)}, {dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][1].ID)}, and {dbo.GetPropertyByID("Genre",listOfMostPopularProperties["Genre"][2].ID)} are all popular!"},
                { "Author", $"{dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][0].ID)}, {dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][1].ID)}, and {dbo.GetPropertyByID("Author",listOfMostPopularProperties["Author"][2].ID)} are all popular!"},
                { "Publisher", $"{dbo.GetPropertyByID("Publisher",listOfMostPopularProperties["Publisher"][0].ID)}, {dbo.GetPropertyByID("Publisher",listOfMostPopularProperties["Publisher"][1].ID)}, and {dbo.GetPropertyByID("Publisher",listOfMostPopularProperties["Publisher"][2].ID)} are all popular!"}
            };
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
    }
}
