using System;
using System.Linq;
using Android.Content;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TapableGrid), typeof(TapableGridRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class TapableGridRenderer : ViewRenderer
    {
        private Color _defaultColor;
        private Label _lbl;

        public TapableGridRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var grid = this.Element as TapableGrid;
            if (grid != null)
            {
                var lbl = grid.Children.Where(x => x is Label).FirstOrDefault();
                if (lbl != null)
                {
                    _lbl = lbl as Label;
                    _defaultColor = _lbl.TextColor;
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                var grid = this.Element as TapableGrid;
                _lbl.TextColor = grid.TapColor;
            }
            else if (e.Action == MotionEventActions.Up)
            {
                _lbl.TextColor = _defaultColor;
            }

            return base.OnTouchEvent(e);
        }
    }
}
