using BookInventoryAdminProgram.ViewModel;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace BookInventoryAdminProgram.Commands.BookManager.NavigationCommands
{
    public class BookManagerNaviagationCommand : CommandBase
    {
        private BookManagerPanelViewModel BookManagerPanelViewModel;
        public BookManagerNaviagationCommand(BookManagerPanelViewModel bookManagerPanelViewModel)
        {
            BookManagerPanelViewModel = bookManagerPanelViewModel;
        }
        public override void Execute(object? parameter)
        {
            switch(parameter.ToString())
            {
                case "Add":
                    BookManagerPanelViewModel.CurrentSubView = new AddBookViewModel();
                    break;
                case "Modify":
                    BookManagerPanelViewModel.CurrentSubView = new ModifyBookViewModel();
                    break;
                case "Remove":
                    BookManagerPanelViewModel.CurrentSubView = new RemoveBookViewModel();
                    break;
            }
                
        }
    }
}
