using System;
using Android.Graphics.Drawables;
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

            if (Control != null)
            {
                var button = this.Element as GradientRoundedButton;

                var startColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.StartColor);
                var endColor = Xamarin.Forms.Platform.Android.ColorExtensions.ToAndroid(button.EndColor);
                int[] colors = new int[] { startColor, endColor };
                var gradientDrawable = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);
                gradientDrawable.SetGradientType(GradientType.LinearGradient);
                gradientDrawable.SetShape(ShapeType.Rectangle);
                gradientDrawable.SetCornerRadius(button.AndroidBorderRadius);
                gradientDrawable.SetStroke((int)button.BorderWidth, Android.Graphics.Color.Black);
                Control.Background = gradientDrawable;
                Control.StateListAnimator = null;
                Control.Elevation = 25;
            }
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }
    }
}
