using System;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ScrollView), typeof(CustomScrollViewRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if(UIDevice.CurrentDevice.CheckSystemVersion(11,0))
            {
                this.ContentInsetAdjustmentBehavior = UIKit.UIScrollViewContentInsetAdjustmentBehavior.Never;   
            }
        }
    }
}
