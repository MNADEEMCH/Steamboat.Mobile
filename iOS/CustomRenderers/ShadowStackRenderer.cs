using System;
using System.ComponentModel;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ShadowStack), typeof(ShadowStackRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class ShadowStackRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Draw();
            SetNeedsDisplay();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (new string[] { "BackgroundColor", "ShadowColor", "ShadowOpacity", "CornerRadius", "Height", "Width" }.Contains(e.PropertyName))
            {
                Draw();
                SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            Draw();
        }

        private void Draw()
        {
            var element = (ShadowStack)Element;
            Layer.BackgroundColor = element.BackgroundColor.ToCGColor();
            DrawBorder(element);
            //DrawShadow(element.ShadowColor, element.CornerRadius, element.ShadowOpacity);
        }

        private void DrawBorder(ShadowStack element)
        {
            Layer.MaskedCorners = (CACornerMask)15;

            Layer.CornerRadius = element.CornerRadius;
            Layer.BorderColor = Color.Transparent.ToCGColor();
            Layer.BorderWidth = 1;
        }

        private void DrawShadow(Color color, float cornerRadius, float opacity)
        {
            Layer.ShadowRadius = cornerRadius;
            Layer.ShadowColor = color.ToCGColor();
            Layer.ShadowOffset = new CGSize(0, 4);
            Layer.ShadowOpacity = opacity;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}
