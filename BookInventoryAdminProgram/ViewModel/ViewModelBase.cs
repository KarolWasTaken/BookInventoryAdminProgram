using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.ViewModel
{
    /// <summary>
    /// Base to scaffold out core functionality of ViewModels
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        // all view models will have properties in their views, so all of them need a propertyChanged event to update ui 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
