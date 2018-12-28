using System;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FeedbackEditor), typeof(FeedbackEditorRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class FeedbackEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var customControl = (Element as FeedbackEditor);
            var padding = customControl.Padding;

            Control.InputAccessoryView = null;
            Control.Layer.CornerRadius = (nfloat)(customControl.CornerRadius);
            Control.Layer.BorderColor = customControl.BorderColor.ToCGColor();
            Control.Layer.BorderWidth = customControl.BorderWidth;
            Control.TextContainerInset = new UIKit.UIEdgeInsets((nfloat)padding.Top, (nfloat)padding.Left, (nfloat)padding.Bottom, (nfloat)padding.Right);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }
    }
}
