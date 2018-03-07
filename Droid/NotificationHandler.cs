using System;
using Android.Content;
using Microsoft.AppCenter.Push;

namespace Steamboat.Mobile.Droid
{
    public class NotificationHandler:PushReceiver
    {
		public override void OnReceive(Context context, Intent intent)
		{
            base.OnReceive(context, intent);

            Android.OS.Bundle extras = intent.Extras;



		}
	}
}
