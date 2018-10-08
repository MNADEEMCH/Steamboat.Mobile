using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class CustomNavigationView : NavigationPage
    {

        public enum GradientDirection
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }

        public CustomNavigationView() : base()
        {
            InitializeComponent();
        }

        public CustomNavigationView(Page root) : base(root)
        {
            InitializeComponent();
        }

        public static readonly BindableProperty BarBackgroundProperty =
            BindableProperty.CreateAttached("BarBackground", typeof(string), typeof(CustomNavigationView), string.Empty);

        public static string GetBarBackground(BindableObject view)
        {

            return (string)view.GetValue(BarBackgroundProperty);
        }

        public static void SetBarBackground(BindableObject view, string value)
        {
            view.SetValue(BarBackgroundProperty, value);
        }

        public static readonly BindableProperty GradientColorsProperty =
            BindableProperty.CreateAttached("GradientColors", typeof(Tuple<Color, Color>), typeof(CustomNavigationView), null);

        public static Tuple<Color, Color> GetGradientColors(BindableObject view)
        {

            return (Tuple<Color, Color>)view.GetValue(GradientColorsProperty);
        }

        public static void SetGradientColors(BindableObject view, Tuple<Color, Color> value)
        {
            view.SetValue(GradientColorsProperty, value);
        }

        public static readonly BindableProperty GradientDirectionProperty =
            BindableProperty.CreateAttached("GradientDirection", typeof(GradientDirection), typeof(CustomNavigationView), GradientDirection.TopToBottom);

        public static GradientDirection GetGradientDirection(BindableObject view)
        {

            return (GradientDirection)view.GetValue(GradientDirectionProperty);
        }

        public static void SetGradientDirection(BindableObject view, GradientDirection value)
        {
            view.SetValue(GradientDirectionProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.CreateAttached("HasShadow", typeof(bool), typeof(CustomNavigationView), false);

        public static bool GetHasShadow(BindableObject view)
        {

            return (bool)view.GetValue(HasShadowProperty);
        }

        public static void SetHasShadow(BindableObject view, bool value)
        {
            view.SetValue(HasShadowProperty, value);
        }

    }
}
