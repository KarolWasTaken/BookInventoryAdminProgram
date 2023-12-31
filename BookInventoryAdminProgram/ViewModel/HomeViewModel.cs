﻿using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Media.Imaging;
using static BookInventoryAdminProgram.Model.PopularityCalculator;
using static BookInventoryAdminProgram.Stores.DatabaseStore;
using static BookInventoryAdminProgram.Model.BookCoverImageProcesses;
using AdonisUI.Controls;

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
        public HomeViewModel()
        {
            var pc = new PopularityCalculator(DatabaseStore.MainDataset);
            MoreInfoRestock = new Dictionary<string, bool>()
            {
                { "Genre", false }, {"Author", false}, { "Stock", false}
            };
            PopularPropertyLowInStockMessage = new Dictionary<string, string>()
            {
                {"Genre", null }, {"Author", null}
            };

            // I could probabaly move this into a Model file. I potentially should. Ill do this later
            InitiliseBestSellerPanel(pc);
            InitialiseNotiflicationPanel(pc);
        }

        private void InitiliseBestSellerPanel(PopularityCalculator pc)
        {
            // this prolly breaks mvvm principles but idc
            DateTime currentDay = DateTime.Now;
            DateTime currentDayLastMonth = currentDay.AddMonths(-1);
            var bestSeller = pc.GetMostPopularBook(currentDayLastMonth, currentDay);
            if (bestSeller == null)
            {
                PopularBookCover = GetNoCoverImage();
                PopularBookInfo = new Dictionary<string, object>()
                {
                    { "Title", "No book found"},
                    { "Authors", string.Empty},
                    { "Genres",  string.Empty},
                    { "ReleaseDate",  string.Empty},       // empty strings to prevent binding errors.
                    { "Publisher",  string.Empty},
                    { "ISBN",  string.Empty},
                    { "Sales", string.Empty}
                };
                return;
            }
            
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
                PopularBookCover = GetNoCoverImage();
            else 
                PopularBookCover = imageBytes; 
            // Use the imageBytes to display the image or save it to a file.
        }
        private void InitialiseNotiflicationPanel(PopularityCalculator pc)
        {
            Dictionary<string, List<SQLPopularity>> listOfMostPopularProperties = pc.GetAllPopulatities();
            DatabaseOperations dbo = new DatabaseOperations();

            NotiflicationPanelMessage = new Dictionary<string, string>()
            {
                { "Genre", string.Empty},
                { "Author", string.Empty},
                { "Publisher", string.Empty}
            };

            foreach (string key in listOfMostPopularProperties.Keys )
            {
                if (listOfMostPopularProperties[key] != null && key != "Book") // && key != "Publisher"
                    for (int i = 0; (i <= listOfMostPopularProperties[key].Count - 1 && i <= 2); i++)
                    {
                        NotiflicationPanelMessage[key] += $"• {dbo.GetPropertyByID(key, listOfMostPopularProperties[key][i].ID)}\n";
                    }
            }
            

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
            foreach (string property in new List<string>() {"Genre", "Author" })
            {
                if(listOfMostPopularProperties[property] != null && listOfMostPopularProperties[property].Count > 0)
                {   
                    List<BookInfo> propertyWithBooksLowInStock = dbo.GetBooksLowInStock(property, new List<string>()
                    {
                        dbo.GetPropertyByID(property,listOfMostPopularProperties[property][0].ID),
                        dbo.GetPropertyByID(property,listOfMostPopularProperties[property][1].ID),
                        dbo.GetPropertyByID(property,listOfMostPopularProperties[property][2].ID)

                    });
                    UpdateCategoryInfo(propertyWithBooksLowInStock, property);
                }
            }
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
