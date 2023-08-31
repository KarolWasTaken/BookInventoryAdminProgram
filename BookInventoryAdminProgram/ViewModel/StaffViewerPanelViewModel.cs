using BookInventoryAdminProgram.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.ViewModel
{
    public class StaffViewerPanelViewModel : ViewModelBase
    {
        public ObservableCollection<ReturnStaff> StaffList { get; set; }
		public StaffViewerPanelViewModel()
        {
            ReturnStaff rs = new ReturnStaff();
            List<ReturnStaff> staffList = rs.GetStaff();

            StaffList = new ObservableCollection<ReturnStaff>(staffList);
        }
    }
}
