using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.View;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShadowStack), typeof(ShadowStackRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class ShadowStackRenderer : ViewRenderer
    {
        private bool drawed = false;

        public ShadowStackRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            DrawElement();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (new string[] { "ShadowColor", "ShadowOpacity" }.Contains(e.PropertyName))
            {
                DrawElement();
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            if (!drawed)
            {
                DrawElement();
                drawed = true;
            }
        }

        private void DrawElement()
        {
            var element = (ShadowStack)Element;
            DrawShape(element);
            DrawShadow();
        }

        private void DrawShape(ShadowStack element)
        {
            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetColors(new int[] { element.BackgroundColor.ToAndroid(), element.BackgroundColor.ToAndroid() });
            backgroundDrawable.SetShape(ShapeType.Rectangle);

            var radius = element.CornerRadius * 2;
            float[] corners = SetCorners(radius);
            backgroundDrawable.SetCornerRadii(corners);
            backgroundDrawable.SetStroke(1, Android.Graphics.Color.White);
            this.SetBackground(backgroundDrawable);
        }

        private static float[] SetCorners(int radius)
        {
            float[] corners = new float[8];

            corners[0] = radius;
            corners[1] = radius;
            corners[2] = radius;
            corners[3] = radius;
            corners[4] = radius;
            corners[5] = radius;
            corners[6] = radius;
            corners[7] = radius;

            return corners;
        }

        private void DrawShadow()
        {
            var element = (ShadowStack)Element;

            ViewCompat.SetElevation(this, element.ShadowOpacity > 0 ? 15 : 0);
            if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
                this.StateListAnimator = null;
        }
    }
}
