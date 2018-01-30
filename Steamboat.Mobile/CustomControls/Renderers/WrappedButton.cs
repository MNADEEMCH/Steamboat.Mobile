using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class WrappedButton : Button
    {
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(WrappedButton), default(Thickness));

        public static readonly BindableProperty ActiveProperty =
            BindableProperty.Create(nameof(Active), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay);

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
    }
}
