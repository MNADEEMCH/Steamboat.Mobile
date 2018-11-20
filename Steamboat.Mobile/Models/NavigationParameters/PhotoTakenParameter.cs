using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.Models.NavigationParameters
{
    public class PhotoTakenParameter
    {
        public byte[] Media { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
