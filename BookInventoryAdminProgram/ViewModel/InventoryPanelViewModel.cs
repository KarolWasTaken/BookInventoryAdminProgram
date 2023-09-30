using BookInventoryAdminProgram.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public ICommand ToggleHeaderCommand { get; }
        public InventoryPanelViewModel()
        {
            InventoryDatagrid = DatabaseStore.updateDatastore();
			//var test = InventoryDatagrid.Select(item => item.ISBN.ToList());
        }
    }
}
