using System;
using Android.Content;
using Android.Gms.Common;

namespace Steamboat.Mobile.Droid.Helpers
{
    public class GooglePlayServicesHelper
    {
        public static bool IsPlayServicesAvailable(Context context){
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            return resultCode == ConnectionResult.Success;
        }
    }
}
