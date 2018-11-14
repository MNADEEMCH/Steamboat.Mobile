using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers;
using Xamanimation;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class ScreeningInterviewView : CustomContentPage
    {
        public ScreeningInterviewView()
        {
            InitializeComponent();
        }

        async void Handle_ItemCreated(object sender, CustomControls.RepeaterControlItemAddedEventArgs args)
        {
            var view = args.View;
            view.Opacity = 0;
            await Task.Delay(1);
            var deviceInfo = DependencyContainer.Resolve<IDeviceInfo>();
            if (deviceInfo.IsAndroid)
            {
                await view.Animate(new Xamanimation.FadeToAnimation()
                {
                    Duration = "400",
                    Easing = EasingType.SinOut,
                    Opacity = 1
                });
            }
            else
            {
                await view.Animate(new Xamanimation.FadeToAnimation()
                {
                    Duration = "800",
                    Easing = EasingType.CubicOut,
                    Opacity = 1
                });
            }
        }
    }
}
