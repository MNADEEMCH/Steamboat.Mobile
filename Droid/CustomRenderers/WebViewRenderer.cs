using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebViewer), typeof(Steamboat.Mobile.Droid.CustomRenderers.WebViewRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class WebViewRenderer : ViewRenderer<WebViewer, Android.Webkit.WebView>
    {
        public static string MimeType = "text/html";

        public static string EncodingType = "UTF-8";

        public static string HistoryUri = "";

        public static string BaseUrl { get; set; } = "file:///android_asset/";

        public WebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebViewer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetUpControl();
            }
            if (e.OldElement != null)
            {
                var oldWebView = e.OldElement as WebViewer;
                CleanupElement(oldWebView);
            }
            if (e.NewElement != null)
            {
                var newWebView = e.NewElement as WebViewer;
                SetUpElement(newWebView);
            }

        }

        private void SetUpControl()
        {
            var webView = new Android.Webkit.WebView(Forms.Context);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            webView.Settings.MinimumFontSize = 0;
            webView.SetInitialScale(0);
            webView.Settings.LoadWithOverviewMode = true;//loads the WebView completely zoomed out

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                webView.Settings.UseWideViewPort = true;


            SetNativeControl(webView);

        }

        private static void CleanupElement(WebViewer e)
        {
            e.Cleanup();
        }

        private void SetUpElement(WebViewer e)
        {
            if (Element.Uri != null)
            {
                Control.SetWebViewClient(new AuthWebViewClient(e));
                if (e.HasCookiesOrHeaders)
                {
                    e.Uri = string.Format(
                        @" <!doctype html><html><body onload=""window.location.href = '{0}';""></body></html>",
                        e.Uri
                    );
                }

                if (WebViewRendererHelper.IsAnHttpUrl(Element.Uri))
                {
                    Control.LoadUrl(Element.Uri);
                }
                else
                {
                    Control.LoadDataWithBaseURL(BaseUrl, Element.Uri, MimeType, EncodingType, HistoryUri);
                }
            }
        }
    }

    public class AuthWebViewClient : WebViewClient
    {
        private WebViewer _webViewer;
        public AuthWebViewClient(WebViewer webViewer)
        {
            _webViewer = webViewer;
        }

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        {
            if (WebViewRendererHelper.IsAnHttpUrl(request.Url.ToString()))
            {
                AddCookies(request.Url.ToString());
                //Add headers
                view.LoadUrl(request.Url.ToString(), _webViewer.Headers);

            }
            return true;
        }

        private void AddCookies(String url)
        {
            if (_webViewer.Cookies != null)
            {

                var cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                cookieManager.RemoveAllCookie();
                CookieCollection cookies = _webViewer.Cookies.GetCookies(new Uri(url));
                IEnumerator cookiesEnumerator = cookies.GetEnumerator();
                while (cookiesEnumerator.MoveNext())
                {
                    var cookie = cookiesEnumerator.Current as Cookie;
                    cookieManager.SetCookie(cookie.Domain, cookie.Name + "=" + cookie.Value);
                }

            }

        }

        public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);

            LoadCompletedWithErrors();
        }

        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);

            LoadCompletedSucessfully(url);
        }

        private void LoadCompletedSucessfully(string url)
        {

            bool fromFile = _webViewer.FromFile;
            bool httpRequest = WebViewRendererHelper.IsAnHttpUrl(url);

            bool executeCommand = fromFile || (!fromFile && httpRequest);

            if (executeCommand)
                ExecuteLoadFinishCommandIfIsNotNull(true);
        }

        private void LoadCompletedWithErrors()
        {
            ExecuteLoadFinishCommandIfIsNotNull(false);
        }

        private void ExecuteLoadFinishCommandIfIsNotNull(bool loadedSucessfully)
        {
            if (_webViewer.LoadFinishedCommand != null)
                _webViewer.LoadFinishedCommand.Execute(loadedSucessfully);
        }

    }
}
