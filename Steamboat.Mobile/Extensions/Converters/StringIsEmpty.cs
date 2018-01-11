using System;
using System.Globalization;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
    public class StringIsEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                throw new InvalidOperationException("The target must be a string");
            }

            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
