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

using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using System.Collections.Generic;
using System.Linq;
using ME.Leolin.Shortcutbadger;
using Steamboat.Mobile.Models.NavigationParameters;

namespace Steamboat.Mobile.Droid
{
    [Activity(LaunchMode = LaunchMode.SingleTask, Label = "Momentum", Icon = "@drawable/icon", Theme = "@style/MyTheme",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {   
        private static bool newIntent=false;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            IsPlayServicesAvailable();

            PushNotificationParameter pushNotificationParameter = createPushNotificationParameter();

            if (pushNotificationParameter != null)
            {
                //TO MODIFY BADGE FROM APP
                //ShortcutBadger.ApplyCount(this.ApplicationContext, 0);
            }

            if (FirebaseInstanceId.Instance.Token != null)
            {
                Log.Debug("Token", FirebaseInstanceId.Instance.Token);
                FirebaseMessaging.Instance.SubscribeToTopic("news");
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ResolveDependencies();
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            UserDialogs.Init(this);
            LoadApplication(new App(pushNotificationParameter));
            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //        .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //        .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //        .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));
        }

		protected async override void OnResume()
		{
            base.OnResume();

            PushNotificationParameter pushNotificationParameter = createPushNotificationParameter();

            if (pushNotificationParameter != null)
            {
                //TO MODIFY BADGE FROM APP
                ShortcutBadger.ApplyCount(this.ApplicationContext, 0);
                await App.HandlePushNotification(pushNotificationParameter);
            }
		}

        private PushNotificationParameter createPushNotificationParameter(){

            PushNotificationParameter pushNotificationParameter = null;
            if (newIntent && this.Intent != null && this.Intent.Extras != null)
            {

                Bundle extras = this.Intent.Extras;
                Dictionary<string, object> data = extras.KeySet()
                                                        .ToDictionary<string, string, object>(key => key, key => extras.Get(key));

                pushNotificationParameter = new PushNotificationParameter() { PruebaPush = string.Join(",", data.Keys.ToList()) };

                newIntent = false;

            }

            return pushNotificationParameter;
        }

		public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            return resultCode == ConnectionResult.Success;
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
            newIntent = true;
		}

	}
}
