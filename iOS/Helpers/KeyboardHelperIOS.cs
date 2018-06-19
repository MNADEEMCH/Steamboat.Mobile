using System;
using Foundation;
using Steamboat.Mobile.Helpers;
using UIKit;

namespace Steamboat.Mobile.iOS.Helpers
{
	// Raises keyboard changed events containing the keyboard height and
    // whether the keyboard is becoming visible or not
    public class KeyboardHelperIOS : IKeyboardHelper
    {
		public KeyboardHelperIOS()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        public event EventHandler<KeyboardHelperEventArgs> KeyboardChanged;

        private void OnKeyboardNotification(NSNotification notification)
        {
            var duration = (uint)(UIKeyboard.AnimationDurationFromNotification(notification) * 1000);
            var visible = notification.Name == UIKeyboard.WillShowNotification;
            var keyboardFrame = visible
                ? UIKeyboard.FrameEndFromNotification(notification)
                : UIKeyboard.FrameBeginFromNotification(notification);
            if (KeyboardChanged != null)
            {
                KeyboardChanged(this, new KeyboardHelperEventArgs
                {
                    Visible = visible,
                    Height = (double)keyboardFrame.Height,
                    Duration = duration
                });
            }
        }
    }
}
