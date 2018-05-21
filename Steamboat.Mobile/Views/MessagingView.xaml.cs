using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
