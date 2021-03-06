using System;
namespace Steamboat.Mobile.Helpers
{
	// Provides static access to keyboard events
    public static class KeyboardHelper
    {
        private static IKeyboardHelper keyboardHelper = null; 

        public static void Init()
        {
            if (keyboardHelper == null)
            {
				keyboardHelper = DependencyContainer.Resolve<IKeyboardHelper>();
            }
        }

        public static event EventHandler<KeyboardHelperEventArgs> KeyboardChanged
        {
            add
            {
                Init();
                keyboardHelper.KeyboardChanged += value;
            }
            remove
            {
                Init();
                keyboardHelper.KeyboardChanged -= value;
            }
        }
    }

    public interface IKeyboardHelper
    {
        event EventHandler<KeyboardHelperEventArgs> KeyboardChanged;
    }

    public class KeyboardHelperEventArgs : EventArgs
    {
        public bool Visible { get; set; }
        public double Height { get; set; }
        public uint Duration { get; set; }
    }
}
