using System;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WrappedButton), typeof(WrappedButtonRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class WrappedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            Control.TitleLabel.TextAlignment = UIKit.UITextAlignment.Center;

            UpdatePadding();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(WrappedButton.Padding))
            {
                UpdatePadding();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void UpdatePadding()
        {
            var button = Element as WrappedButton;

            Control.ContentEdgeInsets = new UIEdgeInsets(
                (int)button.Padding.Top,
                (int)button.Padding.Left,
                (int)button.Padding.Bottom,
                (int)button.Padding.Right
            );
        }

    }
}
