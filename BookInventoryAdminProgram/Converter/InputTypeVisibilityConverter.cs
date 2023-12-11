using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookInventoryAdminProgram.Converter
{
    internal class InputTypeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int selectedType = (int)value;
            int targetType2 = int.Parse(parameter.ToString()); // lazy ass naming will fix later 

            return selectedType == targetType2 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
