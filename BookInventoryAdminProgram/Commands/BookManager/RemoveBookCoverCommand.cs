using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Commands.BookManager
{
    public class RemoveBookCoverCommand : CommandBase
    {
        public AddBookViewModel _addBookViewModel;
        public ModifyBookViewModel _modifyBookViewModel;
        public RemoveBookCoverCommand(AddBookViewModel addBookViewModel)
        {
            _addBookViewModel = addBookViewModel;
        }
        public RemoveBookCoverCommand(ModifyBookViewModel modifyBookViewModel)
        {
            _modifyBookViewModel = modifyBookViewModel;
        }
        public override void Execute(object? parameter)
        {
            if (parameter.ToString() == "AddViewModel")
            { 
                _addBookViewModel.BookCover = null;
                _addBookViewModel.DragDropUIVisibility = true;
                _addBookViewModel.ImageVisibility = false;
            }
            else if (parameter.ToString() == "ModifyViewModel")
            {
                _modifyBookViewModel.BookCover = null;
            }
        }
    }
}
