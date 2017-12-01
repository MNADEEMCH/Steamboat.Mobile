using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class GradientRoundedButton : Button
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public int IOSBorderRadius { get; set; }
        public int AndroidBorderRadius { get; set; }

        public GradientRoundedButton()
        {
        }
    }
}
