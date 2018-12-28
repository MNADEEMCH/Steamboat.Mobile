using System;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class LottieView : AnimationView
    {
        #region Properties

        public static readonly BindableProperty PlayCommandProperty =
            BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(LottieView), null, BindingMode.OneWayToSource);

        public static readonly BindableProperty PlayLoopCommandProperty =
            BindableProperty.Create(nameof(PlayLoopCommand), typeof(ICommand), typeof(LottieView), null, BindingMode.OneWayToSource);

        public static readonly BindableProperty EndLoopCommandProperty =
            BindableProperty.Create(nameof(EndLoopCommand), typeof(ICommand), typeof(LottieView), null, BindingMode.OneWayToSource);

        public static readonly BindableProperty LoopThresholdProperty =
            BindableProperty.Create(nameof(LoopThreshold), typeof(int), typeof(LottieView), default(int));

        public static readonly BindableProperty EndThresholdProperty =
            BindableProperty.Create(nameof(EndThreshold), typeof(int), typeof(LottieView), default(int));

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public ICommand PlayLoopCommand
        {
            get { return (ICommand)GetValue(PlayLoopCommandProperty); }
            set { SetValue(PlayLoopCommandProperty, value); }
        }

        public ICommand EndLoopCommand
        {
            get { return (ICommand)GetValue(EndLoopCommandProperty); }
            set { SetValue(EndLoopCommandProperty, value); }
        }

        public int LoopThreshold
        {
            get { return (int)GetValue(LoopThresholdProperty); }
            set { SetValue(LoopThresholdProperty, value); }
        }

        public int EndThreshold
        {
            get { return (int)GetValue(EndThresholdProperty); }
            set { SetValue(EndThresholdProperty, value); }
        }

        #endregion

        public LottieView()
        {
            PlayCommand = new Command((param) => PlayAnimation(param));
            PlayLoopCommand = new Command((param) => PlayLoop(param));
            EndLoopCommand = new Command((param) => EndLoop(param));
        }

        private void PlayAnimation(object param = null)
        {
            if (!IsPlaying)
                Play();
        }

        private void PlayLoop(object param = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Loop = true;
                this.PlayFrameSegment(77, LoopThreshold);
            });
        }

        private void EndLoop(object param = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Pause();
                this.Loop = false;
                this.PlayFrameSegment(LoopThreshold, EndThreshold);
            });
        }
    }
}
