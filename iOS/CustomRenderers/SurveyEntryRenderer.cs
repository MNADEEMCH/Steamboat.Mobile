using System;
using CoreGraphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SurveyEntry), typeof(SurveyEntryRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class SurveyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null || e.NewElement == null)
                return;

            var entry = (Element as SurveyEntry);

            var padding = entry.GlobalPadding;
            Control.LeftView = new UIView(new CGRect(0, 0, padding, 0));
            Control.LeftViewMode = UITextFieldViewMode.Always;
            Control.RightView = new UIView(new CGRect(0, 0, padding, 0));
            Control.RightViewMode = UITextFieldViewMode.Always;
            Control.Layer.CornerRadius = (nfloat)(entry.CornerRadius);
        }

		public override void Draw(CGRect rect)
		{
            var padding = (Element as SurveyEntry).GlobalPadding;
            this.Element.HeightRequest += rect.Height+2 * padding;
            base.Draw(rect);
		}
	}
}

