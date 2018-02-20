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
        private bool _alreadyDrawed = false;
        private CALayer _shadowLayer;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            if (e.NewElement != null)
            {
                var nativeButton = (UIButton)Control;
                nativeButton.TouchDown += OnTouchDown;
                nativeButton.TouchUpOutside += OnTouchUp;
                nativeButton.TouchUpInside += OnTouchUp;
            }

            var button = this.Element as GradientRoundedButton;

            if (button.DisabledTextColor != null)
                Control?.SetTitleColor(button.DisabledTextColor.ToUIColor(), UIControlState.Disabled);

            Control.ReverseTitleShadowWhenHighlighted = false;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("IsEnabled") && _shadowLayer!=null)
            {
                var button = this.Element as GradientRoundedButton;

                if (button.IsEnabled)
                    _shadowLayer.ShadowColor = button.ShadowColorEnabled.ToCGColor();
                else
                    _shadowLayer.ShadowColor = Color.Transparent.ToCGColor();
            }
            SetNeedsDisplay();//Force draw but only once when all elements properties change
        }


        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            if(!_alreadyDrawed){
                
                var button = this.Element as GradientRoundedButton;
                var buttonBounds = new CGRect(0, 0, button.Width, button.Height);
                var shadowBounds = new CGRect(0, 0, button.Width - 1, button.Height - 1);

                _shadowLayer = GetShadowForLayer(button.ActiveColor,
                                                 button.IsEnabled? button.ShadowColorEnabled:Color.Transparent,
                                                 shadowBounds,
                                                 button.IOSBorderRadius);
                
                var imageNormal = GetGradientBackgroundImage(button.StartColor,
                                                                 button.EndColor,
                                                                 buttonBounds,
                                                                 button.IOSBorderRadius);
                var imageTaped = GetGradientBackgroundImage(button.ActiveColor,
                                                               button.ActiveColor,
                                                                   buttonBounds,
                                                                   button.IOSBorderRadius);
                var imageDisabled = GetGradientBackgroundImage(button.DisabledColor,
                                                               button.DisabledColor,
                                                                   buttonBounds,
                                                                   button.IOSBorderRadius);

                Control?.Layer.InsertSublayer(_shadowLayer, 0);
                Control?.SetBackgroundImage(imageNormal, UIControlState.Normal);
                Control?.SetBackgroundImage(imageTaped, UIControlState.Highlighted);
                Control?.SetBackgroundImage(imageDisabled, UIControlState.Disabled);

                _alreadyDrawed = true;
            }

        }

        private CALayer GetShadowForLayer(Color ShadowBackgroundColor,Color ShadowColor, CGRect ShadowBounds, int BorderRadius){

            var shadowLayer = new CALayer();
            shadowLayer.BackgroundColor = ShadowBackgroundColor.ToCGColor();
            shadowLayer.Bounds = ShadowBounds;
            shadowLayer.CornerRadius = Control.Layer.CornerRadius = BorderRadius;
            shadowLayer.ShadowOffset = new CGSize(0, 12);
            shadowLayer.ShadowOpacity = .5f;
            shadowLayer.ShadowRadius = 7;
            shadowLayer.ShadowColor = ShadowColor.ToCGColor();
            shadowLayer.ZPosition = -5;
            shadowLayer.Position = new CGPoint(ShadowBounds.Width / 2, ShadowBounds.Height / 2);

            return shadowLayer;
        }

        private UIImage GetGradientBackgroundImage(Color StartColor,Color EndColor,CGRect GradientBounds,int BorderRadius){

            var gradientLayer = new CAGradientLayer();
            gradientLayer.Bounds = GradientBounds;
            gradientLayer.CornerRadius = Control.Layer.CornerRadius = BorderRadius;
            gradientLayer.StartPoint = new CGPoint(0.0, 0.25);
            gradientLayer.EndPoint = new CGPoint(1.0, 0.5);
            gradientLayer.Colors = new CGColor[] { StartColor.ToCGColor(), EndColor.ToCGColor() };

            UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
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
            _shadowLayer.ShadowColor = Color.Transparent.ToCGColor();
        }

        private void OnTouchUp(object sender, EventArgs e)
        {
            var button = this.Element as GradientRoundedButton;
            _shadowLayer.ShadowColor = button.ShadowColorEnabled.ToCGColor();
        }


    }
}
