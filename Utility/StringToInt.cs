using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace wfc_demo.Utility
{
    [ValueConversion(typeof(string), typeof(int))]
    internal class StringToInt : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sv = value as string;
            if (string.IsNullOrEmpty(sv)) return 0;
            return Convert.ToInt32(sv);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int iv = (int)value;
            return iv.ToString();
        }
    }
}
