using System;
using System.Globalization;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
    public class TextChangedEventArgsConverter: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textChangedEventArgs = (TextChangedEventArgs)value;
            return textChangedEventArgs.NewTextValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
