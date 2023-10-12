using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookInventoryAdminProgram.Converter
{
    public class BitmapImageToByteArrayConverter
    {
        /// <summary>
        /// Converts <strong>BitmapImage</strong> to <strong>byte[]</strong>
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static byte[] Convert(BitmapImage bitmapImage)
        {
            byte[] byteArray = null;
            // to save processing power
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            // Create a MemoryStream to store the encoded image
            using (MemoryStream stream = new MemoryStream())
            {
                // Create a BitmapFrame from the BitmapImage
                BitmapFrame frame = BitmapFrame.Create(bitmapImage);

                // Encode the frame into the stream using the specified encoder
                encoder.Frames.Add(frame);
                encoder.Save(stream);

                // Get the byte array from the stream
                byteArray = stream.ToArray();
            }

            return byteArray;
        }
    }
}
