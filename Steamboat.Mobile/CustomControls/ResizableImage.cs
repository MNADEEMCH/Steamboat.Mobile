using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class ResizableImage:CachedImage
    {

        public static BindableProperty HeigthFactorProperty
        = BindableProperty.Create(nameof(ResizableImage), typeof(float), typeof(ResizableImage), 0.0f);

        public float HeigthFactor
        {
            get { return (float)GetValue(HeigthFactorProperty); }
            set { SetValue(HeigthFactorProperty, value); }
        }

        public ResizableImage(){
            this.SizeChanged += new EventHandler(OnSizeChanged);     
        }

        private void OnSizeChanged(object sender, System.EventArgs e)
        {
            this.HeightRequest = this.Width / HeigthFactor;
        }

    }
}
