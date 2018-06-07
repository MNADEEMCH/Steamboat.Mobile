using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class TapableGrid : Grid
    {
        public static readonly BindableProperty TapColorProperty =
            BindableProperty.Create(nameof(TapColor), typeof(Color), typeof(GradientRoundedButton), Color.White);

        public Color TapColor
        {
            get { return (Color)GetValue(TapColorProperty); }
            set { SetValue(TapColorProperty, value); }
        }
    }
}
