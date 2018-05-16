using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Steamboat.Mobile.CustomControls.LinkButton;

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

            if (this.Control == null || this.Element == null)
                return;

            if (e.NewElement != null)
            {
                Control.Touch += OnTouch;
            }

            HandleButtonIsEnabled();

            ViewCompat.SetElevation(Control, 0);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var button = this.Element as LinkButton;

            if (e.PropertyName == nameof(button.IsEnabled))
            {
                HandleButtonIsEnabled();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Touch -= OnTouch;
            }

            base.Dispose(disposing);
        }

        private void HandleButtonIsEnabled()
        {
            var button = this.Element as LinkButton;

            if (button.IsEnabled)
            {
                Control.SetTextColor(button.ActiveColor.ToAndroid());
                UpdateCustomImage(button.GetImageSourceForStatus(StatusEnum.Active));
            }
            else
            {
                Control.SetTextColor(button.DisabledColor.ToAndroid());
                UpdateCustomImage(button.GetImageSourceForStatus(StatusEnum.Disabled));
            }

        }

        private void UpdateCustomImage(string imageSource)
        {
            var button = this.Element as LinkButton;

            if (!String.IsNullOrEmpty(imageSource))
            {
                Drawable drawable = GetDrawable(imageSource);
                using (var image = drawable)
                {

                    this.Control.SetPadding(0, 0, 0, 0);
                    this.Control.SetCompoundDrawablesWithIntrinsicBounds(image, null, null, null);
                    this.Control.CompoundDrawablePadding = DisplayMetricsHelper.DpToPx(this.Context, (float)button.ImageTextDistance);

                    if (String.IsNullOrEmpty(this.Control.Text) &&
                        button.WidthRequest < 0 && button.HeightRequest < 0)
                    {
                        button.WidthRequest = button.ImageWidth;
                        button.HeightRequest = button.ImageHeight;
                    }
                }
            }
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            var imageName = System.IO.Path.GetFileNameWithoutExtension(imageEntryImage).ToLower();
            int resID = Resources.GetIdentifier(imageName, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var element = this.Element as LinkButton;

            var width = DisplayMetricsHelper.DpToPx(this.Context, element.ImageWidth);
            var height = DisplayMetricsHelper.DpToPx(this.Context, element.ImageHeight);

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, width, height, true));
        }

        private void OnTouch(object sender, TouchEventArgs e)
        {
            var button = this.Element as LinkButton;

            if (e.Event.Action == MotionEventActions.Down)
            {
                Control.SetTextColor(button.TapColor.ToAndroid());
                UpdateCustomImage(button.GetImageSourceForStatus(StatusEnum.Tap));
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                Control.SetTextColor(button.ActiveColor.ToAndroid());
                ((IButtonController)Element)?.SendClicked();
                UpdateCustomImage(button.GetImageSourceForStatus(StatusEnum.Active));
            }
        }
    }
}
