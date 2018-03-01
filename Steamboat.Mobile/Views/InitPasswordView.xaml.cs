using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Steamboat.Mobile.Views
{
    public partial class InitPasswordView : CustomContentPage
    {
        public InitPasswordView()
        {
            InitializeComponent();

            PasswordEntry.Completed += passwordEntry_Completed;
            PasswordConfirmEntry.Completed += passwordConfirmEntry_Completed;
        }

        private void passwordEntry_Completed(object sender, EventArgs e)
        {
            PasswordConfirmEntry.Focus();
        }
        private void passwordConfirmEntry_Completed(object sender, EventArgs e)
        {
            UpdateButton.Command.Execute(null);
        }
    }
}
