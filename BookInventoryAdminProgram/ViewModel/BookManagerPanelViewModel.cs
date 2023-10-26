using BookInventoryAdminProgram.Commands.BookManager.NavigationCommands;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookInventoryAdminProgram.ViewModel
{
    public class BookManagerPanelViewModel : ViewModelBase
    {
        private ViewModelBase currentSubView;
        public ViewModelBase CurrentSubView
        {
            get { return currentSubView; }
            set
            {
                if(currentSubView != value) 
                {
                    currentSubView = value;
                    OnPropertyChanged(nameof(CurrentSubView));                
                }
            }
        }


        public ICommand NavigateSubViewCommand { get; }
        public BookManagerPanelViewModel()
        {
            NavigateSubViewCommand = new BookManagerNaviagationCommand(this);
        }
    }
}
