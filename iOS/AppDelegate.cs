﻿using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Foundation;
using Microsoft.AppCenter.Push;
using Steamboat.Mobile.CustomControls;
using UIKit;
using UXDivers.Gorilla;

namespace Steamboat.Mobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            ResolveDependencies();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);
            Push.PushNotificationReceived += (sender, e) => {

                // Add the notification message and title to the message
                var summary = $"Push notification received:" +
                                    $"\n\tNotification title: {e.Title}" +
                                    $"\n\tMessage: {e.Message}";

                // If there is custom data associated with the notification,
                // print the entries
                if (e.CustomData != null)
                {
                    summary += "\n\tCustom data:\n";
                    foreach (var key in e.CustomData.Keys)
                    {
                        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    }
                }

                // Send the notification summary to debug output
                System.Diagnostics.Debug.WriteLine(summary);
            };
            LoadApplication(new App());
            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //  new UXDivers.Gorilla.Config("Good Gorilla")
            //    .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //    .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //    .RegisterAssembly(typeof(GradientRoundedButton).Assembly)
            //));

            return base.FinishedLaunching(app, options);
        }

        private void ResolveDependencies()
        {
            IOSDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }
    }
}
