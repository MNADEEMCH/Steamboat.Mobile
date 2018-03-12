using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Firebase.CloudMessaging;
using Foundation;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Models.NavigationParameters;
using UIKit;
using UserNotifications;
using UXDivers.Gorilla;

namespace Steamboat.Mobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, Firebase.CloudMessaging.IMessagingDelegate
    {

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        }


        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //IN ORDER TO SET BADGE
            UIUserNotificationSettings settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            //TO MODIFY BADGE FROM APP
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;
            PushNotificationParameter pushNotificationParameter = null;
            //TO READ THE PUSH NOT WHEN THE APP WAS CLOSED
            if (options != null && options.Keys != null && options.Keys.Count() != 0 && options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
            {
                NSDictionary UIApplicationLaunchOptionsRemoteNotificationKey = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;
                NSError error;
                var json = NSJsonSerialization.Serialize(UIApplicationLaunchOptionsRemoteNotificationKey, NSJsonWritingOptions.SortedKeys, out error);
                pushNotificationParameter = new PushNotificationParameter(){PruebaPush=json.ToString()};
            }

            //SEE WHAT IS THIS FOR
            Messaging.SharedInstance.Delegate = this;

            RegisterForPushNotifications();

            ResolveDependencies();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            LoadApplication(new App(pushNotificationParameter));
            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //  new UXDivers.Gorilla.Config("Good Gorilla")
            //    .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //    .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //    .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));

            return base.FinishedLaunching(app, options);
        }

        public void RegisterForPushNotifications(){
            
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

        public override void RegisteredForRemoteNotifications(
                                UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }


        // iOS 9 <=, fire when recieve notification foreground
        public async override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event

            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var body = alert_d["body"] as NSString;
                var title = alert_d["title"] as NSString;
            }

            var pushNotificationParameter = new PushNotificationParameter() { PruebaPush="recive en background/foreground" };
            await App.HandlePushNotification(pushNotificationParameter);
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public async void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            System.Console.WriteLine(notification.Request.Content.UserInfo);
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;

            var pushNotificationParameter = new PushNotificationParameter() { PruebaPush = "recive en background/foreground" };
            await App.HandlePushNotification(pushNotificationParameter);
        }

        private void ResolveDependencies()
        {
            IOSDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }
    }
}
