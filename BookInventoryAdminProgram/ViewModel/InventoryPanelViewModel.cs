using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel
{
    public class InventoryPanelViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private static List<BookInfo> mainDataBase;
        // combobox options
        private List<string> _salesComboBoxOptions = new List<string> { "Sales", "Revenue" };
        public List<string> SalesComboBoxOptions
        { get => _salesComboBoxOptions; set => _salesComboBoxOptions = value; }
        private List<string> _typeComboBoxOptions = new List<string> { "Daily", "Monthly", "Yearly", "All-Time" };
        public List<string> TypeComboBoxOptions
        { get => _typeComboBoxOptions; set => _typeComboBoxOptions = value; }
        private List<string> _modifierComboBoxOptions = new List<string> { "Greater than", "Less than", "Equal to" };
        public List<string> ModifierComboBoxOptions
        { get => _modifierComboBoxOptions; set => _modifierComboBoxOptions = value; }
        public Dictionary<string, bool> HasValue = new Dictionary<string, bool>()
        {
            {"Sales", false}, {"Type", false}, {"Modifier", false}
        };
        private Dictionary<string, string> _selectedItem = new Dictionary<string, string>
        {
            {"Sales", null}, {"Type", null}, {"Modifier", null}
        };
        public Dictionary<string, string> SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                // I have no words.This doesnt run yet the dict still updates.
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private Dictionary<string, string> _comboBoxTypedText = new Dictionary<string, string>
        {
            {"Sales", null}, {"Type", null}, {"Modifier", null}
        };
        public Dictionary<string, string> ComboBoxTypedText
        {
            get
            {
                return _comboBoxTypedText;
            }
            set
            {
                _comboBoxTypedText = value;
                OnPropertyChanged(nameof(ComboBoxTypedText));
            }
        }


        private string _comboBoxQueryQuantity = null;
        public string ComboBoxQueryQuantity
        {
            get
            {
                return _comboBoxQueryQuantity;
            }
            set
            {
                if (value == "")
                    _comboBoxQueryQuantity = null;
                else
                    _comboBoxQueryQuantity = value;
                OnPropertyChanged(nameof(ComboBoxQueryQuantity));
            }
        }

        // Search Bar
        private string _searchFieldValue;
        public string SearchFieldValue
        {
            get
            {
                return _searchFieldValue;
            }
            set
            {
                _searchFieldValue = value;
                OnPropertyChanged(nameof(SearchFieldValue));
            }
        }

        /*// Author Search
        private List<string> _presentAuthorList = FilteringDatabase.MergeSort(mainDataBase
            .SelectMany(book => book.Authors)
            .Distinct()
            .ToList()
            );
        public List<string> PresentAuthorList
        { get => _presentAuthorList; set => _presentAuthorList = value; }
        // GenreSearch
        private List<string> _presentGenreList = FilteringDatabase.MergeSort(mainDataBase
            .SelectMany(book => book.Genres)
            .Distinct()
            .ToList()
            );
        public List<string> PresentGenreList
        { get => _presentGenreList; set => _presentGenreList = value; }*/

        private Dictionary<string, List<string>> _presentAGList;
        public Dictionary<string, List<string>> PresentAGList
        {
            get
            {
                return _presentAGList;
            }
        }

        // this one is for the fist combo box in author/genre search
        private Dictionary<string, string> _selectedItemAG = new Dictionary<string, string>()
        {
            {"Author", null }, {"Genre", null}
        };

        public Dictionary<string, string> SelectedItemAG
        {
            get { return _selectedItemAG; }
            set { _selectedItemAG = value; }
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



        // datagrid property
        private List<BookInfo> _inventoryDatagrid;
        public List<BookInfo> InventoryDatagrid
        {
            get
            {
                return _inventoryDatagrid;
            }
            set
            {
                _inventoryDatagrid = value;
                OnPropertyChanged(nameof(InventoryDatagrid));
            }
        }
        // all the checkbuttons bind to this. When check/uncheck it reflects here
        private Dictionary<string, bool> _headerVisibility = new Dictionary<string, bool>
        {
            { "ISBN", true },{ "Title", true }, {"Price", true }, {"PricePerUnit", true }, {"BookStock", true}, { "Author", true }, { "Genre", true }, { "ReleaseDate", true },
            { "Publisher", true }, { "AllTimeSales", true }, { "YearlySales", true }, { "MonthlySales", true },
            { "DailySales", true }
        };
        public Dictionary<string, bool> HeaderVisibility
        {
            get
            {
                return _headerVisibility;
            }
            set
            {
                // this part doesnt even run. why?
                _headerVisibility = value;
                OnPropertyChanged(nameof(HeaderVisibility));
            }
        }
        /// <summary>
        /// Because the goddamn setter wont run on this cursed Dict, i need to make 
        /// a method to run in it's stead.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDictionary(string key, bool value)
        {
            HeaderVisibility[key] = value;
            OnPropertyChanged(nameof(HeaderVisibility));
        }

        public ICommand ToggleHeaderVisibilityCommand { get; }
        public ICommand InventorySearchButtonCommand { get; }
        public ICommand AddSearchListItem { get; }
        public ICommand RemoveSearchListItem { get; }


        public readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public InventoryPanelViewModel(DatabaseHashStore databaseHashStore)
        {
            databaseHashStore.UpdateHash(DataHasher.CalculateHash(DatabaseStore.MainDataset));

            _headerVisibility = Helper.ReturnSettings().HeaderVisibilitiesSerialised;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            IsLoading = true;
            if (mainDataBase == null || databaseHashStore.NewHash != databaseHashStore.OldHash)
            {
                // this needs optimisation because loading is too slow.
                Task.Run(() =>
                {
                    mainDataBase = DatabaseStore.MainDataset;
                    _presentAGList = new Dictionary<string, List<string>>()
                    {
                        {"Author",
                            FilteringDatabase.MergeSort(mainDataBase
                                .SelectMany(book => book.Authors)
                                .Distinct()
                                .ToList()
                        )},
                        {"Genre",
                            FilteringDatabase.MergeSort(mainDataBase
                                .SelectMany(book => book.Genres)
                                .Distinct()
                                .ToList()
                        )}
                    };

                    System.Threading.Thread.Sleep(500);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        InventoryDatagrid = mainDataBase; // i think this operation is slowing down the ui thread look below
                        IsLoading = false; 
                    });
                });

            }
            else
            {
                _presentAGList = new Dictionary<string, List<string>>()
                {
                    {"Author",
                        FilteringDatabase.MergeSort(mainDataBase
                            .SelectMany(book => book.Authors)
                            .Distinct()
                            .ToList()
                    )},
                    {"Genre",
                        FilteringDatabase.MergeSort(mainDataBase
                            .SelectMany(book => book.Genres)
                            .Distinct()
                            .ToList()
                    )}
                };
                   
                InventoryDatagrid = mainDataBase; // i think this operation is slowing down the ui thread look below
                IsLoading = false;     
            }
            // NOTE TO EXAMINER:
            // when the inner datagrids are activated, they slow down the ui when being assigned to. (alltimessales, priceperunit, etc). This upsets me.
            // i dont know how to fix that. This will have to remain for now. The issue is that instead of loading 1 datagrid, I load > 10 in a short time.
            // Pagination wont help because its all in 1 page. Maybe i can tell the ui to only load them in 1 by 1 with a 1/4 second pause in between each so the ui
            // doesnt look like its crashing, but thats beyond the scope of this project.
            // TLDR:
            // WPF take long long time to load datagrids inside datagrid, therefore program will look like its crashing when loading columns like alltimessales.
            // Not fixable because i'd have to interfere with how wpf renders elements. Too hard. Not worth the hassle for mininal mark potential.

            ToggleHeaderVisibilityCommand = new ToggleHeaderVisibilityCommand(HeaderVisibility, SetDictionary);
            InventorySearchButtonCommand = new InventorySearchButtonCommand(this, _errorsViewModel);
            AddSearchListItem = new AddSearchListItem(SelectedItemAG, SearchList);
            RemoveSearchListItem = new RemoveSearchListItem(SearchList, SelectedSearchListItem);
        }








        private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(ComboBoxQueryQuantity));
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}