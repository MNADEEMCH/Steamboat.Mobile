using System;
using System.Globalization;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
    public class PhotoDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new InvalidOperationException("The target must be a boolean");
            }
            var ret = ((DateTime)value).ToString("MMM", CultureInfo.InvariantCulture);
            return ret.ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
