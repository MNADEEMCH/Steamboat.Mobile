using System;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BubbleStack), typeof(BubbleStackRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class BubbleStackRenderer : ViewRenderer
    {
        public BubbleStackRenderer()
        {
            SetNeedsDisplay();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);

            this.LayoutIfNeeded();

            var element = (BubbleStack)Element;
            var layer = this.Layer;

            layer.BackgroundColor = element.FillColor.ToCGColor();

            layer.BorderWidth = 1f;
            layer.BorderColor = element.FillColor.ToCGColor();
            layer.MaskedCorners = CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;
            layer.CornerRadius = element.BorderRadius;
            layer.ShadowOpacity = 0.5f;
            layer.ShadowOffset = new CGSize(0, 2);
            layer.ShadowColor = Color.FromHex("#777777").ToCGColor();
            layer.ShadowRadius = 2f;

            if (element.BorderWidth > 0 && element.BorderColor.A > 0.0)
            {
                this.Layer.BorderWidth = element.BorderWidth;
                this.Layer.BorderColor =
                    new UIKit.UIColor(
                    (nfloat)element.BorderColor.R,
                    (nfloat)element.BorderColor.G,
                    (nfloat)element.BorderColor.B,
                    (nfloat)element.BorderColor.A).CGColor;
            }

            //var shadowLayer = CreateShadowLayer(new CGRect(6, rect.Height- 0.5f, rect.Width - 6, 1.5f));
            //this.Layer.InsertSublayerBelow(shadowLayer, Layer);       
        }

        private CALayer CreateShadowLayer(CGRect rect)
        {
            var shadowLayer = new CALayer();
            shadowLayer.BackgroundColor = Color.Black.ToCGColor();
            shadowLayer.ShadowOpacity = 0.6f;
            shadowLayer.ShadowOffset = new CGSize(0, 1f);
            shadowLayer.ShadowColor = Color.FromHex("#777777").ToCGColor();
            shadowLayer.ShadowPath = UIBezierPath.FromRect(rect).CGPath;

            return shadowLayer;
        }
    }
}
