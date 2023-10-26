using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace BookInventoryAdminProgram.View.BookManagerSubViews
{
    /// <summary>
    /// Interaction logic for ModifyBookView.xaml
    /// </summary>
    public partial class ModifyBookView : UserControl
    {
        public ModifyBookView()
        {
            InitializeComponent();
        }

        // Reduce repeat code between here and addbookview.xaml.cs

        private void CtrlV(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                // MessageBox.Show("hi");
                // Check for Ctrl+V
                if (Clipboard.ContainsImage())
                {
                    // Get the image data from the clipboard
                    BitmapSource clipboardImage = Clipboard.GetImage();
                    if (clipboardImage == null)
                        return;

                    // grabs current datacontext (the viewmodel)
                    ModifyBookViewModel viewModel = ((FrameworkElement)sender).DataContext as ModifyBookViewModel;
                    SelectFileCommand selectFileCommand = new SelectFileCommand(viewModel);

                    // Create a BitmapImage and set its source to the clipboard image
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = BitmapSourceToStreamConverter.Convert(clipboardImage);
                    bitmapImage.EndInit();

                    byte[] coverImage = BitmapImageToByteArrayConverter.Convert(bitmapImage);
                    selectFileCommand.SetImage(coverImage, "ModifyViewModel");
                }
            }
        }
        private void Button_DropFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string path = System.IO.Path.GetFullPath(files[0]);

                string extension = System.IO.Path.GetExtension(path).ToLower();
                if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
                {
                    ModifyBookViewModel viewModel = ((FrameworkElement)sender).DataContext as ModifyBookViewModel;
                    SelectFileCommand selectFileCommand = new SelectFileCommand(viewModel);
                    BitmapImage coverImageBitmap = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    byte[] coverImage = BitmapImageToByteArrayConverter.Convert(coverImageBitmap);
                    selectFileCommand.SetImage(coverImage, "ModifyViewModel");
                    //notifier.Text = null;
                }
                //else
                    //notifier.Text = "Unsupported file format";

            }
        }

        
        private void KeyDownInTextBox(object sender, KeyEventArgs e)
        {
            ModifyBookViewModel viewModel = ((FrameworkElement)sender).DataContext as ModifyBookViewModel;
            viewModel.EditMade = true;
            viewModel.ListBoxEnabled = false;
            viewModel.UpdateProperty(nameof(viewModel.EditMade));
            viewModel.UpdateProperty(nameof(viewModel.ListBoxEnabled));
        }
    }
}
