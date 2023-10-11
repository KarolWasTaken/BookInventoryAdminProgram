using AdonisUI.Controls;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace BookInventoryAdminProgram.Commands
{
    public class AddSearchListItem : CommandBase
    {
        private Dictionary<string, string> _selectedItemAG;
        private Dictionary<string, ObservableCollection<string>> _searchList;

        public AddSearchListItem(Dictionary<string, string> selectedItemAG, Dictionary<string, ObservableCollection<string>> searchList)
        {
            _selectedItemAG = selectedItemAG;
            _searchList = searchList;
        }

        public override void Execute(object? parameter)
        {
            string key;
            if ((string)parameter == "Author")
                key = "Author";
            else if ((string)parameter == "Genre")
                key = "Genre";
            else
                throw new Exception("type not recognised");

            if ((_selectedItemAG[key] == null || _selectedItemAG[key] == "") || _searchList[key].Contains(_selectedItemAG[key]))
                return;
            else
                _searchList[key].Add(_selectedItemAG[key]);
        }
    }
}
