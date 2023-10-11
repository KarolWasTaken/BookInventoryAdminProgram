using BookInventoryAdminProgram.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookInventoryAdminProgram.Converter
{
    public class ListToObservableCollectionConverter 
    {
        public static ObservableCollection<T> Convert<T>(List<T> list)
        {
            var observableCollection = new ObservableCollection<T>();
            foreach (var item in list)
            {
                observableCollection.Add(item);
            }
            return observableCollection;
        }
    }
}
