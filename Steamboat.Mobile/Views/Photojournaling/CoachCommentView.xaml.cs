using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Steamboat.Mobile.Views.Photojournaling
{
    public partial class CoachCommentView : ContentView
    {
        #region Properties

        public static readonly BindableProperty ShowCommentProperty =
            BindableProperty.Create(nameof(ShowComment), typeof(bool), typeof(CoachCommentView), default(bool));

        public static readonly BindableProperty IsAcknowledgedProperty =
            BindableProperty.Create(nameof(IsAcknowledged), typeof(bool), typeof(CoachCommentView), default(bool));

        public bool ShowComment
        {
            get { return (bool)GetValue(ShowCommentProperty); }
            set { SetValue(ShowCommentProperty, value); }
        }

        public bool IsAcknowledged
        {
            get { return (bool)GetValue(IsAcknowledgedProperty); }
            set { SetValue(IsAcknowledgedProperty, value); }
        }

        #endregion

        public CoachCommentView()
        {
            InitializeComponent();
        }
    }
}
