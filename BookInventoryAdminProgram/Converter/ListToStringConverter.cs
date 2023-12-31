﻿using BookInventoryAdminProgram.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookInventoryAdminProgram.Converter
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> stringList) 
            {
                // converts List<string> to string of comma seperated values
                List<string> sortedList = FilteringDatabase.MergeSort(stringList);
                return string.Join(", ", sortedList);
            }

            // value was not a list of strings
            return "";
        }
        
        // for non-xaml use
        public static string Convert(List<string> value)
        {
            if (value is List<string> stringList)
            {
                // converts List<string> to string of comma seperated values
                List<string> sortedList = FilteringDatabase.MergeSort(stringList);
                return string.Join(", ", sortedList);
            }

            // value was not a list of strings
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
