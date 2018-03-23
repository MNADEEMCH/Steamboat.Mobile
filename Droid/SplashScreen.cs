using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Steamboat.Mobile.Droid
{
    [Activity(Label = "Momentum", Icon = "@drawable/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory=true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {

            await Task.Delay(1000); // Simulate a bit of startup work.
            Intent intent = new Intent(this, typeof(MainActivity));
            if (this.Intent != null && this.Intent.Extras != null)
                intent.PutExtras(this.Intent.Extras);

            StartActivity(intent);
        }

        public override void OnBackPressed() { }
    }
}