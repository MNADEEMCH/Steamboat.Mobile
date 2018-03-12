using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class PreScreeningInterviewView : CustomContentPage
    {
        public PreScreeningInterviewView()
        {
            InitializeComponent();
        }

        async Task Handle_ItemCreated(object sender, CustomControls.RepeaterControlItemAddedEventArgs args)
        {
            args.View.Opacity = 0;
            await Task.Delay(1);
            //await args.View.LayoutTo(new Rectangle(500, args.View.Y, args.View.Width, args.View.Height), 0);

            //var translateAnimation = args.View.LayoutTo(new Rectangle(0, args.View.Y, args.View.Width, args.View.Height), 500, Easing.CubicInOut);
            var fadeInAnimation = args.View.FadeTo(1, 500, Easing.Linear);
            await Task.WhenAll(fadeInAnimation);
        }
    }
}
