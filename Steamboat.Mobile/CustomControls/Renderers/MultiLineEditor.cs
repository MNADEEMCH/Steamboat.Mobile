using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
	public class MultiLineEditor : Editor
    {
		public static BindableProperty PaddingProperty =
			BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(MultiLineEditor), default(Thickness));
		
		public static BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(MultiLineEditor), 0.0);

		public Thickness Padding
        {
			get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

		public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}
