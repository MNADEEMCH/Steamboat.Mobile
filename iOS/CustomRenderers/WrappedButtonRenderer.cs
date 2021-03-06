using System;
using CoreGraphics;
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

            //UpdatePadding();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var element = sender as WrappedButton;

            if (e.PropertyName == nameof(WrappedButton.Padding))
            {
                //UpdatePadding();
            }
            if (element.AutoFitText && e.PropertyName == nameof(WrappedButton.Height))
            {
                if (element.Width > 0)
                {
                    var width = Element.Width;
                    var size = Control.TitleLabel.SizeThatFits(new CoreGraphics.CGSize(width, 100000));
                    var paddings = element.Padding.Top + element.Padding.Bottom;
                    if (size.Height+paddings > element.Height)
                    {
                        Element.HeightRequest = size.Height + paddings;
                    }
                }
            }
        }

		public override void Draw(CGRect rect)
        {   
            var element = Element as WrappedButton;
            if (element.AutoFitText) { 
                element.HeightRequest = Control.TitleLabel.Bounds.Height + element.Padding.Top + element.Padding.Bottom;
            }
            base.Draw(rect);
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
