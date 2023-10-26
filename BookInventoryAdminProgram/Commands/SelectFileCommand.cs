using AdonisUI.Controls;
using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookInventoryAdminProgram.Commands
{
    public class SelectFileCommand : CommandBase
    {
        private AddBookViewModel _addBookViewModel;
        private ModifyBookViewModel _modifyBookViewModel;

        public SelectFileCommand(AddBookViewModel addBookViewModel)
        {
            _addBookViewModel = addBookViewModel;
        }
        public SelectFileCommand(ModifyBookViewModel modifyBookViewModel)
        {
            _modifyBookViewModel = modifyBookViewModel;
        }

        public override void Execute(object? parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                BitmapImage coverImageBitmap = new BitmapImage(new Uri(selectedFilePath, UriKind.RelativeOrAbsolute));
                byte[] coverImage = BitmapImageToByteArrayConverter.Convert(coverImageBitmap);
                SetImage(coverImage, parameter.ToString());
            }
        }
        public void SetImage(byte[] image, string viewModelType)
        {
            if(viewModelType == "AddViewModel")
            {
                _addBookViewModel.BookCover = image;
                _addBookViewModel.DragDropUIVisibility = false;
                _addBookViewModel.ImageVisibility = true;
            }
            else if (viewModelType == "ModifyViewModel")
            {
                _modifyBookViewModel.BookCover = image;    
            }
        }
    }
}
