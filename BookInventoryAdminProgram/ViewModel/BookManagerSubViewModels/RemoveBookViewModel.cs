using BookInventoryAdminProgram.Commands.BookManager;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels
{
    public class RemoveBookViewModel : ViewModelBase
    {
		private List<BookInfo>mainDataset = DatabaseStore.MainDataset;
		public List<BookInfo>MainDataset
		{
			get
			{
				return mainDataset;
			}
			set
			{
				mainDataset = value;
				OnPropertyChanged(nameof(MainDataset));
            }
		}

		private BookInfo selectedBook;
		public BookInfo SelectedBook
		{
			get
			{
				return selectedBook;
			}
			set
			{
				selectedBook = value;
                if(selectedBook != null)
                    RemoveButtonEnabled = true;
                else
                    RemoveButtonEnabled = false;
                OnPropertyChanged(nameof(RemoveButtonEnabled));
				OnPropertyChanged(nameof(SelectedBook));
                OnPropertyChanged(nameof(BookCover));
            }
		}

        public byte[] BookCover
        {
            get
            {
                if (selectedBook == null)
                {
                    // No Item Selected
                    return null;
                }
                if (selectedBook.BookCover == null)
                {
                    // If selectedBook.BookCover is null, set it to a default image.
                    return BookCoverImageProcesses.GetNoCoverImage();
                }
                else
                {
                    // Otherwise, return the actual book cover.
                    return selectedBook.BookCover;
                }
            }       
        }
        public bool RemoveButtonEnabled {get; set;}
        public ICommand RemoveBookCommand { get; }

        public RemoveBookViewModel()
        {
            RemoveButtonEnabled = false;
            RemoveBookCommand = new RemoveBookCommand(this);
        }
    }
}
