using System;
using System.Threading.Tasks;
using Android.App;
using Android.Util;
using Firebase.Iid;

namespace Steamboat.Mobile.Droid.Services.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            //FirebaseInstanceId.Instance.Token;
            Task.Run(async () => await App.PushTokenRefreshed());
        }
    }
}