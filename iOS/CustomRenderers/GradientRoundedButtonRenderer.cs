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

        //protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.OldElement != null || e.NewElement == null)
        //        return;

        //    var button = this.Element as GradientRoundedButton;

        //    if(button.IsEnabled)
        //    {
        //        var gradient = new CAGradientLayer();
        //        gradient.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
        //        gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
        //        gradient.StartPoint = new CGPoint(0.0, 0.25);
        //        gradient.EndPoint = new CGPoint(1.0, 0.5);
        //        //var layer = Control?.Layer.Sublayers.LastOrDefault();

        //        gradient.ShadowColor = Color.FromHex("9EC8CA").ToCGColor();
        //        gradient.ShadowOffset = new CGSize(0, 12);
        //        gradient.ShadowOpacity = 0.5f;
        //        gradient.ShadowRadius = 7;

        //        //Control?.Layer.InsertSublayerBelow(gradient, layer);

        //        Control?.Layer.InsertSublayer(gradient, 0);
        //    }
        //    else
        //    {
        //        var solid = new CALayer();
        //        solid.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
        //        Control.BackgroundColor = button.DisabledColor.ToUIColor();
        //        //var layer = Control?.Layer.Sublayers.LastOrDefault();

        //        Control?.Layer.InsertSublayer(solid, 0);
        //    }

        //    if (button.DisabledTextColor != null)
        //        Control?.SetTitleColor(button.DisabledTextColor.ToUIColor(), UIControlState.Disabled);

        //    SetNativeControl(Control);
        //}

        //protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);

        //    if(e.PropertyName.Equals("IsEnabled"))
        //    {
        //        var button = this.Element as GradientRoundedButton;

        //        if (button.IsEnabled)
        //        {
        //            var gradient = new CAGradientLayer();
        //            gradient.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
        //            gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
        //            gradient.StartPoint = new CGPoint(0.0, 0.25);
        //            gradient.EndPoint = new CGPoint(1.0, 0.5);

        //            //var firstLayer = Control?.Layer.Sublayers.FirstOrDefault();
        //            //firstLayer.RemoveFromSuperLayer();

        //            gradient.ShadowColor = Color.FromHex("9EC8CA").ToCGColor();
        //            gradient.ShadowOffset = new CGSize(0, 12);
        //            gradient.ShadowOpacity = 0.5f;
        //            gradient.ShadowRadius = 7;

        //            //var layer = Control?.Layer.Sublayers.LastOrDefault();
        //            //Control?.Layer.InsertSublayerBelow(gradient, layer);
        //            Control?.Layer.ReplaceSublayer(Control?.Layer.Sublayers[0], gradient);
        //        }
        //        else
        //        {
        //            var solid = new CALayer();
        //            solid.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
        //            Control.BackgroundColor = button.DisabledColor.ToUIColor();
        //            //var firstLayer = Control?.Layer.Sublayers.FirstOrDefault();
        //            //firstLayer.RemoveFromSuperLayer();

        //            //var layer = Control?.Layer.Sublayers.LastOrDefault();
        //            //Control?.Layer.InsertSublayerBelow(solid, layer);

        //            Control?.Layer.ReplaceSublayer(Control?.Layer.Sublayers[0], solid);
        //        }
        //        SetNativeControl(Control);
        //    }
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var button = this.Element as GradientRoundedButton;
            var gradient = new CAGradientLayer();
            gradient.CornerRadius = Control.Layer.CornerRadius = button.IOSBorderRadius;
            gradient.StartPoint = new CGPoint(0.0, 0.25);
            gradient.EndPoint = new CGPoint(1.0, 0.5);
            gradient.ShadowOffset = new CGSize(0, 12);
            gradient.ShadowOpacity = 0.5f;
            gradient.ShadowRadius = 7;

            if(button.IsEnabled)
            {               
                gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
                gradient.ShadowColor = Color.FromHex("9EC8CA").ToCGColor();
            }
            else
            {
                gradient.Colors = new CGColor[] { button.DisabledColor.ToCGColor(), button.DisabledColor.ToCGColor()};
                gradient.ShadowColor = Color.FromHex("FFFFFF").ToCGColor();
            }

            var layer = Control?.Layer.Sublayers.LastOrDefault();
            Control?.Layer.InsertSublayerBelow(gradient, layer);

            if (button.DisabledTextColor != null)
                Control?.SetTitleColor(button.DisabledTextColor.ToUIColor(), UIControlState.Disabled);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName.Equals("IsEnabled"))
            {
                var button = this.Element as GradientRoundedButton;
                var gradient = Control?.Layer.Sublayers[0] as CAGradientLayer;

                if (button.IsEnabled)
                {                   
                    gradient.Colors = new CGColor[] { button.StartColor.ToCGColor(), button.EndColor.ToCGColor() };
                    gradient.ShadowColor = Color.FromHex("9EC8CA").ToCGColor();
                }
                else
                {
                    gradient.Colors = new CGColor[] { button.DisabledColor.ToCGColor(), button.DisabledColor.ToCGColor() };
                    gradient.ShadowColor = Color.FromHex("FFFFFF").ToCGColor();
                }
                SetNativeControl(Control);
            }
        }
    }
}
