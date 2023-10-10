using BookInventoryAdminProgram.Commands;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel
{
    public class InventoryPanelViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        // combobox options
        private List<string> _salesComboBoxOptions = new List<string> {"Sales", "Revenue"};
        public List<string> SalesComboBoxOptions
        { get => _salesComboBoxOptions; set => _salesComboBoxOptions = value; }
        private List<string> _typeComboBoxOptions = new List<string> {"Daily","Monthly","Yearly","All-Time" };
        public List<string> TypeComboBoxOptions
        { get => _typeComboBoxOptions; set => _typeComboBoxOptions = value; }
        private List<string> _modifierComboBoxOptions = new List<string> {"Greater than","Less than","Equal to"};
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
        private Dictionary<string,string> _comboBoxTypedText = new Dictionary<string, string>
        {
            {"Sales", null}, {"Type", null}, {"Modifier", null}
        };
        public Dictionary<string,string> ComboBoxTypedText
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
            { "ISBN", true },{ "Title", true }, {"Price", true },  { "Author", true }, { "Genre", true }, { "ReleaseDate", true },
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


        private readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public InventoryPanelViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            InventoryDatagrid = DatabaseStore.updateDatastore();

            ToggleHeaderVisibilityCommand = new ToggleHeaderVisibilityCommand(HeaderVisibility, SetDictionary);
            InventorySearchButtonCommand = new InventorySearchButtonCommand(this, _errorsViewModel);
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
