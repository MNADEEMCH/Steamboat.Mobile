using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class LinkButton : Button
    {
        public static readonly BindableProperty ActiveColorProperty =
            BindableProperty.Create(nameof(ActiveColor), typeof(Color), typeof(GradientRoundedButton), default(Color));

        public static readonly BindableProperty TapColorProperty =
            BindableProperty.Create(nameof(TapColor), typeof(Color), typeof(GradientRoundedButton), default(Color));

        public Color ActiveColor
        {
            get { return (Color)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        public Color TapColor
        {
            get { return (Color)GetValue(TapColorProperty); }
            set { SetValue(TapColorProperty, value); }
        }
    }
}
