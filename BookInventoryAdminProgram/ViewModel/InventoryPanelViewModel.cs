using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.ViewModel
{
    public class InventoryPanelViewModel : ViewModelBase
    {
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

        private Dictionary<string, bool> _headerVisibility = new Dictionary<string, bool>
        {
            { "ISBN", true },{ "Title", true }, { "Author", true }, { "Genre", true }, { "ReleaseDate", true },
            { "Publisher", true }, { "AllTimeSales", false }, { "YearlySales", true }, { "MonthlySales", true },
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
                //_headerVisibility = value;
                //OnPropertyChanged(nameof(HeaderVisibility));
                _headerVisibility = value;
                OnPropertyChanged(nameof(HeaderVisibility));

            }
        }
        public void SetDictionary(string key, bool value)
        {
            HeaderVisibility[key] = value;
            OnPropertyChanged(nameof(HeaderVisibility));
        }

        public ICommand ToggleHeaderVisibilityCommand { get; }
        public InventoryPanelViewModel()
        {
            ToggleHeaderVisibilityCommand = new ToggleHeaderVisibilityCommand(HeaderVisibility, SetDictionary);
            InventoryDatagrid = DatabaseStore.updateDatastore();
            //var test = InventoryDatagrid.Select(item => item.ISBN.ToList());
        }

    }
}
