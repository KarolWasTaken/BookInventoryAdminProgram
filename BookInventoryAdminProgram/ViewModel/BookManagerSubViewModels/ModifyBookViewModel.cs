using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Commands.BookManager;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels
{
    public class ModifyBookViewModel : ViewModelBase, INotifyDataErrorInfo
    {
		private ObservableCollection<BookInfo>? mainDataset = new ObservableCollection<BookInfo>(DatabaseStore.MainDataset);
		public ObservableCollection<BookInfo> MainDataset
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
				OnPropertyChanged(nameof(SelectedBook));
                OnPropertyChanged(nameof(BookCover));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PricePerUnit));
                OnPropertyChanged(nameof(BookStock));
                OnPropertyChanged(nameof(EnableTextBoxes));
            }
		}
        
		private string price;
		public string Price
		{
			get
			{
				if(selectedBook == null) 
				{
					return price;
				}
                return selectedBook.Price.ToString();

            }
			set
			{
                _errorsViewModel.RemoveError(nameof(Price));
                price = value;
                if (selectedBook == null)
                {
                    return;
                }
                if (!price.Contains("."))
                    price += ".";
                if (double.TryParse(price, out double result) && price.Length - price.IndexOf(".") - 1 <= 2)
                {
                    if (price.Length - price.IndexOf(".") - 1 < 2)
                    {
                        int decimalPlacesLeftToPopulate = 2 - (price.Length - price.IndexOf(".") - 1);
                        for (int i = decimalPlacesLeftToPopulate - 1; i >= 0; i--)
                        {
                            price += "0";
                        }
                    }
                    SelectedBook.Price = double.Parse(price);
                    ListBoxEnabled = false; // disable listbox until edits are committed
                    EditMade = true;
                    OnPropertyChanged(nameof(selectedBook));
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(ListBoxEnabled));
                }
                else
                    _errorsViewModel.AddError(nameof(Price), "Invalid format for Price");
                OnPropertyChanged(nameof(CanCommitChanges));
            }
		}
        private string pricePerUnit;
        public string PricePerUnit
        {
            get
            {
                if (selectedBook == null)
                {
                    return pricePerUnit;
                }
                // return most recent Price Per Unit or empty string.
                return selectedBook.PricePerUnit.OrderByDescending(ppu => ppu.SetDate).FirstOrDefault()?.PricePerUnit.ToString() ?? "";
            }
            set
            {
                OnPropertyChanged(nameof(PricePerUnit));
                pricePerUnit = value;

                if (selectedBook == null)
                {
                    return;
                }
                if (!pricePerUnit.Contains("."))
                    pricePerUnit += ".";
                if (double.TryParse(pricePerUnit, out double result))
                {
                    if (pricePerUnit.Length - pricePerUnit.IndexOf(".") - 1 < 2)
                    {
                        int decimalPlacesLeftToPopulate = 2 - (pricePerUnit.Length - pricePerUnit.IndexOf(".") - 1);
                        for (int i = decimalPlacesLeftToPopulate - 1; i >= 0; i--)
                        {
                            pricePerUnit += "0";
                        }
                    }
                    SelectedBook.PricePerUnit.OrderByDescending(ppu => ppu.SetDate).FirstOrDefault().PricePerUnit = double.Parse(pricePerUnit);
                    ListBoxEnabled = false; // disable listbox until edits are committed
                    EditMade = true;
                    OnPropertyChanged(nameof(selectedBook));
                    OnPropertyChanged(nameof(PricePerUnit));
                    OnPropertyChanged(nameof(ListBoxEnabled));
                }
                else
                    _errorsViewModel.AddError(nameof(PricePerUnit), "Invalid format for Price");
                OnPropertyChanged(nameof(CanCommitChanges));
            }
        }

        private string bookStock;
        public string BookStock
        {
            get
            {
                if (selectedBook == null)
                {
                    return bookStock;
                }
                return selectedBook.BookStock.ToString();
            }
            set
            {
                _errorsViewModel.RemoveError(nameof(BookStock));
                bookStock = value;
                if (selectedBook == null)
                {
                    return;
                }
                if (int.TryParse(bookStock, out int result) && int.Parse(bookStock) >= -1)
                {
                    selectedBook.BookStock = int.Parse(bookStock);
                    ListBoxEnabled = false; // disable listbox until edits are committed
                    EditMade = true;
                    OnPropertyChanged(nameof(selectedBook));
                    OnPropertyChanged(nameof(BookStock));
                    OnPropertyChanged(nameof(ListBoxEnabled));
                }
                else
                    _errorsViewModel.AddError(nameof(BookStock), "BookStock must be an integer");
                OnPropertyChanged(nameof(CanCommitChanges));
            }
        }
        private byte[] bookCover;
        public byte[] BookCover
        {
            get
            {
                if (selectedBook == null)
                {
                    // No Item Selected
                    return BookCoverImageProcesses.GetNoCoverImage();
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
            set
            {
                if(selectedBook == null)
                {
                    return;
                }
                bookCover = value;
                selectedBook.BookCover = bookCover;
                ListBoxEnabled = false;
                EditMade = true;
                OnPropertyChanged(nameof(selectedBook));
                OnPropertyChanged(nameof(BookCover));
                OnPropertyChanged(nameof(ListBoxEnabled));
                OnPropertyChanged(nameof(CanCommitChanges));
            }
        }
        public ICommand RemoveBookCoverCommand { get; }
        public ICommand SelectFileCommand { get; }
        public ICommand CommitChangesCommand { get; }

        public bool ListBoxEnabled { get; set; }
        public bool EditMade = false;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public bool EnableTextBoxes => selectedBook != null;
        public bool CanCommitChanges => !HasErrors && EditMade;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private readonly ErrorsViewModel _errorsViewModel;
        public ModifyBookViewModel()
        {
            ListBoxEnabled = true;
            RemoveBookCoverCommand = new RemoveBookCoverCommand(this);
            SelectFileCommand = new SelectFileCommand(this);
            CommitChangesCommand = new CommitChangesCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += _errorsViewModel_ErrorsChanged; ;
        }

        private void _errorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCommitChanges));
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        public void UpdateProperties()
        {
            OnPropertyChanged(nameof(selectedBook));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(PricePerUnit));
            OnPropertyChanged(nameof(BookStock));
            OnPropertyChanged(nameof(ListBoxEnabled));
            OnPropertyChanged(nameof(CanCommitChanges));
            OnPropertyChanged(nameof(EnableTextBoxes));
            MainDataset = new ObservableCollection<BookInfo>(DatabaseStore.MainDataset); // refresh the database listview
        }
        public void UpdateProperty(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}
