using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views.Survey
{
    public partial class SurveySelectManyView : ContentView
    {
        public static BindableProperty ParentBindingContextProperty =
            BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(SurveySelectManyView), null, BindingMode.OneWay);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }

        public SurveySelectManyView()
        {
            InitializeComponent();
        }
	}
}
