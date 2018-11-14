using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Graphics.Drawable;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(IconMasterDetailPage), typeof(IconMasterDetailPageRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class IconMasterDetailPageRenderer : MasterDetailPageRenderer
    {
        private static Context _context;
        private static int _currentMenuIconId = -1;
        private static Android.Support.V7.Widget.Toolbar _toolbar;
        private static ImageButton _menuImageButton;
        private bool _disposed;

        public IconMasterDetailPageRenderer(Context context) : base(context)
        {
            _context = context;
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (_currentMenuIconId != -1)
            {
                ChangeMenuIcon();
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
        }

        public static void ChangeMenuIcon()
        {
            if (_toolbar != null)
            {
                var app = Xamarin.Forms.Application.Current;
                if (app != null)
                {
                    var masterDetailPage = (app.MainPage as MasterDetailPage);
                    if (masterDetailPage != null)
                    {
                        var detailPage = masterDetailPage.Detail;
                        var displayBack = detailPage.Navigation.NavigationStack.Count > 1;
                        SetMenuButton();
                        if (!displayBack)
                        {
                            _menuImageButton.SetImageDrawable(ContextCompat.GetDrawable(_context, _currentMenuIconId));
                            if (_toolbar != null)
                                _toolbar.RefreshDrawableState();
                        }
                    }
                }
            }
        }

        private static void SetMenuButton()
        {
           for (var i = 0; i < _toolbar.ChildCount; i++)
            {
                var imageButton = _toolbar.GetChildAt(i) as ImageButton;

                var drawerArrow = imageButton?.Drawable as DrawerArrowDrawable;
                if (drawerArrow == null)
                    continue;

                _menuImageButton = imageButton;
            }
        }

        public static void ChangeMenuIcon(int menuIconId)
        {
            _currentMenuIconId = menuIconId;
            ChangeMenuIcon();
        }

    }
}
