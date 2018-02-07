using System;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LinkButton), typeof(LinkButtonRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class LinkButtonRenderer : ButtonRenderer
    {
        public LinkButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            if (e.NewElement != null)
            {
                Control.Touch += OnTouch;
            }

            var button = this.Element as LinkButton;
            Control.SetTextColor(button.ActiveColor.ToAndroid());
            ViewCompat.SetElevation(Control, 0);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Touch -= OnTouch;
            }

            base.Dispose(disposing);
        }

        private void OnTouch(object sender, TouchEventArgs e)
        {
            var button = this.Element as LinkButton;

            if (e.Event.Action == MotionEventActions.Down)
            {
                Control.SetTextColor(button.TapColor.ToAndroid());
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                Control.SetTextColor(button.ActiveColor.ToAndroid());
                ((IButtonController)Element)?.SendClicked();
            }
        }
    }
}
