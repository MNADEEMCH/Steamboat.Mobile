using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Firebase.CloudMessaging;
using Foundation;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.Helpers;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Services.RequestProvider;
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

            DisableDefaultLineBreakStrategy();

            EnableToModifyIconBadgeFromTheApp();

            //CHECK IF THERE IS A NOTIFICATION
            PushNotification pushNotification = NotificationHelper.TryGetPushNotificationWhenIsClosed(options);

            RegisterForPushNotifications();

            ResolveDependencies();
            CachedImageRenderer.Init();
            ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpClient = new HttpClient(new AuthenticatedHttpClient())
            });
            //var ignore = typeof(SvgCachedImage);
            LoadApplication(new App(pushNotification));

            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //  new UXDivers.Gorilla.Config("Good Gorilla")
            //    .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //    .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //    .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));

            return base.FinishedLaunching(app, options);
        }

        private void DisableDefaultLineBreakStrategy(){
            //iOS 11: Label with WordWrap, doesnt allow orphans. To prevent orphans it
            //takes the last two words to the bottom. We disable that behavior because it looks weird.
            NSUserDefaults.StandardUserDefaults.SetString("No", "NSAllowsDefaultLineBreakStrategy");
        }

        private void EnableToModifyIconBadgeFromTheApp(){
            UIUserNotificationSettings settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
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
        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public async override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            PushNotification pushNotification = NotificationHelper.TryGetPushNotification(userInfo);

            if(pushNotification!=null){
                await App.HandlePushNotification(IsApplicationInactive(),IsApplicationBackgrounded(),pushNotification);
                if(pushNotification.IsContentAvailablePresent)
                    completionHandler(UIBackgroundFetchResult.NewData);
            }
            
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public async void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {

            PushNotification pushNotification = NotificationHelper.TryGetPushNotification(notification.Request.Content.UserInfo);
            if (pushNotification != null)
                await App.HandlePushNotification(IsApplicationInactive(),IsApplicationBackgrounded(),pushNotification);

        }


        private bool IsApplicationBackgrounded(){
            return GetAppState().Equals(UIApplicationState.Background); 
        }

        private bool IsApplicationInactive()
        {
            return GetAppState().Equals(UIApplicationState.Inactive);
        }

        private UIApplicationState GetAppState(){
            return UIApplication.SharedApplication.ApplicationState;
        }

        private void ResolveDependencies()
        {
            IOSDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }
    
    }
}
