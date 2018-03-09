using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using UXDivers.Gorilla;
using FFImageLoading.Forms.Droid;
using FFImageLoading.Svg.Forms;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.CustomControls;
using Acr.UserDialogs;

using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace Steamboat.Mobile.Droid
{
    [Activity(Label = "Momentum", Icon = "@drawable/icon", Theme = "@style/MyTheme",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            IsPlayServicesAvailable();
            if(this.Intent!=null && this.Intent.Extras!=null){

            }
            if(FirebaseInstanceId.Instance.Token!=null)
                Log.Debug("Token", FirebaseInstanceId.Instance.Token);
            FirebaseMessaging.Instance.SubscribeToTopic("news");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ResolveDependencies();
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            UserDialogs.Init(this);
            LoadApplication(new App());
            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //        .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //        .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //        .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                /*if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    msgText.Text = "This device is not supported";
                    Finish();
                }*/
                return false;
            }
            else
            {
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }

        private void ResolveDependencies()
        {
            AndroidDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }
    }
}
