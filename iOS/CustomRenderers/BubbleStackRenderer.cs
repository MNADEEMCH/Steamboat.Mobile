using System;
using System.ComponentModel;
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
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
        }

		public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);

            var element = (BubbleStack)Element;
            var layer = this.Layer;

            CACornerMask corners;
            if (element.IsLabel)
                corners = CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;
            else
                corners = CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MinXMinYCorner;

            layer.MaskedCorners = corners;
            layer.CornerRadius = element.BorderRadius;
            layer.ShadowOpacity = 0.5f;
            layer.ShadowOffset = new CGSize(0, 2);
            layer.ShadowColor = Color.FromHex("#777777").ToCGColor();
            layer.ShadowRadius = 2f;
            layer.BackgroundColor = element.FillColor.ToCGColor();
            layer.BorderWidth = 1f;
            layer.BorderColor = element.BorderColor.ToCGColor();
        }
    }
}
