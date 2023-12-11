using BookInventoryAdminProgram.Commands;
using iText.Layout.Splitting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using AdonisUI.Controls;
using System.IO.Packaging;
using BookInventoryAdminProgram.ViewModel;

namespace BookInventoryAdminProgram.Commands
{
    public class SelectFileLocationCommand : CommandBase
    {
        private SalesPanelViewModel _salesPanelViewModel;
        private Action<string> _onPropertyChanged;
        public SelectFileLocationCommand(SalesPanelViewModel salesPanelViewModel, Action<string> onPropertyChanged)
        {
            _salesPanelViewModel = salesPanelViewModel;
            _onPropertyChanged = onPropertyChanged;
        }

        public override void Execute(object? parameter)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _salesPanelViewModel.FileLocation = dialog.FileName;
                //MessageBox.Show("You selected: " + dialog.FileName); debug
            }
            _onPropertyChanged(nameof(_salesPanelViewModel.CanCreatePDF));
        }
    }
}
