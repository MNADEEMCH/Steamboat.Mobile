using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class WrappedButton : Button
    {
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(WrappedButton), new Thickness());

        public static readonly BindableProperty ActiveProperty =
            BindableProperty.Create(nameof(Active), typeof(bool), typeof(WrappedButton), false, BindingMode.TwoWay);

        public static readonly BindableProperty AutoFitTextProperty =
            BindableProperty.Create(nameof(AutoFitText), typeof(bool), typeof(WrappedButton), false);

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }

        public bool AutoFitText
        {
            get { return (bool)GetValue(AutoFitTextProperty); }
            set { SetValue(AutoFitTextProperty, value); }
        }
    }
}
