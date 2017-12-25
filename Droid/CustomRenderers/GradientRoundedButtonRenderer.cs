using System;
using Android.Graphics.Drawables;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(GradientRoundedButton), typeof(GradientRoundedButtonRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class GradientRoundedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            if (e.NewElement != null)
            {
                Control.Touch += OnTouch;
            }

            var button = this.Element as GradientRoundedButton;

            int[] colors;
            if (button.IsEnabled)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                colors = new int[] { startColor, endColor };
                Control.Elevation = 25;
                Control.SetTextColor(Android.Graphics.Color.White);
            }
            else
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                colors = new int[] { startColor, endColor };
                Control.Elevation = 0;
                Control.SetTextColor(Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledTextColor));
            }

            var gradientDrawable = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);
            gradientDrawable.SetGradientType(GradientType.LinearGradient);
            gradientDrawable.SetShape(ShapeType.Rectangle);
            gradientDrawable.SetCornerRadius(button.AndroidBorderRadius);
            gradientDrawable.SetStroke((int)button.BorderWidth, Android.Graphics.Color.Black);
            Control.Background = gradientDrawable;
            Control.StateListAnimator = null;
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("IsEnabled"))
            {
                var button = this.Element as GradientRoundedButton;

                int[] colors;
                var background = Control.Background as GradientDrawable;
                if (button.IsEnabled)
                {

                    var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                    var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                    colors = new int[] { startColor, endColor };
                    Control.Elevation = 25;
                    Control.SetTextColor(Android.Graphics.Color.White);
                }
                else
                {
                    var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                    var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                    colors = new int[] { startColor, endColor };
                    Control.Elevation = 0;
                    Control.SetTextColor(Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledTextColor));
                }
                background.SetColors(colors);
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

        private void OnTouch(object sender, TouchEventArgs e)
        {
            var button = this.Element as GradientRoundedButton;
            int[] colors;
            var background = Control.Background as GradientDrawable;

            if (e.Event.Action == MotionEventActions.Down)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.ActiveColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.ActiveColor);
                colors = new int[] { startColor, endColor };
                Control.Elevation = 0;
                background.SetColors(colors);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                colors = new int[] { startColor, endColor };
                Control.Elevation = 25;
                background.SetColors(colors);
            }
        }
    }
}
