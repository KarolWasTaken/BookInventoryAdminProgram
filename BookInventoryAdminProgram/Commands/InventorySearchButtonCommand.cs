using BookInventoryAdminProgram.Model;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Windows;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Commands
{
    public class InventorySearchButtonCommand : CommandBase
    {
        private ErrorsViewModel _errorsViewModel;
        private InventoryPanelViewModel _inventoryPanelViewModel;

        public InventorySearchButtonCommand(InventoryPanelViewModel inventoryPanelViewModel, ErrorsViewModel errorsViewModel)
        {
            _inventoryPanelViewModel = inventoryPanelViewModel;
            _errorsViewModel = errorsViewModel;
        }
        
        public override void Execute(object? parameter)
        {
            FilteringDatabase fd = new FilteringDatabase();
            bool filterData = false;
            if (fd.CheckForErrors(_errorsViewModel, _inventoryPanelViewModel))
                return;


            Dictionary<string, string> inputs = fd.GetInputsForFiltering(_inventoryPanelViewModel);
            List<string> authorSearchlist = _inventoryPanelViewModel.SearchList["Author"].ToList();
            List<string> genreSearchlist = _inventoryPanelViewModel.SearchList["Genre"].ToList();
            // update here to ensure we are using most up to date version of db
            List<BookInfo> database = DatabaseStore.updateDatastore();

            bool areComboBoxesPopulated = inputs["PropertyName"] != null && inputs["FieldName"] != null && inputs["Condition"] != null && inputs["FilterValue"] != null;
            bool isSearchFieldPopulated = inputs["FilterBookName"] != null;
            bool isAuthorSearchListPopulated = authorSearchlist.Count > 0;
            bool isGenreSearchListPopulated = genreSearchlist.Count > 0;

            IQueryable<BookInfo> query = database.AsQueryable();
            if (areComboBoxesPopulated)
            {
                // Construct the filtering condition based on the criteria         
                string filterExpression = $"{inputs["PropertyName"]}.Any({inputs["PropertyName"]}Item => {inputs["PropertyName"]}Item.{inputs["FieldName"]} {inputs["Condition"]} {inputs["FilterValue"]})";
            
                // Use Dynamic LINQ to filter the data
                query = query.Where(filterExpression);
                filterData = true;
            }

            if (isSearchFieldPopulated)
            {
                query = query.Where(n => n.Title.ToUpper().StartsWith(inputs["FilterBookName"].ToUpper()));
                filterData = true;
            }
            
            if(isAuthorSearchListPopulated) 
            { 
                query = query.Where(n => n.Authors.Any(item => authorSearchlist.Contains(item)));
                filterData = true;
            }
            if (isGenreSearchListPopulated)
            {
                query = query.Where(n => n.Genres.Any(item => genreSearchlist.Contains(item)));
                filterData = true;
            }

            // Update datagrid in inventoryView if a query was performed
            if (filterData)
            {
                List<BookInfo> filteredResults = query.ToList();
                _inventoryPanelViewModel.InventoryDatagrid = filteredResults;
            }
            else
            {
                _inventoryPanelViewModel.InventoryDatagrid = database;
            }
            
        }
    }
}
