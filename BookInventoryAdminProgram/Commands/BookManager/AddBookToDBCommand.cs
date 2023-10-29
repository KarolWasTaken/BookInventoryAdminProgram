using AdonisUI.Controls;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookInventoryAdminProgram.Commands.BookManager
{
    public class AddBookToDBCommand : CommandBase
    {
        private AddBookViewModel _addBookViewModel;
        public AddBookToDBCommand(AddBookViewModel addBookViewModel)
        {
            _addBookViewModel = addBookViewModel;
        }
        public override void Execute(object? parameter)
        {
            string GenreNames = GetAttributeNamesCommaseparated(_addBookViewModel.SearchList["Genre"]);
            string AuthorNames = GetAttributeNamesCommaseparated(_addBookViewModel.SearchList["Author"]);
            byte[] BookCover;
            if (_addBookViewModel.BookCover == null)
                BookCover = null;
            else
                BookCover = _addBookViewModel.BookCover;

            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                var parameters = new
                {
                    Title = _addBookViewModel.Title,
                    GenreNames = GenreNames,
                    AuthorNames = AuthorNames,
                    ReleaseDate = _addBookViewModel.ReleaseDate,
                    PublisherName = _addBookViewModel.Publisher,
                    ISBN = _addBookViewModel.ISBN,
                    BookCover = BookCover,
                    Price = decimal.Parse(_addBookViewModel.Price),
                    PricePerUnit = decimal.Parse(_addBookViewModel.PricePerUnit),
                    BookStock = int.Parse(_addBookViewModel.BookStock)

                };
                connection.Execute("dbo.spAddBookWithEntities @Title, @GenreNames, @AuthorNames, @ReleaseDate, @PublisherName, @ISBN, @BookCover, @Price, @PricePerUnit, @BookStock", parameters);
                var messageBox = new MessageBoxModel
                {
                    Text = "Book added to database.",
                    Caption = "Info",
                    Icon = AdonisUI.Controls.MessageBoxImage.Information,
                    Buttons = new List<IMessageBoxButtonModel> { MessageBoxButtons.Ok() }
                };
                AdonisUI.Controls.MessageBox.Show(messageBox);
                ResetAllProperties();
            }
        }
        private void ResetAllProperties()
        {
            _addBookViewModel.Title = null;
            _addBookViewModel.SearchList["Author"].Clear();
            _addBookViewModel.SearchList["Genre"].Clear();
            _addBookViewModel.AdditionNotifier = null;
            _addBookViewModel.ReleaseDate = DateTime.Now;
            _addBookViewModel.Publisher = null;
            _addBookViewModel.ISBN = null;
            _addBookViewModel.BookCover = null;
            _addBookViewModel.Price = null;
            _addBookViewModel.PricePerUnit = null;
            _addBookViewModel.BookStock = null;
            _addBookViewModel.BookCover = null;
            _addBookViewModel.DragDropUIVisibility = true;
            _addBookViewModel.ImageVisibility = false;
        }
        private string GetAttributeNamesCommaseparated(System.Collections.ObjectModel.ObservableCollection<string> observableCollection)
        {
            string names = string.Empty;
            foreach (var item in observableCollection) 
            {
                names += $"{item},";
            }
            names = names.Substring(0, names.Length - 1);
            return names;
        }
    }
}
