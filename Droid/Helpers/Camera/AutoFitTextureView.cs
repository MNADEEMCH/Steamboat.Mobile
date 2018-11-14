using Android.Content;
using Android.Util;
using Android.Views;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class AutoFitTextureView : TextureView
    {
        public AutoFitTextureView(Context context) : this(context, null)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs) :
        this(context, attrs, 0)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

        }
    }
}
