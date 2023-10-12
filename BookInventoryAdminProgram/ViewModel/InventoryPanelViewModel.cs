using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel
{
    public class InventoryPanelViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private static List<BookInfo> mainDataBase = DatabaseStore.updateDatastore();
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

        private Dictionary<string,List<string>> _presentAGList = new Dictionary<string, List<string>>()
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
        public Dictionary<string,List<string>> PresentAGList
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
            { "ISBN", true },{ "Title", true }, {"Price", true }, {"BookStock", true}, { "Author", true }, { "Genre", true }, { "ReleaseDate", true },
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
        public InventoryPanelViewModel()
        {
            _headerVisibility = Helper.ReturnSettings().HeaderVisibilitiesSerialised;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            InventoryDatagrid = mainDataBase;

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
