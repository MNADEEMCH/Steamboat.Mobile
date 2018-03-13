using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamanimation;
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
            var view = args.View;
            view.Opacity = 0;
            await Task.Delay(1);

            await view.Animate(new Xamanimation.FadeToAnimation()
            {
                Duration = "800",
                Easing = EasingType.CubicOut,
                Opacity = 1
            });
        }
    }
}
