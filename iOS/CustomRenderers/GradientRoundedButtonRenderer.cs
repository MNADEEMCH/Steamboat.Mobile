using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientRoundedButton), typeof(GradientRoundedButtonRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class GradientRoundedButtonRenderer : ButtonRenderer
    {
        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                if (value.Width > 0 && value.Height > 0)
                {
                    foreach (var layer in Control?.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
                        layer.Frame = new CGRect(0, 0, value.Width, value.Height);
                }
                base.Frame = value;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = this.Element as GradientRoundedButton;

                var gradient = new CAGradientLayer();
                gradient.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
                gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
                gradient.StartPoint = new CGPoint(0.0, 0.25);
                gradient.EndPoint = new CGPoint(1.0, 0.5);
                var layer = Control?.Layer.Sublayers.LastOrDefault();

                gradient.ShadowColor = Color.FromHex("9EC8CA").ToCGColor();
                gradient.ShadowOffset = new CGSize(0, 12);
                gradient.ShadowOpacity = 0.5f;
                gradient.ShadowRadius = 7;

                Control?.Layer.InsertSublayerBelow(gradient, layer);
            }
        }
    }
}
