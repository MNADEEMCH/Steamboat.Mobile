using System;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CustomFrameRenderer : FrameRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            SetupLayer();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CustomFrame.IsActive))
            {
                SetupLayer();
            }
        }

        public override void Draw(CGRect rect)
        {
            SetupLayer();
            base.Draw(rect);
        }

        void SetupLayer()
        {
            var element = Element as CustomFrame;

            Layer.CornerRadius = element.CornerRadius;
            Layer.BackgroundColor = ToCGColor(element.BackgroundColor);

            Layer.BorderWidth = (float)(element.IsActive ? element.ActiveBorderWidth : element.DefaultBorderWidth);
            Layer.BorderColor = element.IsActive ? ToCGColor(element.ActiveOutlineColor) : ToCGColor(element.OutlineColor);

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }

        private CGColor ToCGColor(Color color)
        {
            UIKit.UIColor uicolor = new UIKit.UIColor(
            (System.nfloat)color.R,
            (System.nfloat)color.G,
            (System.nfloat)color.B,
            (System.nfloat)color.A);
            return uicolor.CGColor;
        }

    }
}
