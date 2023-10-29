using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static BookInventoryAdminProgram.Stores.DatabaseStore;

namespace BookInventoryAdminProgram.Converter
{
    public class MostRecentPricePerUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<PricePerUnitCollection> pricePerUnit)
            {
                if (pricePerUnit.Count > 0)
                {
                    // Assuming that the PricePerUnit list is sorted by SetDate in descending order.
                    return pricePerUnit.OrderByDescending(ppu => ppu.SetDate).FirstOrDefault()?.PricePerUnit.ToString() ?? "0.0";
                }
            }

            return 0.0; // Return a default value if the list is empty.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
