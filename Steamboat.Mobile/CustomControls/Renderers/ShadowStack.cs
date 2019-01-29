using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class ShadowStack : StackLayout
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(BubbleStack), 0);

        public static readonly BindableProperty ShadowColorProperty =
            BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(ShadowStack), Color.Default);

        public static readonly BindableProperty ShadowOpacityProperty =
            BindableProperty.Create(nameof(ShadowOpacity), typeof(float), typeof(ShadowStack), .5f);

        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        public float ShadowOpacity
        {
            get { return (float)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}
