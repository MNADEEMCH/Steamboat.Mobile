using System;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WrappedButton), typeof(WrappedButtonRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class WrappedButtonRenderer : ButtonRenderer
    {
        public WrappedButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Control.SetAllCaps(false);
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

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void UpdatePadding()
        {
            var button = Element as WrappedButton;

            var paddingLeft = DisplayMetricsHelper.DpToPx(this.Context, (int)button.Padding.Left);
            var paddingTop = DisplayMetricsHelper.DpToPx(this.Context, (int)button.Padding.Top);
            var paddingRight = DisplayMetricsHelper.DpToPx(this.Context, (int)button.Padding.Right);
            var paddingBottom = DisplayMetricsHelper.DpToPx(this.Context, (int)button.Padding.Bottom);

            this.Control.SetPadding(
                paddingLeft,
                paddingTop,
                paddingRight,
                paddingBottom
            );
        }
    }
}
