﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
	public class MessagingDateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new InvalidOperationException("The target must be a date");
            }
			var ret = ((DateTime)value).ToString("MMMM d, yyyy hh:mm tt", CultureInfo.InvariantCulture);
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
