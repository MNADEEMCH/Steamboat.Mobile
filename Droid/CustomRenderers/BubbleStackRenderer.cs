using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.View;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BubbleStack), typeof(BubbleStackRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class BubbleStackRenderer : ViewRenderer
    {
        Context _context;
        public BubbleStackRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var stack = this.Element as BubbleStack;
            var radius = stack.BorderRadius * 2;

            stack.BackgroundColor = stack.FillColor;
            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetColors(new int[] { stack.FillColor.ToAndroid(), stack.FillColor.ToAndroid() });
            backgroundDrawable.SetShape(ShapeType.Rectangle);

            float[] corners;
            if (stack.IsLabel)
                corners = new float[] { 0, 0, radius, radius, radius, radius, radius, radius };
            else
                corners = new float[] { radius, radius, radius, radius, 0, 0, radius, radius };

            backgroundDrawable.SetCornerRadii(corners);
            backgroundDrawable.SetStroke(2, stack.BorderColor.ToAndroid());

            this.SetBackground(backgroundDrawable);

            ViewCompat.SetElevation(this, 10);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                this.StateListAnimator = null;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }
    }
}
