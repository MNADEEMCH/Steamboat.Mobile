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

            var gradientEnabled = new CAGradientLayer();
            gradientEnabled.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
            gradientEnabled.StartPoint = new CGPoint(0.0, 0.25);
            gradientEnabled.EndPoint = new CGPoint(1.0, 0.5);
            gradientEnabled.ShadowOffset = new CGSize(0, 12);
            gradientEnabled.ShadowOpacity = 0.5f;
            gradientEnabled.ShadowRadius = 7;
            gradientEnabled.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
            gradientEnabled.ShadowColor = button.ShadowColorEnabled.ToCGColor();

            var gradientDisabled = new CAGradientLayer();
            gradientDisabled.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
            gradientDisabled.StartPoint = new CGPoint(0.0, 0.25);
            gradientDisabled.EndPoint = new CGPoint(1.0, 0.5);
            gradientDisabled.ShadowOffset = new CGSize(0, 12);
            gradientDisabled.ShadowOpacity = 0.5f;
            gradientDisabled.ShadowRadius = 7;
            gradientDisabled.Colors = new CGColor[] { button.DisabledColor.ToCGColor(), button.DisabledColor.ToCGColor() };
            gradientDisabled.ShadowColor = button.ShadowColorEnabled.ToCGColor();

            UIGraphics.BeginImageContext(gradientEnabled.Bounds.Size);
            gradientEnabled.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage imageEnabled = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIGraphics.BeginImageContext(gradientDisabled.Bounds.Size);
            gradientDisabled.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage imageDisabled = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var layer = Control?.Layer.Sublayers.LastOrDefault();


            Control?.SetBackgroundImage(imageEnabled,UIControlState.Normal);
            Control?.SetBackgroundImage(imageDisabled, UIControlState.Disabled);

            if (button.DisabledTextColor != null)
                Control?.SetTitleColor(button.DisabledTextColor.ToUIColor(), UIControlState.Disabled);
            
            Control.ReverseTitleShadowWhenHighlighted = false;
        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            /*if (e.PropertyName.Equals("IsEnabled"))
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
            }*/
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
            /*var button = this.Element as GradientRoundedButton;
            var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;
            gradient.Colors = new CGColor[] { button.ActiveColor.ToCGColor(), button.ActiveColor.ToCGColor() };
            gradient.ShadowColor = Color.Transparent.ToCGColor();*/
        }

        private void OnTouchUp(object sender, EventArgs e)
        {
            /*var button = this.Element as GradientRoundedButton;
            var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;
            gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
            gradient.ShadowColor = button.ShadowColorEnabled.ToCGColor();*/
        }
    }
}
