﻿using System;

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

namespace Steamboat.Mobile.Droid
{
    [Activity(LaunchMode = LaunchMode.SingleTask, Label = "Momentum", Icon = "@drawable/icon", Theme = "@style/MyTheme",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static bool _newIntent = false;
        private static Context _mContext;

        public static Context getContext()
        {
            return _mContext;
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


            global::Xamarin.Forms.Forms.Init(this, bundle);

            ResolveDependencies();
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            UserDialogs.Init(this);
            LoadApplication(new App(pushNotification));



            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //        .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //        .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //        .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_newIntent)
            {
                PushNotification pushNotificationParameter = NotificationHelper.TryGetPushNotification(this.Intent);

                if (pushNotificationParameter != null)
                {
                    Task.Run(async() => await App.HandlePushNotification(pushNotificationParameter));
                }

                _newIntent = false;
            }
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
