using AdonisUI.Controls;
using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Commands.BookManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels
{
    public class AddBookViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string title;
        public string Title
        {
            get
            { return title; }
            set
            {
                if (value == null || value == "")
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                    return;
                }
                title = value.Trim();
                _errorsViewModel.RemoveError(nameof(Title));
                if(title.Length > 255)
                    _errorsViewModel.AddError(nameof(Title), "Title cant be larger than 255 characters");
                else
                    OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }

        private string isbn;
        public string ISBN
        {
            get
            { return isbn; }
            set
            {
                _errorsViewModel.RemoveError(nameof(ISBN));
                if(value == null || value == "")
                {
                    isbn = value;
                    OnPropertyChanged(nameof(ISBN));
                    return;
                }
                isbn = value.Trim();
                if(long.TryParse(isbn, out long result)) 
                { 
                    if(isbn.Length == 13 || isbn.Length == 10)
                        OnPropertyChanged(nameof(ISBN));
                    else
                        _errorsViewModel.AddError(nameof(ISBN), "An ISBN is 10 or 13 digits");
                }
                else 
                    _errorsViewModel.AddError(nameof(ISBN), "An ISBN cannot contain characters");
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }
        private string publisher;
        public string Publisher
        {
            get
            { return publisher; }
            set
            {
                _errorsViewModel.RemoveError(nameof(Publisher));
                if (value == null || value == "")
                {
                    publisher = value;
                    OnPropertyChanged(nameof(Publisher));
                    return;
                }
                publisher = value.Trim();
                if (publisher.Length > 255)
                    _errorsViewModel.AddError(nameof(Publisher), "Publisher name cannot be larger than 255 characters");
                else
                    OnPropertyChanged(nameof(Publisher));
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }
        private DateTime releaseDate = DateTime.Now;
        public DateTime ReleaseDate
        {
            get
            { return releaseDate; }
            set
            {
                _errorsViewModel.RemoveError(nameof(ReleaseDate));
                releaseDate = value;
                if (releaseDate < new DateTime(1753, 1, 1))
                    _errorsViewModel.AddError(nameof(ReleaseDate), "Date must be after 01/01/1753");
                else
                {
                    OnPropertyChanged(nameof(ReleaseDate));
                }    
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }

        private string authorFieldBox;
        public string AuthorFieldBox
        {
            get
            {
                return authorFieldBox;
            }
            set
            {
                authorFieldBox = value;
                OnPropertyChanged(nameof(AuthorFieldBox));
            }
        }
        private string genreFieldBox;
        public string GenreFieldBox
        {
            get
            {
                return genreFieldBox;
            }
            set
            {
                genreFieldBox = value;
                OnPropertyChanged(nameof(GenreFieldBox));
            }
        }

        private Dictionary<string, string> itemToAddAG = new Dictionary<string, string>()
        {
            {"Author", null},{"Genre", null}
        };

        public Dictionary<string, string> ItemToAddAG
        {
            get { return itemToAddAG; }
            set { itemToAddAG  = value;}
        }


        // Search List Dict
        private Dictionary<string, ObservableCollection<string>> _searchList = new Dictionary<string, ObservableCollection<string>>()
        {
            {"Author", new ObservableCollection<string>() }, {"Genre", new ObservableCollection<string>() }
        };
        public Dictionary<string, ObservableCollection<string>> SearchList
        {
            get
            {
                return _searchList;
            }
            set
            {
                _searchList = value;
                OnPropertyChanged(nameof(SearchList));
            }
        }
        private Dictionary<string, string> _selectedSearchListItem = new Dictionary<string, string>()
        {
            {"Author", null }, {"Genre", null}
        };
        public Dictionary<string, string> SelectedSearchListItem
        {
            get { return _selectedSearchListItem; }
            set { _selectedSearchListItem = value; }
        }
        private string price;
        public string Price
        {
            get
            {
                return price;
            }
            set
            {
                _errorsViewModel.RemoveError(nameof(Price));
                price = value;

                if(price == null || price == "")
                {
                    OnPropertyChanged(nameof(Price));
                    return;
                }

                if(!price.Contains("."))
                    price += ".";
                if (float.TryParse(price, out float result) && price.Length - price.IndexOf(".") - 1 <= 2)
                {
                    if(price.Length - price.IndexOf(".") - 1 < 2)
                    {
                        int decimalPlacesLeftToPopulate = 2 - (price.Length - price.IndexOf(".") - 1);
                        for (int i = decimalPlacesLeftToPopulate - 1; i >= 0; i--)
                        {
                            price += "0";
                        }
                    }
                    OnPropertyChanged(nameof(Price));
                }
                else
                    _errorsViewModel.AddError(nameof(Price), "Invalid format for Price");
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }
        private string bookStock;
        public string BookStock
        {
            get
            {
                return bookStock;
            }
            set
            {
                _errorsViewModel.RemoveError(nameof(BookStock));
                bookStock = value;

                if (bookStock == null || bookStock == "")
                {
                    OnPropertyChanged(nameof(BookStock));
                    return;
                }
                if (int.TryParse(bookStock, out int result) && int.Parse(bookStock) >= -1)
                    OnPropertyChanged(nameof(BookStock));
                else
                    _errorsViewModel.AddError(nameof(BookStock), "BookStock must be an integer");
                OnPropertyChanged(nameof(CanCreateBook));
            }
        }

        private string additionNotifier;
        public string AdditionNotifier
        {
            get
            {
                return additionNotifier;
            }
            set
            {
                additionNotifier = value;
                OnPropertyChanged(nameof(AdditionNotifier));
            }
        }
        private byte[] bookCover;
        public byte[] BookCover
        {
            get
            {
                return bookCover;
            }
            set
            {
                bookCover = value;
                OnPropertyChanged(nameof(BookCover));
            }
        }
        private bool dragDropUIVisibility = true;
        public bool DragDropUIVisibility
        {
            get
            {
                return dragDropUIVisibility;
            }
            set
            {
                dragDropUIVisibility = value;
                OnPropertyChanged(nameof(DragDropUIVisibility));
            }
        }
        private bool imageVisibility = false;
        public bool ImageVisibility
        {
            get
            {
                return imageVisibility;
            }
            set
            {
                imageVisibility = value;
                OnPropertyChanged(nameof(ImageVisibility));
            }
        }

        public ICommand AddSearchListItem { get; }
        public ICommand RemoveSearchListItem { get; }
        public ICommand SelectFileCommand { get; }
        public ICommand RemoveBookCoverCommand { get; }
        public ICommand AddBookToDBCommand { get; }
        public bool HasErrors => _errorsViewModel.HasErrors;
        public bool CanCreateBook => !HasErrors 
            && Title != null 
            && (Price != null || Price != "0.00")
            && bookStock != null 
            && ISBN != null 
            && publisher != null 
            && SearchList["Author"].Count > 0 
            && SearchList["Genre"].Count > 0;


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private readonly ErrorsViewModel _errorsViewModel;
        public AddBookViewModel()
        {
            AddSearchListItem = new AddSearchListItem(ItemToAddAG, SearchList, true, this);
            RemoveSearchListItem = new RemoveSearchListItem(SearchList, SelectedSearchListItem);
            SelectFileCommand = new SelectFileCommand(this);
            RemoveBookCoverCommand = new RemoveBookCoverCommand(this);
            AddBookToDBCommand = new AddBookToDBCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += _errorsViewModel_ErrorsChanged;
        }

        private void _errorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreateBook));
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
        public void NotifyAGUpdate()
        {
            OnPropertyChanged(nameof(CanCreateBook));
        }
    }
}
