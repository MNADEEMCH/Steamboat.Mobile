using System;
using Android.Graphics.Drawables;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Support.V4.View;
using Android.Content;
using Android.OS;

[assembly: ExportRenderer(typeof(GradientRoundedButton), typeof(GradientRoundedButtonRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class GradientRoundedButtonRenderer : ButtonRenderer
    {
        public GradientRoundedButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            if (e.NewElement != null)
            {
                Control.SetOnTouchListener(new TouchListener(this));
            }

            var button = this.Element as GradientRoundedButton;

            int[] colors;
            if (button.IsEnabled)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                colors = new int[] { startColor, endColor };
                ViewCompat.SetElevation(Control, 25);
                Control.SetTextColor(Android.Graphics.Color.White);
            }
            else
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                colors = new int[] { startColor, endColor };
                ViewCompat.SetElevation(Control, 0);
                Control.SetTextColor(Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledTextColor));
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Control.StateListAnimator = null;
            }

            var gradientDrawable = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);
            gradientDrawable.SetGradientType(GradientType.LinearGradient);
            gradientDrawable.SetShape(ShapeType.Rectangle);
            gradientDrawable.SetCornerRadius(button.AndroidBorderRadius);
            gradientDrawable.SetStroke((int)button.BorderWidth, Android.Graphics.Color.Black);
            Control.Background = gradientDrawable;
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
                    ViewCompat.SetElevation(Control, 25);
                    Control.SetTextColor(Android.Graphics.Color.White);
                }
                else
                {
                    var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                    var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledColor);
                    colors = new int[] { startColor, endColor };
                    ViewCompat.SetElevation(Control, 0);
                    Control.SetTextColor(Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.DisabledTextColor));
                }
                background.SetColors(colors);
            }
        }

        public void ButtonTouched(Android.Views.View v, MotionEvent e)
        {
            var button = this.Element as GradientRoundedButton;
            int[] colors;
            var background = Control.Background as GradientDrawable;

            if (e.Action == MotionEventActions.Down)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.ActiveColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.ActiveColor);
                colors = new int[] { startColor, endColor };
                ViewCompat.SetElevation(Control, 0);
                background.SetColors(colors);
            }
            else if (e.Action == MotionEventActions.Up)
            {
                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                colors = new int[] { startColor, endColor };
                ViewCompat.SetElevation(Control, 25);
                background.SetColors(colors);
                ((IButtonController)Element)?.SendClicked();
            }
        }

        private class TouchListener : Java.Lang.Object, IOnTouchListener
        {
            private GradientRoundedButtonRenderer _buttonRenderer;

            public TouchListener(GradientRoundedButtonRenderer buttonRenderer)
            {
                _buttonRenderer = buttonRenderer;
            }

            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                _buttonRenderer.ButtonTouched(v, e);
                return true;
            }

        }
    }
}
