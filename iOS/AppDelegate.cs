﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Firebase.CloudMessaging;
using Foundation;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.Helpers;
using Steamboat.Mobile.Models.Notification;
using UIKit;
using UserNotifications;
using UXDivers.Gorilla;

namespace Steamboat.Mobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate,IUNUserNotificationCenterDelegate, Firebase.CloudMessaging.IMessagingDelegate
    {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //IN ORDER TO SET BADGE 
            UIUserNotificationSettings settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            //CHECK IF THERE IS A NOTIFICATION
            PushNotification pushNotification = NotificationHelper.TryGetPushNotificationWhenIsClosed(options);

            RegisterForPushNotifications();

            ResolveDependencies();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);
            LoadApplication(new App(pushNotification));

            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //  new UXDivers.Gorilla.Config("Good Gorilla")
            //    .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //    .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //    .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));

            return base.FinishedLaunching(app, options);
        }

        private void RegisterForPushNotifications()
        {

            //In order to get the refresh token in DidRefreshRegistrationToken
            Messaging.SharedInstance.Delegate = this;

            Firebase.Core.App.Configure();

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }
       
        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)

        [Export("messaging:didRefreshRegistrationToken:")]
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            // Save new device token
            NSUserDefaults.StandardUserDefaults.SetString(fcmToken, "PushDeviceToken");
            Task.Run(async () => await App.PushTokenRefreshed());
        }

        // iOS 9 <=, fire when recieve notification foreground
        public async override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            PushNotification pushNotification = NotificationHelper.TryGetPushNotification(userInfo);
            if(pushNotification!=null)
                await App.HandlePushNotification(pushNotification);

        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public async void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {

            PushNotification pushNotification = NotificationHelper.TryGetPushNotification(notification.Request.Content.UserInfo);
            if (pushNotification != null)
                await App.HandlePushNotification(pushNotification);

        }

        private void ResolveDependencies()
        {
            IOSDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }
    }
}
