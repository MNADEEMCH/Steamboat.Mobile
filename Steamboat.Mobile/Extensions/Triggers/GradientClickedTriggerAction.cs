using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.Extensions
{
    public class GradientClickedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button sender)
        {
            sender.BackgroundColor = Color.Red;
        }
    }
}
