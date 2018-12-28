using System;
using Android.Content;
using Android.Graphics.Drawables;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FeedbackEditor), typeof(FeedbackEditorRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class FeedbackEditorRenderer : EditorRenderer
    {
        public FeedbackEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var entry = (Element as FeedbackEditor);
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(entry.BackgroundColor.ToAndroid());
                gd.SetCornerRadius((float)(entry.CornerRadius));
                gd.SetStroke(entry.BorderWidth, entry.BorderColor.ToAndroid());
                Control.SetBackground(gd);

                var paddingLeft = DisplayMetricsHelper.DpToPx(this.Context, (int)entry.Padding.Left);
                var paddingTop = DisplayMetricsHelper.DpToPx(this.Context, (int)entry.Padding.Top);
                var paddingRight = DisplayMetricsHelper.DpToPx(this.Context, (int)entry.Padding.Right);
                var paddingBottom = DisplayMetricsHelper.DpToPx(this.Context, (int)entry.Padding.Bottom);
                Control.SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);
            }
        }
    
    }
}
