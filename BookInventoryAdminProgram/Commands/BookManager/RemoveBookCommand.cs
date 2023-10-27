using AdonisUI.Controls;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static BookInventoryAdminProgram.Model.DatabaseOperations;

namespace BookInventoryAdminProgram.Commands.BookManager
{
    public class RemoveBookCommand : CommandBase
    {
        public RemoveBookViewModel _removeBookViewModel;
        public RemoveBookCommand(RemoveBookViewModel removeBookViewModel)
        {
            _removeBookViewModel = removeBookViewModel;
        }

        public override void Execute(object? parameter)
        {
            string messageText;
            DatabaseOperations dbo = new DatabaseOperations();
            RemoveType removeType;
            if (parameter.ToString() == "Complete")
            {
                messageText = $"Are you sure you want to remove {_removeBookViewModel.SelectedBook.Title}?\nThis will also erase all sales data. Consider removing from catalogue.";
                removeType = RemoveType.Complete;
            }
            else if (parameter.ToString() == "Catalogue")
            {
                messageText = $"Are you sure you want to remove {_removeBookViewModel.SelectedBook.Title} from catalogue?";
                removeType = RemoveType.Catalogue;
            }
            else
            {
                throw new Exception("Operation not recognised");
            }



            var messageBox = new MessageBoxModel
            {
                Text = messageText,
                Caption = "Info",
                Icon = AdonisUI.Controls.MessageBoxImage.Question,
                Buttons = MessageBoxButtons.YesNo(),
            };
            AdonisUI.Controls.MessageBox.Show(messageBox);


            switch (messageBox.Result)
            {
                case AdonisUI.Controls.MessageBoxResult.Yes:
                    dbo.RemoveBookByID(_removeBookViewModel.SelectedBook.BookID, removeType);
                    _removeBookViewModel.SelectedBook = null;
                    _removeBookViewModel.MainDataset = DatabaseStore.MainDataset; // updates database in ui
                    //_navigationCommand.ChangeViewModel();
                    break;
                case AdonisUI.Controls.MessageBoxResult.No:
                    return;
            }
        }
    }
}
