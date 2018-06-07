using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TapableGrid), typeof(TapableGridRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class TapableGridRenderer : ViewRenderer
    {
        private Color _defaultColor;
        private Label _lbl;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var grid = this.Element as TapableGrid;
            if (grid != null)
            {
                var lbl = grid.Children.Where(x => x is Label).FirstOrDefault();
                if (lbl != null)
                {
                    _lbl = lbl as Label;
                    _defaultColor = _lbl.TextColor;
                }
                BackgroundColor = Element.BackgroundColor.ToUIColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("BackgroundColor"))
            {
                BackgroundColor = Element.BackgroundColor.ToUIColor();
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                var grid = this.Element as TapableGrid;
                _lbl.TextColor = grid.TapColor;
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            _lbl.TextColor = _defaultColor;
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            _lbl.TextColor = _defaultColor;
        }
    }
}
