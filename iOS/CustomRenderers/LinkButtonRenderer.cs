using System;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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

            var button = this.Element as LinkButton;
            Control.SetTitleColor(button.ActiveColor.ToUIColor(), UIControlState.Normal);
            Control.SetTitleColor(button.ActiveColor.ToUIColor(), UIControlState.Highlighted);
            Control.SetTitleColor(button.ActiveColor.ToUIColor(), UIControlState.Selected);

            Control.AdjustsImageWhenHighlighted = false;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
