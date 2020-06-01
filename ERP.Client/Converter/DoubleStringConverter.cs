using System;
using Windows.UI.Xaml.Data;

namespace ERP.Client.Converter
{
    public class DoubleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double val)
            {
                return val.ToString("0");
            }

            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
