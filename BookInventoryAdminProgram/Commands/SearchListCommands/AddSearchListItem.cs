using AdonisUI.Controls;
using BookInventoryAdminProgram.ViewModel;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
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
        private bool _notifier;
        private AddBookViewModel _addViewModel;

        public AddSearchListItem(Dictionary<string, string> selectedItemAG, Dictionary<string, ObservableCollection<string>> searchList)
        {
            _selectedItemAG = selectedItemAG;
            _searchList = searchList;
        }

        // not the most elegant way to do this but it'll do.
        public AddSearchListItem(Dictionary<string, string> selectedItemAG, Dictionary<string, ObservableCollection<string>> searchList, bool notifier, AddBookViewModel addViewModel)
        {
            _selectedItemAG = selectedItemAG;
            _searchList = searchList;
            _notifier = notifier;
            _addViewModel = addViewModel;
        }


        public override void Execute(object? parameter)
        {
            string key;
            if ((string)parameter == "Author")
            {
                key = "Author";
                if(_notifier == true)
                    LinkItemDictionary("Author");
            }
            else if ((string)parameter == "Genre")
            {
                key = "Genre";
                if (_notifier == true)
                    LinkItemDictionary("Genre");
            }
            else
                throw new Exception("type not recognised");

            if ((_selectedItemAG[key] == null || _selectedItemAG[key] == "") || _searchList[key].Contains(_selectedItemAG[key]))
            {
                if (_notifier == true)
                {
                    _addViewModel.AdditionNotifier = null;
                    string fieldValue = (key == "Author") ? _addViewModel.AuthorFieldBox?.Trim() : _addViewModel.GenreFieldBox?.Trim();
                    if (_searchList[key].Contains(_selectedItemAG[key]) && fieldValue != null)
                        _addViewModel.AdditionNotifier = $"Item already exists in {key}s";
                }
                return;
            }
            else
            {
                _searchList[key].Add(_selectedItemAG[key]);
                
                if (_notifier == true)
                {
                    if (key == "Author")
                        _addViewModel.AuthorFieldBox = null;
                    else if (key == "Genre")
                        _addViewModel.GenreFieldBox = null;
                    _addViewModel.AdditionNotifier = $"{_addViewModel.ItemToAddAG[key]} added to {key}s!";
                    _addViewModel.NotifyAGUpdate();
                }
                    
            }
        }

        private void LinkItemDictionary(string key)
        {

            try
            {
                
                string fieldValue = (key == "Author") ? _addViewModel.AuthorFieldBox?.Trim() : _addViewModel.GenreFieldBox?.Trim();
                if (fieldValue == "" || fieldValue == null)
                    return;
                _selectedItemAG[key] = CapitaliseEachWord(fieldValue);
            }
            catch (Exception e)
            {
                string fieldValue = (key == "Author") ? _addViewModel.AuthorFieldBox : _addViewModel.GenreFieldBox;
                _selectedItemAG[key] = CapitaliseEachWord(fieldValue);
            }
        }
        private string CapitaliseEachWord(string inputString)
        {
            string[] words = inputString.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
            }

            string outputString = string.Join(" ", words);

            return outputString; // This will output "Hello World"

        }
    }
}
