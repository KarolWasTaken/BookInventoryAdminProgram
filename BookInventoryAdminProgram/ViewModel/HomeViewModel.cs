using AdonisUI.Controls;
using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        public HomeViewModel()
        {
            PopularityCalculator pc = new PopularityCalculator(DatabaseStore.updateDatastore());
            var test = pc.getPopularity(new DateTime(2023, 9, 12), new DateTime(2023, 10, 12));
            BookInfo popularBook = (BookInfo)test[0];
            //MessageBox.Show(book.Title);
            PopularBookInfo = new Dictionary<string, object>()
            {
                { "Title", popularBook.Title},
                { "Authors", popularBook.Authors},
                { "Genres",  popularBook.Genres},
                { "ReleaseDate",  popularBook.ReleaseDate},
                { "Publisher",  popularBook.PublisherName},
                { "ISBN",  popularBook.ISBN},
                { "Sales", $"{test[1]} units sold this month"}
            };

            using (IDbConnection dbConnection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                var book = dbConnection.QueryFirst<BookInfo>("spGetBookCover @BookID", new { BookID = popularBook.BookID });
                byte[] imageBytes = book.BookCover;
                if (imageBytes == null) 
                {

                    BitmapImage defaultNoCoverImageBitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/BookImages/NoCoverDefault.png"));
                    byte[] defaultNoCoverImage = BitmapImageToByteArrayConverter.Convert(defaultNoCoverImageBitmap);
                    PopularBookCover = defaultNoCoverImage;
                }
                else { PopularBookCover = imageBytes; }
                // Use the imageBytes to display the image or save it to a file.
            }

        }
    }
}
