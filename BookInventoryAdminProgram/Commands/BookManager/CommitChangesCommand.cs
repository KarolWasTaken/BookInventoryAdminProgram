using AdonisUI.Controls;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands.BookManager
{
    public class CommitChangesCommand : CommandBase
    {
        private ModifyBookViewModel _modifyBookViewModel;

        public CommitChangesCommand(ModifyBookViewModel modifyBookViewModel)
        {
            _modifyBookViewModel = modifyBookViewModel;
        }

        public override void Execute(object? parameter)
        {
            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                var parameters = new
                {
                    BookID = _modifyBookViewModel.SelectedBook.BookID,
                    Price = _modifyBookViewModel.SelectedBook.Price,
                    BookStock = _modifyBookViewModel.SelectedBook.BookStock,
                    BookCover = _modifyBookViewModel.SelectedBook.BookCover
                };
                connection.Execute("dbo.spUpdateBookProperties @BookID, @Price, @BookStock, @BookCover", parameters);
                var messageBox = new MessageBoxModel
                {
                    Text = $"{_modifyBookViewModel.SelectedBook.Title} has been updated.",
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
            _modifyBookViewModel.ListBoxEnabled = true;
            _modifyBookViewModel.EditMade = false;
            _modifyBookViewModel.SelectedBook = null;
            _modifyBookViewModel.Price = null;
            _modifyBookViewModel.BookStock = null;
            _modifyBookViewModel.UpdateProperties();
        }
    }
}
