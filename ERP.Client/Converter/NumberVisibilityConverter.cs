using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ERP.Client.Converter
{
    public class NumberVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int number)
            {
                if (number > 0)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
