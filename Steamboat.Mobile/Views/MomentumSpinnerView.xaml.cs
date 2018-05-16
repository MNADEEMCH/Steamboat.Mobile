﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class MomentumSpinnerView : ContentView
    {
        public MomentumSpinnerView()
        {
            InitializeComponent();

            AnimateSpinner();
        }

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName.Equals("IsVisible") && this.IsVisible)
            {
                AnimateSpinner();
            }
        }

        private void AnimateSpinner()
        {
            SpinnerGif.Scale = 0;
            SpinnerLogo.Scale = 0;
            Task.Run(async () =>
            {
                await Task.Delay(1);

                await Task.WhenAll(
                    SpinnerGif.Animate(new Xamanimation.ScaleToAnimation()
                    {
                        Duration = "400",
                        Easing = EasingType.CubicOut,
                        Scale = 1
                    }),
                    SpinnerLogo.Animate(new Xamanimation.ScaleToAnimation()
                    {
                        Duration = "400",
                        Easing = EasingType.CubicIn,
                        Scale = 1
                    })
                );
            });
        }
    }
}
