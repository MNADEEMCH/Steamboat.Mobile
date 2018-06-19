using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.ViewModels.Interfaces;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
	public partial class MessagingView : CustomContentPage
	{
		private double _lastScroll;

		public MessagingView()
		{
			InitializeComponent();

			var deviceInfo = DependencyContainer.Resolve<IDeviceInfo>();
            if (!deviceInfo.IsAndroid)
            {
                if (deviceInfo.Model.ToLower().Contains("iphone x"))
                {
					EntryGrid.Padding = new Thickness(EntryGrid.Padding.Left, EntryGrid.Padding.Top, EntryGrid.Padding.Right, EntryGrid.Padding.Bottom + 12);
					var actualHeight = MainGrid.RowDefinitions[2].Height.Value;
					MainGrid.RowDefinitions[2].Height = actualHeight + 12;
                }
            }

			KeyboardHelper.KeyboardChanged += (sender, e) => {
				var bottomOffset = MainGrid.Bounds.Bottom - EntryGrid.Bounds.Bottom;   // This offset allows us to only raise the stack by the amount required to stay above the keyboard. 
                var ty = e.Visible ? -e.Height : -1;
				MainGrid.TranslateTo(MainGrid.TranslationX, ty, e.Duration, Easing.CubicInOut);               
                //              spacer.HeightRequest = e.Visible ? e.Height : -1;
            };
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (BindingContext is IHandleViewAppearing viewAware)
            {
                await viewAware.OnViewAppearingAsync(this);
            }
		}

		protected override async void OnDisappearing()
		{
			base.OnDisappearing();

			if (BindingContext is IHandleViewDisappearing viewAware)
            {
                await viewAware.OnViewDisappearingAsync(this);
            }
		}
	}
}
