using System;
using System.Globalization;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
    public class EventDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new InvalidOperationException("The target must be a boolean");
            }
            //StringFormat = '{0:hh:mm tt}'
            var ret = ((DateTime)value).ToString("hh:mmtt", CultureInfo.InvariantCulture);
            return ret.ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
