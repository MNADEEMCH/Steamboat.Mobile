using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views.Survey
{
    public partial class SurveySelectOneView : ContentView
    {
        public static BindableProperty ParentBindingContextProperty =
            BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(SurveySelectOneView), null, BindingMode.OneWay);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }

        public SurveySelectOneView()
        {
            InitializeComponent();
        }
    }
}
