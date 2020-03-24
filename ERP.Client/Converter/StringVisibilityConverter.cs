using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ERP.Client.Converter
{
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    return Visibility.Collapsed;
                }
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
