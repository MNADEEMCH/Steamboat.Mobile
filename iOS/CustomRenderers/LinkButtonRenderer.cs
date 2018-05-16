using System;
using System.Drawing;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static Steamboat.Mobile.CustomControls.LinkButton;

[assembly: ExportRenderer(typeof(LinkButton), typeof(LinkButtonRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class LinkButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            SetTextColorForStatus();
            SetImagesForStatus();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var button = this.Element as LinkButton;

            if (e.PropertyName == nameof(button.IsEnabled))
            {
                SetImagesForStatus();
            }
            else if (e.PropertyName == nameof(button.Height))
            {
                button.WidthRequest = button.Width + button.ImageTextDistance;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var button = this.Element as LinkButton;
            if (!string.IsNullOrEmpty(Control.TitleLabel.Text) && !string.IsNullOrEmpty(button.GetImageSourceForStatus(StatusEnum.Active)))
                button.WidthRequest = button.Width + button.ImageTextDistance;
        }

        private void SetTextColorForStatus()
        {
            var button = this.Element as LinkButton;

            Control.SetTitleColor(button.ActiveColor.ToUIColor(), UIControlState.Normal);
            Control.SetTitleColor(button.TapColor.ToUIColor(), UIControlState.Highlighted);
            Control.SetTitleColor(button.TapColor.ToUIColor(), UIControlState.Selected);
            Control.SetTitleColor(button.DisabledColor.ToUIColor(), UIControlState.Disabled);
        }

        private void SetImagesForStatus()
        {
            var button = this.Element as LinkButton;

            if (!string.IsNullOrEmpty(button.GetImageSourceForStatus(StatusEnum.Active)))
            {
                Control.SetImage(new UIKit.UIImage(button.GetImageSourceForStatus(StatusEnum.Active)).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
                Control.SetImage(new UIKit.UIImage(button.GetImageSourceForStatus(StatusEnum.Tap)).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Highlighted);
                Control.SetImage(new UIKit.UIImage(button.GetImageSourceForStatus(StatusEnum.Tap)).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Selected);
                Control.SetImage(new UIKit.UIImage(button.GetImageSourceForStatus(StatusEnum.Disabled)).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Disabled);

                if (!string.IsNullOrEmpty(Control.TitleLabel.Text))
                    Control.TitleEdgeInsets = new UIEdgeInsets(0, (nfloat)button.ImageTextDistance, 0, 0);
                else{
                    button.HeightRequest = button.ImageHeight;
                    button.WidthRequest = button.ImageWidth;
                }
            }
        }
    }
}
