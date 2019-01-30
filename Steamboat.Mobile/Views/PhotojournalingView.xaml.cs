using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Steamboat.Mobile.Views
{
    public partial class PhotojournalingView : CustomContentPage
    {
        bool first;

        public PhotojournalingView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            var tapGestureRecognizer = new TapGestureRecognizer();
            var swipeUpGestureRecognizer = new SwipeGestureRecognizer()
            {
                Direction = SwipeDirection.Up
            };
            var swipeDownGestureRecognizer = new SwipeGestureRecognizer()
            {
                Direction = SwipeDirection.Down
            };
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                FooterTapped();
            };
            swipeUpGestureRecognizer.Swiped += (s, e) =>
            {
                FooterSwippedUp();
            };
            swipeDownGestureRecognizer.Swiped += (s, e) =>
            {
                FooterSwippedDown();
            };
            PhotosBar.GestureRecognizers.Add(tapGestureRecognizer);
            PhotosBar.GestureRecognizers.Add(swipeUpGestureRecognizer);
            PhotosBar.GestureRecognizers.Add(swipeDownGestureRecognizer);
        }

        private bool isCollapsed;

        public bool IsCollapsed
        {
            get => isCollapsed;
            set
            {
                isCollapsed = value;

                if (isCollapsed)
                {
                    PhotosContent.Animate("IncreaseHeight", ChangeHeight, 0, main.ContentSize.Height, 16, 400, Easing.CubicIn);
                    ChevronIcon.RotateTo(-90);
                    this.BackgroundColor = (Color)Xamarin.Forms.Application.Current.Resources["LightGrayColor"];
                }
                else
                {
                    PhotosContent.Animate("DecreaseHeight", ChangeHeight, PhotosContent.Height, 0, 16, 400, Easing.CubicOut);
                    ChevronIcon.RotateTo(90);
                    this.BackgroundColor = (Color)Xamarin.Forms.Application.Current.Resources["SubtleBlueColor"];
                }
            }
        }

        private void ChangeHeight(double input)
        {
            PhotosContent.HeightRequest = input;
        }

        private void FooterTapped()
        {
            IsCollapsed = !IsCollapsed;
        }

        private void FooterSwippedUp()
        {
            if (!isCollapsed)
                FooterTapped();
        }

        private void FooterSwippedDown()
        {
            if (isCollapsed)
                FooterTapped();
        }

    }
}
