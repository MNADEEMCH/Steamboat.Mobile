using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;

            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {                
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 15;
            Control.Background.SetColorFilter(element.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);

            element.Focused += (object sender, FocusEventArgs ev) => {
                Control.Background.SetColorFilter(Xamarin.Forms.Color.FromHex("#2C9FC9").ToAndroid(), PorterDuff.Mode.SrcAtop);
            };
            element.Unfocused += (object sender, FocusEventArgs ev) => {
                Control.Background.SetColorFilter(element.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            };
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("BorderColor"))
            {
                var textfield = (ImageEntry)sender;
                Control.Background.SetColorFilter(textfield.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            var imageName = System.IO.Path.GetFileNameWithoutExtension(imageEntryImage).ToLower();
            int resID = Resources.GetIdentifier(imageName, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }

    }
}
