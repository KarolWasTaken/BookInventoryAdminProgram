using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.ViewModel
{
    public class MainWindowViewModel : ViewModelBase 
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainWindowViewModel()
        {
            CurrentViewModel = new LoginWindowViewModel();
        }
    }
}
