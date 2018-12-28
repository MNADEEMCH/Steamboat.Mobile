using System;
using System.Windows.Input;
using FFImageLoading.Forms;
using Xamanimation;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class AnimatedPhoto : CachedImage
    {
        public static readonly BindableProperty AnimatePhotoCommandProperty =
            BindableProperty.Create(nameof(AnimatePhotoCommand), typeof(ICommand), typeof(LottieView), null, BindingMode.OneWayToSource);

        public ICommand AnimatePhotoCommand
        {
            get { return (ICommand)GetValue(AnimatePhotoCommandProperty); }
            set { SetValue(AnimatePhotoCommandProperty, value); }
        }

        public AnimatedPhoto()
        {
            AnimatePhotoCommand = new Command((param) => AnimatePhoto(param));
        }

        private void AnimatePhoto(object param = null)
        {
            this.Animate(new Xamanimation.ScaleToAnimation()
            {
                Duration = "800",
                Easing = EasingType.SinInOut,
                Scale = 0.92
            });
        }
    }
}
