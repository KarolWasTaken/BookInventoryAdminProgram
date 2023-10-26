using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookInventoryAdminProgram.Converter
{
    public class BitmapSourceToStreamConverter
    {
        public static Stream Convert(BitmapSource bitmapSource)
        {
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder(); // You can choose an appropriate encoder for your needs
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(stream);
            return stream;
        }
    }
}
