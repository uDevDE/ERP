using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace ERP.Client.Converter
{
    public class DecimalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal val)
            {
                var culture = CultureInfo.CreateSpecificCulture("de-DE");
                return val.ToString("N", culture);
            }

            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
