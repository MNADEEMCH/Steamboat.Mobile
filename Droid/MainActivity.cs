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

namespace Steamboat.Mobile.Droid
{
    [Activity(Label = "Steamboat.Mobile.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ResolveDependencies();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);
            //LoadApplication(new App());
            LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
                new UXDivers.Gorilla.Config("Good Gorilla").RegisterAssembliesFromTypes<FFImageLoading.Forms.CachedImage,FFImageLoading.Svg.Forms.SvgCachedImage>()
            ));
        }

        private void ResolveDependencies()
        {
            DependencyContainer.RegisterDependencies();
            AndroidDependencyContainer.RegisterDependencies();
        }
    }
}
