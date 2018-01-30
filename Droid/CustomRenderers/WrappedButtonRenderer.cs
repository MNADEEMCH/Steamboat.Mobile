using System;
using Android.Support.V4.View;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WrappedButton), typeof(WrappedButtonRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class WrappedButtonRenderer : ButtonRenderer
    {
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

            this.Control.SetPadding(
                (int)button.Padding.Left,
                (int)button.Padding.Top,
                (int)button.Padding.Right,
                (int)button.Padding.Bottom
            );
        }
    }
}
