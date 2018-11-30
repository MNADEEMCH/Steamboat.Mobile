using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Graphics.Drawable;

[assembly: ExportRenderer(typeof(ColorProgressBar), typeof(ColorProgressBarRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class ColorProgressBarRenderer : ProgressBarRenderer
    {
        public ColorProgressBarRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control != null)
            {
                UpdateBarColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ColorProgressBar.FilledColorProperty.PropertyName)
            {
                UpdateBarColor();
            }

            if(e.PropertyName.Equals("Progress")){

                if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M){
                    Control.Invalidate();
                }
            }
        }



        private void UpdateBarColor()
        {
            var element = Element as ColorProgressBar;
            // http://stackoverflow.com/a/29199280

            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
            {
                Drawable progressDrawable = DrawableCompat.Wrap(Control.ProgressDrawable);
                DrawableCompat.SetTintList(progressDrawable, Android.Content.Res.ColorStateList.ValueOf(element.FilledColor.ToAndroid()));
                DrawableCompat.SetTintMode(progressDrawable, PorterDuff.Mode.SrcIn);
            }
            else
            {
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(element.FilledColor.ToAndroid());
                Control.ProgressTintMode = PorterDuff.Mode.SrcIn;

                Control.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(element.EmptyColor.ToAndroid());
                Control.ProgressBackgroundTintMode = PorterDuff.Mode.Overlay;
            }

            //Control.ScaleY = (float)element.ProgressBarHeigth; //Changes the height
        }
    }
}