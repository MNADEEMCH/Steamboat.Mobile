using System;
using Android.Content;
using Android.Util;

namespace Steamboat.Mobile.Droid.Helpers
{
    public class DisplayMetricsHelper
    {
        public static int DpToPx(Context context, float dp){
            var pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
            return pixel;
        }
    }
}
