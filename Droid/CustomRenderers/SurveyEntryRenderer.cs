using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SurveyEntry), typeof(SurveyEntryRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class SurveyEntryRenderer:EntryRenderer
    {
        Context _localContext;

        public SurveyEntryRenderer(Context context) : base(context)
        {
            _localContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var entry = (Element as SurveyEntry);
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.White);
                gd.SetCornerRadius((float)(entry.CornerRadius));
                Control.SetBackground(gd);

                var padding = (int)entry.GlobalPadding;
                Control.SetPadding(padding, padding, padding, padding);
            }
        }
    }
}
