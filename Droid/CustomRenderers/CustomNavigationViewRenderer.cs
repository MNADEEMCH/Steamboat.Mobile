using System;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
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
        AppCompatImageView _titleImageView;
        Android.Widget.FrameLayout _parentLayout;
        Context _context;

        Drawable _originalDrawable;
        Drawable _originalToolbarBackground;
        Drawable _originalWindowContent;
        ColorStateList _originalColorStateList;
        Typeface _originalFont;

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
            else if (e.PropertyName == CustomNavigationView.ImageSourceProperty.PropertyName)
            {
                UpdateImageSource(_titleImageView, CustomNavigationView.GetImageSource(lastPage));
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
            else if (e.PropertyName == CustomNavigationView.ImagePositionProperty.PropertyName)
            {
                UpdateTitleViewLayoutAlignment(_titleViewLayout, _titleImageView, CustomNavigationView.GetImagePosition(lastPage));
            }
            else if (e.PropertyName == CustomNavigationView.ImageMarginProperty.PropertyName)
            {
                UpdateTitleViewLayoutMargin(lastPage, _titleViewLayout, CustomNavigationView.GetImageMargin(lastPage), false);
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

                _titleImageView = new AppCompatImageView(_parentLayout.Context)
                {
                    LayoutParameters = new ActionBar.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                };

                _titleViewLayout.AddView(_titleImageView);
                _parentLayout.AddView(_titleViewLayout);
                _toolbar.AddView(_parentLayout);

                _toolbar.ChildViewAdded += OnToolbarChildViewAdded;
                lastPage.PropertyChanged += LastPage_PropertyChanged;
            }
        }

        void SetupToolbarCustomization(Page lastPage, bool isBack = false)
        {
            if (lastPage != null && _titleViewLayout != null)
            {
                UpdateImageSource(_titleImageView, CustomNavigationView.GetImageSource(lastPage));

                UpdateToolbarStyle(_toolbar, lastPage, Context as Activity, _originalToolbarBackground, _originalWindowContent);

                UpdateTitleViewLayout(lastPage, _titleViewLayout, _titleImageView, isBack);
            }
        }

        #region Title View Layout

        void UpdateTitleViewLayout(Page lastPage, LinearLayout titleViewLayout, AppCompatImageView titleImageView, bool isBack)
        {
            UpdateTitleViewLayoutAlignment(titleViewLayout, titleImageView, CustomNavigationView.GetImagePosition(lastPage));
            UpdateTitleViewLayoutMargin(lastPage, titleViewLayout, CustomNavigationView.GetImageMargin(lastPage), isBack);
        }

        void UpdateTitleViewLayoutAlignment(LinearLayout titleViewLayout, AppCompatImageView titleImageView, CustomNavigationView.ImageAlignment alignment)
        {
            var titleViewParams = titleViewLayout.LayoutParameters as FrameLayout.LayoutParams;
            var titleImageViewParams = titleImageView.LayoutParameters as LinearLayout.LayoutParams;

            switch (alignment)
            {
                case CustomNavigationView.ImageAlignment.Start:
                    titleViewParams.Gravity = GravityFlags.Start | GravityFlags.CenterVertical;
                    titleImageViewParams.Gravity = GravityFlags.Start;
                    break;
                case CustomNavigationView.ImageAlignment.Center:
                    titleViewParams.Gravity = GravityFlags.Center;
                    titleImageViewParams.Gravity = GravityFlags.Center;
                    break;
                case CustomNavigationView.ImageAlignment.End:
                    titleViewParams.Gravity = GravityFlags.End | GravityFlags.CenterVertical;
                    titleImageViewParams.Gravity = GravityFlags.End;
                    break;
            }
            titleViewLayout.LayoutParameters = titleViewParams;
            titleImageView.LayoutParameters = titleImageViewParams;
        }

        void UpdateTitleViewLayoutBackground(LinearLayout titleViewLayout, string backgroundResource, Drawable defaultBackground)
        {

            if (!string.IsNullOrEmpty(backgroundResource))
            {
                titleViewLayout?.SetBackgroundResource(this.Context.Resources.GetIdentifier(backgroundResource, "drawable", Android.App.Application.Context.PackageName));
            }
            else
            {
                titleViewLayout?.SetBackground(defaultBackground);
            }
        }

        void UpdateTitleViewLayoutMargin(Page lastPage, LinearLayout titleViewLayout, Thickness margin, bool isBack)
        {
            var titleViewParams = titleViewLayout.LayoutParameters as FrameLayout.LayoutParams;

            var hasToolbarItem = lastPage.ToolbarItems.Count() > 0;
            var singlePage = lastPage.Navigation.NavigationStack.Count() < 2;

            if (hasToolbarItem && !singlePage && !isBack)
            {
                titleViewParams?.SetMargins(0, 0, 32, 0);
            }
            else if (hasToolbarItem)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    titleViewParams?.SetMargins(25, 0, 0, 0);
                });
            }
            else if (!singlePage)
            {
                titleViewParams?.SetMargins(0, 0, 70, 0);
            }
            else
            {
                titleViewParams?.SetMargins(0, 0, 15, 0);
            }

            titleViewLayout.LayoutParameters = titleViewParams;
        }

        #endregion

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
                    toolbar.Elevation = 4;
                }
                else
                {
                    androidContent.Foreground = windowContent;
                    toolbar.Elevation = 0;
                }
            }
        }

        #endregion

        #region General ImageView

        private void UpdateImageSource(AppCompatImageView imageView, string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                var imageName = System.IO.Path.GetFileNameWithoutExtension(source).ToLower();
                int resID = Resources.GetIdentifier(imageName, "drawable", this.Context.PackageName);
                var drawable = _context.GetDrawable(resID);
                imageView.SetBackground(drawable);
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
