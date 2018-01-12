using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Steamboat.Mobile.Views
{
    public partial class ReportView : ContentPage
    {
        public ReportView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            Stepper.BindingContext = null;
        }
    }
}