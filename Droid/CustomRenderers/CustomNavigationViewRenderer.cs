using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomNavigationView), typeof(CustomNavigationViewRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CustomNavigationViewRenderer : NavigationPageRenderer
    {
        Android.Support.V7.Widget.Toolbar _toolbar;
        LinearLayout _titleViewLayout;
        Android.Widget.FrameLayout _parentLayout;
        Context _context;

        Drawable _originalToolbarBackground;
        Drawable _originalWindowContent;

        public CustomNavigationViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        protected override void SetupPageTransition(Android.Support.V4.App.FragmentTransaction transaction, bool isPush)
        {
            Page lastPage = null;

            if (isPush)
            {
                if (Element?.Navigation?.NavigationStack?.Count() >= 2)
                {
                    var previousPage = Element?.Navigation?.NavigationStack[Element.Navigation.NavigationStack.Count() - 2];
                    previousPage.PropertyChanged -= LastPage_PropertyChanged;
                }

                lastPage = Element?.Navigation?.NavigationStack?.Last();
                SetupToolbarCustomization(lastPage);
                lastPage.PropertyChanged += LastPage_PropertyChanged;
            }
            else if (Element?.Navigation?.NavigationStack?.Count() >= 2)
            {
                var previousPage = Element?.Navigation?.NavigationStack?.Last();
                previousPage.PropertyChanged -= LastPage_PropertyChanged;

                lastPage = Element?.Navigation?.NavigationStack[Element.Navigation.NavigationStack.Count() - 2];
                lastPage.PropertyChanged += LastPage_PropertyChanged;
                SetupToolbarCustomization(lastPage, true);

            }

            base.SetupPageTransition(transaction, isPush);
        }

        private void LastPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var lastPage = sender as Page;
            if (e.PropertyName == CustomNavigationView.HasShadowProperty.PropertyName)
            {
                UpdateToolbarShadow(_toolbar, CustomNavigationView.GetHasShadow(lastPage), Context as Activity, _originalWindowContent);
            }
            else if (e.PropertyName == CustomNavigationView.BarBackgroundProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);
            }
            else if (e.PropertyName == CustomNavigationView.GradientColorsProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);
            }
            else if (e.PropertyName == CustomNavigationView.GradientDirectionProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);

            }
        }

        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                if (_toolbar != null)
                {
                    var lastPage = Element?.Navigation?.NavigationStack?.Last();
                    try
                    {
                        _toolbar.ChildViewAdded -= OnToolbarChildViewAdded;
                    }
                    catch
                    {

                    }
                    finally
                    {
                        lastPage.PropertyChanged -= LastPage_PropertyChanged;
                    }
                }
            }
        }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                var lastPage = Element?.Navigation?.NavigationStack?.Last();

                _toolbar = (Android.Support.V7.Widget.Toolbar)child;
                _originalToolbarBackground = _toolbar.Background;

                var originalContent = (Context as Activity)?.Window?.DecorView?.FindViewById<FrameLayout>(Window.IdAndroidContent);
                if (originalContent != null)
                {
                    _originalWindowContent = originalContent.Foreground;
                }

                _parentLayout = new FrameLayout(_toolbar.Context)
                {
                    LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                };

                _titleViewLayout = new LinearLayout(_parentLayout.Context)
                {
                    Orientation = Android.Widget.Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
                };

                _toolbar.ChildViewAdded += OnToolbarChildViewAdded;
                lastPage.PropertyChanged += LastPage_PropertyChanged;
            }
        }

        void SetupToolbarCustomization(Page lastPage, bool isBack = false)
        {
            if (lastPage != null && _titleViewLayout != null)
            {
                UpdateToolbarStyle(_toolbar, lastPage, Context as Activity, _originalToolbarBackground, _originalWindowContent);
            }
        }

        #region Toolbar

        void UpdateToolbarStyle(Android.Support.V7.Widget.Toolbar toolbar, Page lastPage, Activity activity, Drawable defaultBackground, Drawable windowContent)
        {
            UpdateToolbarBackground(toolbar, lastPage, activity, defaultBackground);
            UpdateToolbarShadow(toolbar, CustomNavigationView.GetHasShadow(lastPage), activity, windowContent);
        }

        void UpdateToolbarBackground(Android.Support.V7.Widget.Toolbar toolbar, Page lastPage, Activity activity, Drawable defaultBackground)
        {

            if (string.IsNullOrEmpty(CustomNavigationView.GetBarBackground(lastPage)) && CustomNavigationView.GetGradientColors(lastPage) == null)
            {

                toolbar.SetBackground(defaultBackground);
            }
            else
            {
                if (!string.IsNullOrEmpty(CustomNavigationView.GetBarBackground(lastPage)))
                {

                    toolbar.SetBackgroundResource(this.Context.Resources.GetIdentifier(CustomNavigationView.GetBarBackground(lastPage), "drawable", Android.App.Application.Context.PackageName));
                }

                if (CustomNavigationView.GetGradientColors(lastPage) != null)
                {
                    var colors = CustomNavigationView.GetGradientColors(lastPage);
                    var direction = GradientDrawable.Orientation.TopBottom;
                    switch (CustomNavigationView.GetGradientDirection(lastPage))
                    {
                        case CustomNavigationView.GradientDirection.BottomToTop:
                            direction = GradientDrawable.Orientation.BottomTop;
                            break;
                        case CustomNavigationView.GradientDirection.RightToLeft:
                            direction = GradientDrawable.Orientation.RightLeft;
                            break;
                        case CustomNavigationView.GradientDirection.LeftToRight:
                            direction = GradientDrawable.Orientation.LeftRight;
                            break;
                    }

                    GradientDrawable gradient = new GradientDrawable(direction, new int[] { colors.Item1.ToAndroid().ToArgb(), colors.Item2.ToAndroid().ToArgb() });
                    gradient.SetCornerRadius(0f);
                    toolbar.SetBackground(gradient);

                }
            }
            toolbar.Background.SetAlpha(255);
        }

        void UpdateToolbarShadow(Android.Support.V7.Widget.Toolbar toolbar, bool hasShadow, Activity activity, Android.Graphics.Drawables.Drawable windowContent)
        {
            var androidContent = activity?.Window?.DecorView?.FindViewById<FrameLayout>(Window.IdAndroidContent);
            if (androidContent != null)
            {
                if (hasShadow && activity != null)
                {

                    GradientDrawable shadowGradient = new GradientDrawable(GradientDrawable.Orientation.RightLeft, new int[] { Android.Graphics.Color.Transparent.ToArgb(), Android.Graphics.Color.Gray.ToArgb() });
                    shadowGradient.SetCornerRadius(0f);
                    androidContent.Foreground = shadowGradient;
                    ViewCompat.SetElevation(toolbar, 4);
                }
                else
                {
                    androidContent.Foreground = windowContent;
                    ViewCompat.SetElevation(toolbar, 0);
                }
            }
        }

        #endregion

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void OnToolbarChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var view = e.Child.GetType();

            if (view == typeof(AppCompatTextView))
            {
                var textView = (AppCompatTextView)e.Child;
                textView.Visibility = ViewStates.Gone;

                var lastPage = Element?.Navigation?.NavigationStack?.Last();
                SetupToolbarCustomization(lastPage);
            }
        }
    }
}
