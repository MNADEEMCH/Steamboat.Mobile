using System;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using CoreAnimation;
using Steamboat.Mobile.Views;
using Foundation;
using Steamboat.Mobile.iOS.Helpers;

[assembly: ExportRenderer(typeof(CustomContentPage), typeof(CustomNavigationViewRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CustomNavigationViewRenderer : PageRenderer
    {
        private bool _firstLoad = true;


        //protected override void OnElementChanged(VisualElementChangedEventArgs e)
        //{
        //    base.OnElementChanged(e);
        //}

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    Element.PropertyChanged += Element_PropertyChanged;
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    if (Element is CustomContentPage page && _firstLoad && NavigationController != null)
        //    {
        //        SetupNavBar(NavigationController.NavigationBar.Bounds.Size);
        //        _firstLoad = false;
        //    }
        //}

        //public override void ViewWillDisappear(bool animated)
        //{
        //    base.ViewWillDisappear(animated);
        //}

        //void SetupNavBar(CGSize size)
        //{
        //    if (NavigationController != null && Element is CustomContentPage page)
        //    {
        //        SetupBackground();
        //    }
        //}

        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);

        //    var element = this.Element as CustomContentPage;

        //    UIApplication.SharedApplication.SetStatusBarHidden(!element.ShowStatusBar, false);
        //}

        //public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        //{
        //    base.ViewWillTransitionToSize(toSize, coordinator);
        //    SetupNavBar(new CGSize(NavigationController?.NavigationBar?.Bounds.Size.Width ?? 0.0f, NavigationController?.NavigationBar?.Bounds.Height ?? 0.0f));
        //}

        //UIImage CreateGradientBackground(Color startColor, Color endColor, CustomNavigationView.GradientDirection direction)
        //{
        //    var gradientLayer = new CAGradientLayer();
        //    gradientLayer.Bounds = NavigationController.NavigationBar.Bounds;
        //    gradientLayer.Colors = new CGColor[] { startColor.ToCGColor(), endColor.ToCGColor() };

        //    switch (direction)
        //    {
        //        case CustomNavigationView.GradientDirection.LeftToRight:
        //            gradientLayer.StartPoint = new CGPoint(0.0, 0.5);
        //            gradientLayer.EndPoint = new CGPoint(1.0, 0.5);
        //            break;
        //        case CustomNavigationView.GradientDirection.RightToLeft:
        //            gradientLayer.StartPoint = new CGPoint(1.0, 0.5);
        //            gradientLayer.EndPoint = new CGPoint(0.0, 0.5);
        //            break;
        //        case CustomNavigationView.GradientDirection.BottomToTop:
        //            gradientLayer.StartPoint = new CGPoint(1.0, 1.0);
        //            gradientLayer.EndPoint = new CGPoint(0.0, 0.0);
        //            break;
        //        default:
        //            gradientLayer.StartPoint = new CGPoint(1.0, 0.0);
        //            gradientLayer.EndPoint = new CGPoint(0.0, 1.0);
        //            break;
        //    }

        //    UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
        //    gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
        //    UIImage image = UIGraphics.GetImageFromCurrentImageContext();
        //    UIGraphics.EndImageContext();

        //    return image;
        //}

        //public override void ViewDidDisappear(bool animated)
        //{
        //    base.ViewDidDisappear(animated);
        //}

        //void SetupShadow(bool hasShadow)
        //{
        //    if (hasShadow)
        //    {
        //        NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Gray.CGColor;
        //        NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
        //        NavigationController.NavigationBar.Layer.ShadowOpacity = 1;
        //    }
        //    else
        //    {
        //        NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Clear.CGColor;
        //        NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
        //        NavigationController.NavigationBar.Layer.ShadowOpacity = 0;
        //    }
        //}

        //void SetupBackground(UIImage image, float alpha)
        //{
        //    NavigationController.NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
        //    NavigationController.NavigationBar.Alpha = alpha;
        //}

        //void SetupBackground()
        //{
        //    if (!string.IsNullOrEmpty(CustomNavigationView.GetBarBackground(Element)))
        //    {
        //        SetupBackground(UIImage.FromBundle(CustomNavigationView.GetBarBackground(Element)), 1);
        //    }
        //    else if (CustomNavigationView.GetGradientColors(Element) != null)
        //    {
        //        SetupBackground(CreateGradientBackground(CustomNavigationView.GetGradientColors(Element).Item1, CustomNavigationView.GetGradientColors(Element).Item2, CustomNavigationView.GetGradientDirection(Element)), 1);
        //    }
        //}

        //private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    var page = sender as Page;

        //    if (e.PropertyName == CustomNavigationView.GradientColorsProperty.PropertyName || e.PropertyName == CustomNavigationView.GradientDirectionProperty.PropertyName || e.PropertyName == CustomNavigationView.BarBackgroundProperty.PropertyName)
        //    {
        //        SetupBackground();
        //    }
        //    else if (e.PropertyName == CustomNavigationView.HasShadowProperty.PropertyName)
        //    {
        //        SetupShadow(CustomNavigationView.GetHasShadow(page));
        //    }
        //}

        //public override void ViewDidUnload()
        //{
        //    Element.PropertyChanged -= Element_PropertyChanged;

        //    base.ViewDidUnload();
        //}

        private UIImageView _imageView;
        private UIView _containerView;
        //private bool _firstLoad = true;
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        private bool _pageWasShiftedUp;
        private double _activeViewBottom;
        private bool _isKeyboardShown;
        private int _entryPadding => 12;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _containerView = new UIView();
            _imageView = new UIImageView();

            Element.PropertyChanged += Element_PropertyChanged;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var page = Element as CustomContentPage;

            if (page != null)
            {
                //if (page.iOSKeyboardScroll)
                //{
                //  var contentScrollView = page.Content as ScrollView;

                //  if (contentScrollView != null)
                //      return;

                //  RegisterForKeyboardNotifications();
                //}

                if (_firstLoad)
                {
                    SetupNavBar(NavigationController.NavigationBar.Bounds.Size);
                    SetImageSource("icLogo.png");
                    SetImagePosition(new CGRect(0, 0, _imageView.IntrinsicContentSize.Width, _imageView.IntrinsicContentSize.Height));
                    _firstLoad = false;
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        void SetupNavBar(CGSize size)
        {
            if (NavigationController != null && _imageView != null)
            {
                var page = Element as CustomContentPage;
                _containerView.Frame = new CGRect(0, 0, size.Width, size.Height);

                if (page != null)
                {
                    SetupBackground();

                    ParentViewController.NavigationItem.TitleView = _imageView;
                    ParentViewController.NavigationItem.TitleView.SetNeedsDisplay();
                }
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            var element = this.Element as CustomContentPage;

            UIApplication.SharedApplication.SetStatusBarHidden(!element.ShowStatusBar, false);
        }

        void SetImagePosition(CGRect vFrame)
        {
            _imageView.ContentMode = UIViewContentMode.Top;
            _imageView.Frame = new CGRect(_imageView.Frame.X, _imageView.Frame.Y, _imageView.Bounds.Width + 0, _imageView.Bounds.Height + 0);
            _imageView.Center = new CGPoint(_containerView.Center.X - 8, _containerView.Center.Y);
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);
            SetupNavBar(new CGSize(NavigationController?.NavigationBar?.Bounds.Size.Width ?? 0.0f, NavigationController?.NavigationBar?.Bounds.Height ?? 0.0f));
        }

        UIImage CreateGradientBackground(Color startColor, Color endColor, CustomNavigationView.GradientDirection direction)
        {
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Bounds = NavigationController.NavigationBar.Bounds;
            gradientLayer.Colors = new CGColor[] { startColor.ToCGColor(), endColor.ToCGColor() };

            switch (direction)
            {
                case CustomNavigationView.GradientDirection.LeftToRight:
                    gradientLayer.StartPoint = new CGPoint(0.0, 0.5);
                    gradientLayer.EndPoint = new CGPoint(1.0, 0.5);
                    break;
                case CustomNavigationView.GradientDirection.RightToLeft:
                    gradientLayer.StartPoint = new CGPoint(1.0, 0.5);
                    gradientLayer.EndPoint = new CGPoint(0.0, 0.5);
                    break;
                case CustomNavigationView.GradientDirection.BottomToTop:
                    gradientLayer.StartPoint = new CGPoint(1.0, 1.0);
                    gradientLayer.EndPoint = new CGPoint(0.0, 0.0);
                    break;
                default:
                    gradientLayer.StartPoint = new CGPoint(1.0, 0.0);
                    gradientLayer.EndPoint = new CGPoint(0.0, 1.0);
                    break;
            }

            UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        void SetupShadow(bool hasShadow)
        {
            if (hasShadow)
            {
                NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Gray.CGColor;
                NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
                NavigationController.NavigationBar.Layer.ShadowOpacity = 1;
            }
            else
            {
                NavigationController.NavigationBar.Layer.ShadowColor = UIColor.Clear.CGColor;
                NavigationController.NavigationBar.Layer.ShadowOffset = new CGSize(0, 0);
                NavigationController.NavigationBar.Layer.ShadowOpacity = 0;
            }
        }

        void SetupBackground(UIImage image, float alpha)
        {
            NavigationController.NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
            NavigationController.NavigationBar.Alpha = alpha;
        }

        void SetupBackground()
        {
            if (!string.IsNullOrEmpty(CustomNavigationView.GetBarBackground(Element)))
            {
                SetupBackground(UIImage.FromBundle(CustomNavigationView.GetBarBackground(Element)), 1);
            }
            else if (CustomNavigationView.GetGradientColors(Element) != null)
            {
                SetupBackground(CreateGradientBackground(CustomNavigationView.GetGradientColors(Element).Item1, CustomNavigationView.GetGradientColors(Element).Item2, CustomNavigationView.GetGradientDirection(Element)), 1);
            }
        }

        private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var page = sender as Page;

            //if (e.PropertyName == CustomNavigationView.ImagePositionProperty.PropertyName || e.PropertyName == CustomNavigationView.ImageMarginProperty.PropertyName)
            //{
            //    SetImagePosition(CustomNavigationView.GetImagePosition(page), CustomNavigationView.GetImageMargin(Element), new CGRect(0, 0, _imageView.IntrinsicContentSize.Width, _imageView.IntrinsicContentSize.Height));
            //}
            //else if (e.PropertyName == CustomNavigationView.ImageSourceProperty.PropertyName)
            //{
            //    SetImageSource(CustomNavigationView.GetImageSource(page));
            //}
            if (e.PropertyName == CustomNavigationView.GradientColorsProperty.PropertyName || e.PropertyName == CustomNavigationView.GradientDirectionProperty.PropertyName || e.PropertyName == CustomNavigationView.BarBackgroundProperty.PropertyName)
            {
                SetupBackground();
            }
            else if (e.PropertyName == CustomNavigationView.HasShadowProperty.PropertyName)
            {
                SetupShadow(CustomNavigationView.GetHasShadow(page));
            }
        }

        private void SetImageSource(string source)
        {
            _imageView.Image = UIImage.FromBundle(source);
        }

        public override void ViewDidUnload()
        {
            _imageView = null;
            Element.PropertyChanged -= Element_PropertyChanged;

            base.ViewDidUnload();
        }
    }
}
