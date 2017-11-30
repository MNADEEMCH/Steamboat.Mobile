using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using Foundation;
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
            LoadApplication(new App());
            //LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
            //  new UXDivers.Gorilla.Config("Good Gorilla")
            //    .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //));

            return base.FinishedLaunching(app, options);
        }

        private void ResolveDependencies()
        {
            DependencyContainer.RegisterDependencies();
            IOSDependencyContainer.RegisterDependencies();
        }
    }
}
