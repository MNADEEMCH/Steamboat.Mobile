using System;
using Java.Lang;
using Size = Android.Util.Size;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class CompareSizesByArea : Java.Lang.Object, Java.Util.IComparator
    {
        public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
        {
            // We cast here to ensure the multiplications won't overflow
            if (lhs is Size && rhs is Size)
            {
                var right = (Size)rhs;
                var left = (Size)lhs;
                return Long.Signum((long)left.Width * left.Height -
                    (long)right.Width * right.Height);
            }
            else
                return 0;

        }
    }
}
