﻿using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace BookInventoryAdminProgram.Commands
{
    public class RemoveSearchListItem : CommandBase
    {
        private Dictionary<string, ObservableCollection<string>> _searchList;
        private Dictionary<string, string> _selectedSearchListItem;

        public RemoveSearchListItem(Dictionary<string, ObservableCollection<string>> searchList, Dictionary<string, string> selectedSearchListItem)
        {
            _searchList = searchList;
            _selectedSearchListItem = selectedSearchListItem;
        }

        public override void Execute(object? parameter)
        {
            string key;
            bool deleteAll = false;
            string[] parameters = parameter.ToString().Split(",");


            if (parameters[0] == "Author")
                key = "Author";
            else if (parameters[0] == "Genre")
                key = "Genre";
            else
                throw new Exception("type not recognised");

            if (parameters[1] == "All")
                deleteAll = true;


            if ((_selectedSearchListItem[key] == null || _selectedSearchListItem[key] == "") && !deleteAll)
                return;
            else
                if (deleteAll)
                    _searchList[key].Clear();
                else
                    _searchList[key].Remove(_selectedSearchListItem[key]);
        }
    }
}
