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
        public RemoveBookCoverCommand(AddBookViewModel addBookViewModel)
        {
            _addBookViewModel = addBookViewModel;
        }
        public override void Execute(object? parameter)
        {
            _addBookViewModel.BookCover = null;
            _addBookViewModel.DragDropUIVisibility = true;
            _addBookViewModel.ImageVisibility = false;
        }
    }
}
