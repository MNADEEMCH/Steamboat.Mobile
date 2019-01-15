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
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Droid.Helpers;
using System.Threading.Tasks;
using FFImageLoading;
using System.Net.Http;
using Steamboat.Mobile.Services.RequestProvider;
using Lottie.Forms.Droid;
using Plugin.Permissions;
using Steamboat.Mobile.Services.Orientation;

namespace Steamboat.Mobile.Droid
{
	[Activity(LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        private static bool _newIntent = false;
        private static bool _isAppBackgrounded = false;
        private static Context _mContext;
        private IDeviceOrientationService _deviceOrientationListener;

        public static bool IsAppBackgrounded
        {
            get { return _isAppBackgrounded; }
        }

        public static Context Context
        {
            get { return _mContext; }
        }


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            //IN ORDER TO SET THE BADGE USING THE CONTEXT
            _mContext = this.ApplicationContext;
            //CHECK IF THERE IS A NOTIFICATION
            PushNotification pushNotification = NotificationHelper.TryGetPushNotification(this.Intent);
            _newIntent = false;
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ResolveDependencies();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpClient = new HttpClient(new AuthenticatedHttpClient())
            });
            AnimationViewRenderer.Init();
            _deviceOrientationListener = AndroidDependencyContainer.Resolve<IDeviceOrientationService>();

            //var ignore = typeof(SvgCachedImage);
            UserDialogs.Init(this);
            LoadApplication(new App(pushNotification));

            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //        .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //        .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //        .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));
        }

		protected override void OnPause()
		{
            base.OnPause();
            _isAppBackgrounded = true;
            _deviceOrientationListener.UnregisterListener();
        }

		protected override void OnResume()
        {
            base.OnResume();
            _isAppBackgrounded = false;
            if (_newIntent)
            {
                PushNotification pushNotificationParameter = NotificationHelper.TryGetPushNotification(this.Intent);

                if (pushNotificationParameter != null)
                {
                    Task.Run(async () => await App.HandlePushNotification(true,IsAppBackgrounded,pushNotificationParameter));
                }

                _newIntent = false;
            }
            _deviceOrientationListener.RegisterListener();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ResolveDependencies()
        {
            AndroidDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            this.Intent = intent;
            _newIntent = true;

        }
    }
}
