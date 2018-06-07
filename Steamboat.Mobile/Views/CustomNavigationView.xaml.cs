using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class CustomNavigationView : NavigationPage
    {
        public enum ImageAlignment
        {
            Start,
            Center,
            End
        }

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

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.CreateAttached("ImageSource", typeof(string), typeof(CustomNavigationView), default(string));

        public static string GetImageSource(BindableObject view)
        {
            return (string)view.GetValue(ImageSourceProperty);
        }

        public static void SetImageSource(BindableObject view, string value)
        {
            view.SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty ImagePositionProperty = 
            BindableProperty.CreateAttached("ImagePosition", typeof(ImageAlignment), typeof(CustomNavigationView), ImageAlignment.Center);

        public static ImageAlignment GetImagePosition(BindableObject view)
        {

            return (ImageAlignment)view.GetValue(ImagePositionProperty);
        }

        public static void SetImagePosition(BindableObject view, ImageAlignment value)
        {
            view.SetValue(ImagePositionProperty, value);
        }

        public static readonly BindableProperty ImageMarginProperty = 
            BindableProperty.CreateAttached("ImageMargin", typeof(Thickness), typeof(CustomNavigationView), default(Thickness));

        public static Thickness GetImageMargin(BindableObject view)
        {

            return (Thickness)view.GetValue(ImageMarginProperty);
        }

        public static void SetImageMargin(BindableObject view, Thickness value)
        {
            view.SetValue(ImageMarginProperty, value);
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

        public class TitleAlignment
        {
        }
    }
}
