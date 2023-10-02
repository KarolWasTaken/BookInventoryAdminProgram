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
            if (fd.CheckForErrors(_errorsViewModel, _inventoryPanelViewModel))
                return;


            Dictionary<string, string> inputs = fd.GetInputsForFiltering(_inventoryPanelViewModel);
            // update here to ensure we are using most up to date version of db
            List<BookInfo> database = DatabaseStore.updateDatastore();
            IQueryable<BookInfo> query = database.AsQueryable();


            // Construct the filtering condition based on the criteria         
            string filterExpression = $"{inputs["PropertyName"]}.Any({inputs["PropertyName"]}Item => {inputs["PropertyName"]}Item.{inputs["FieldName"]} {inputs["Condition"]} {inputs["FilterValue"]})";
            
            // Use Dynamic LINQ to filter the data
            query = query.Where(filterExpression);

            // Filtered result in the 'query' variable
            List<BookInfo> filteredResults = query.ToList();
            
            // Update datagrid in inventoryView
            _inventoryPanelViewModel.InventoryDatagrid = filteredResults; 
        }
    }
}
