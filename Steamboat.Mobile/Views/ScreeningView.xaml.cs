using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Steamboat.Mobile.Views
{
    public partial class ScreeningView : CustomContentPage
    {
        public ScreeningView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            Stepper.BindingContext = null;
        }
    }
}