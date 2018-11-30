using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomContentPage), typeof(CustomContentPageRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CustomContentPageRenderer : PageRenderer
    {
        private Context _context;

        public CustomContentPageRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            var element = this.Element as CustomContentPage;
            if (!element.ShowStatusBar)
            {
                var activity = (Activity)_context;
                activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
            }
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            var element = this.Element as CustomContentPage;
            if (!element.ShowStatusBar)
            {
                var activity = (Activity)_context;
                activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            }
        }

    }
}
