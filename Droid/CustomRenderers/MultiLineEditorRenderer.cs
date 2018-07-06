using System;
using Android.Content;
using Android.Graphics.Drawables;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MultiLineEditor), typeof(MultiLineEditorRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
	public class MultiLineEditorRenderer : EditorRenderer
    {
		Context _localContext;
        
		public MultiLineEditorRenderer(Context context) : base(context)
        {
            _localContext = context;
        }

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				var entry = (Element as MultiLineEditor);
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.White);
                gd.SetCornerRadius((float)(entry.CornerRadius));
                Control.SetBackground(gd);

				var padding = DisplayMetricsHelper.DpToPx(this.Context, (int)entry.Padding.Top);
                Control.SetPadding(padding, padding, padding, padding);
			}
		}
	}
}
