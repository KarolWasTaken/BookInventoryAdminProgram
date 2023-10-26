using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Converter;
using BookInventoryAdminProgram.Stores;
using BookInventoryAdminProgram.ViewModel;
using BookInventoryAdminProgram.ViewModel.BookManagerSubViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BookInventoryAdminProgram.View.BookManagerSubViews
{
    /// <summary>
    /// Interaction logic for AddBookView.xaml
    /// </summary>
    public partial class AddBookView : UserControl
    {
        public AddBookView()
        {
            InitializeComponent();
        }

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
                    AddBookViewModel viewModel = ((FrameworkElement)sender).DataContext as AddBookViewModel;
                    SelectFileCommand selectFileCommand = new SelectFileCommand(viewModel);

                    // Create a BitmapImage and set its source to the clipboard image
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ConvertBitmapSourceToStream(clipboardImage);
                    bitmapImage.EndInit();
                    
                    byte[] coverImage = BitmapImageToByteArrayConverter.Convert(bitmapImage);
                    selectFileCommand.SetImage(coverImage);
                }
            }
        }
        private Stream ConvertBitmapSourceToStream(BitmapSource bitmapSource)
        {
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder(); // You can choose an appropriate encoder for your needs
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(stream);
            return stream;
        }

        private void Button_DropFile(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string path = System.IO.Path.GetFullPath(files[0]);

                string extension = System.IO.Path.GetExtension(path).ToLower();
                if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
                {
                    AddBookViewModel viewModel = ((FrameworkElement)sender).DataContext as AddBookViewModel;
                    SelectFileCommand selectFileCommand = new SelectFileCommand(viewModel);
                    BitmapImage coverImageBitmap = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    byte[] coverImage = BitmapImageToByteArrayConverter.Convert(coverImageBitmap);
                    selectFileCommand.SetImage(coverImage);
                    notifier.Text = null;
                }
                else
                    notifier.Text = "Unsupported file format";

            }
        }
    }
}
