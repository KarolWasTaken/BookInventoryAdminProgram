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

		//
		private Dictionary<string, bool> _headerVisibility = new Dictionary<string, bool>
		{
			{ "ISBN", true },{ "Title", true }
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
                if (_headerVisibility != value)
                {
                    _headerVisibility = value;
                    OnPropertyChanged(nameof(HeaderVisibility));
                }
            }
		}
		private Visibility _vis;
		public Visibility Vis
		{
			get
			{
				return _vis;
			}
			set
			{
				_vis = value;
				OnPropertyChanged(nameof(Vis));
			}
		}

		public ICommand ToggleHeaderVisibilityCommand { get; }
        public InventoryPanelViewModel()
        {
            ToggleHeaderVisibilityCommand = new ToggleHeaderVisibilityCommand(_headerVisibility, OnPropertyChanged);
            InventoryDatagrid = DatabaseStore.updateDatastore();
            //var test = InventoryDatagrid.Select(item => item.ISBN.ToList());
        }

    }
}
