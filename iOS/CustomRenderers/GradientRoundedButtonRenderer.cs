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

            if (e.OldElement != null || e.NewElement == null)
                return;
            
            if(e.NewElement != null)
            {
                var nativeButton = (UIButton)Control;
                nativeButton.TouchDown += OnTouchDown;
                nativeButton.TouchUpOutside += OnTouchUp;
                nativeButton.TouchUpInside += OnTouchUp;
            }

            var button = this.Element as GradientRoundedButton;
            var gradient = new CAGradientLayer();
            gradient.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
            gradient.StartPoint = new CGPoint(0.0, 0.25);
            gradient.EndPoint = new CGPoint(1.0, 0.5);
            gradient.ShadowOffset = new CGSize(0, 12);
            gradient.ShadowOpacity = 0.5f;
            gradient.ShadowRadius = 7;

            if (button.IsEnabled)
            {
                gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
                gradient.ShadowColor = button.ShadowColorEnabled.ToCGColor();
            }
            else
            {
                gradient.Colors = new CGColor[] { button.DisabledColor.ToCGColor(), button.DisabledColor.ToCGColor() };
                gradient.ShadowColor = Color.Transparent.ToCGColor();
            }

            var layer = Control?.Layer.Sublayers.LastOrDefault();
            Control?.Layer.InsertSublayerBelow(gradient, layer);

            if (button.DisabledTextColor != null)
                Control?.SetTitleColor(button.DisabledTextColor.ToUIColor(), UIControlState.Disabled);
            
            Control.ReverseTitleShadowWhenHighlighted = false;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("IsEnabled"))
            {
                var button = this.Element as GradientRoundedButton;
                var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;

                if (button.IsEnabled)
                {
                    gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
                    gradient.ShadowColor = button.ShadowColorEnabled.ToCGColor();
                }
                else
                {
                    gradient.Colors = new CGColor[] { button.DisabledColor.ToCGColor(), button.DisabledColor.ToCGColor() };
                    gradient.ShadowColor = Color.Transparent.ToCGColor();
                }
                SetNativeControl(Control);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.TouchDown -= OnTouchDown;
                Control.TouchUpOutside -= OnTouchUp;
                Control.TouchUpInside -= OnTouchUp;
            }

            base.Dispose(disposing);
        }

        private void OnTouchDown(object sender, EventArgs e)
        {
            var button = this.Element as GradientRoundedButton;
            var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;
            gradient.Colors = new CGColor[] { button.ActiveColor.ToCGColor(), button.ActiveColor.ToCGColor() };
            gradient.ShadowColor = Color.Transparent.ToCGColor();
        }

        private void OnTouchUp(object sender, EventArgs e)
        {
            var button = this.Element as GradientRoundedButton;
            var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;
            gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
            gradient.ShadowColor = button.ShadowColorEnabled.ToCGColor();
        }
    }
}
