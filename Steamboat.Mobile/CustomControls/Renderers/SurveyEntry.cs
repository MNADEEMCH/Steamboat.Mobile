using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class SurveyEntry:Entry
    {
        public static BindableProperty GlobalPaddingProperty = 
			BindableProperty.Create(nameof(GlobalPadding), typeof(double), typeof(SurveyEntry), 0.0);

        public static BindableProperty CornerRadiusProperty = 
			BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(SurveyEntry), 0.0);

        public double GlobalPadding
        {
            get { return (double)GetValue(GlobalPaddingProperty); }
            set { SetValue(GlobalPaddingProperty, value); }
        }

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public SurveyEntry()
        {
        }
    }
}
