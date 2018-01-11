using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class InterviewView : ContentPage
    {
        public InterviewView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            Stepper.BindingContext = null;
        }
    }
}
