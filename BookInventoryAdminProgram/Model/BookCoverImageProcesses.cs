using BookInventoryAdminProgram.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookInventoryAdminProgram.Model
{
    public class BookCoverImageProcesses
    {


        public static byte[] GetNoCoverImage()
        {
            BitmapImage defaultNoCoverImageBitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/BookImages/NoCoverDefault.png"));
            byte[] defaultNoCoverImage = BitmapImageToByteArrayConverter.Convert(defaultNoCoverImageBitmap);
            return defaultNoCoverImage;
        }
    }
}
