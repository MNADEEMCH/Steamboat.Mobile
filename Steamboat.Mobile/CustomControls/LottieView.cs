using System;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class LottieView : AnimationView
    {
        public static readonly BindableProperty PlayCommandProperty =
            BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(LottieView), null, BindingMode.OneWayToSource);

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public LottieView()
        {
            PlayCommand = new Command((param) => PlayAnimation(param));
        }

        private void PlayAnimation(object param = null)
        {
            if (!IsPlaying)
                Play();
        }
    }
}
