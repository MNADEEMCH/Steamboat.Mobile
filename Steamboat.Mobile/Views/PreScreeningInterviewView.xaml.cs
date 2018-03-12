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
            
            /*
             * Native animations without Xamanimation Library
             * args.View.Opacity = 0;
            await Task.Delay(1);
            await args.View.LayoutTo(new Rectangle(500, args.View.Y, args.View.Width, args.View.Height), 0);

            var translateAnimation = args.View.LayoutTo(new Rectangle(0, args.View.Y, args.View.Width, args.View.Height), 500, Easing.CubicInOut);
            var fadeInAnimation = args.View.FadeTo(1, 500, Easing.Linear);
            await Task.WhenAll(fadeInAnimation);
            */

            args.View.Opacity = 0;
            await Task.Delay(1);
            var view = args.View;

            await view.Animate(new Xamanimation.TranslateToAnimation()
            {
                Duration = "1",
                Easing = EasingType.Linear,
                TranslateX = -500,
                TranslateY = 0
            });

            var translate =  view.Animate(new Xamanimation.TranslateToAnimation()
            {
                Duration = "500",
                Easing = EasingType.Linear,
                TranslateX = 0,
                TranslateY = 0
            });

            var fadeIn = view.Animate(new Xamanimation.FadeToAnimation()
            {
                Duration="800",
                Easing = EasingType.Linear,
                Opacity = 1
            });

            await Task.WhenAll(fadeIn, translate);
        }
    }
}
