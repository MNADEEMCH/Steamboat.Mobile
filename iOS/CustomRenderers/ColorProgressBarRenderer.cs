using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Steamboat.Mobile.CustomControls;
using System.ComponentModel;
using Steamboat.Mobile.iOS.CustomRenderers;
using Xamarin.Forms;
using CoreGraphics;

[assembly: ExportRenderer(typeof(ColorProgressBar), typeof(ColorProgressBarRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class ColorProgressBarRenderer : ProgressBarRenderer
    {
        public ColorProgressBarRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control != null)
            {
                UpdateBarColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ColorProgressBar.FilledColorProperty.PropertyName)
            {
                UpdateBarColor();
            }
        }


        private void UpdateBarColor()
        {
            var element = Element as ColorProgressBar;
            Control.TintColor = element.FilledColor.ToUIColor();
            Control.TrackTintColor = element.EmptyColor.ToUIColor();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var element = Element as ColorProgressBar;

            var X = 1.0f;
            var Y = element.ProgressBarHeigth; // This changes the height

            this.Control.Transform = CGAffineTransform.MakeScale(X, Y);
            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;
        }

    }
}