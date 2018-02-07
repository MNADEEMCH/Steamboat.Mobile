using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Foundation;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using WebKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(WebViewer), typeof(WebViewerRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class WebViewerRenderer : ViewRenderer<WebViewer, UIKit.UIWebView>
    {
        WKUserContentController userController;

        public static string BaseUrl { get; set; } = NSBundle.MainBundle.BundlePath;

        protected override void OnElementChanged(ElementChangedEventArgs<WebViewer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetUpControl();
            }
            if (e.OldElement != null)
            {
                CleanupElement(e.OldElement);
            }
            if (e.NewElement != null)
            {
                SetUpElement(e.NewElement);
            }
        }

        private void SetUpControl()
        {
            userController = new WKUserContentController();

            var config = new WKWebViewConfiguration { UserContentController = userController };
            var webView = new UIKit.UIWebView(Frame);
            webView.ScalesPageToFit = true;


            SetNativeControl(webView);
        }

        private void CleanupElement(WebViewer e)
        {
            e.Cleanup();
        }

        private void SetUpElement(WebViewer e)
        {
            if (Element.Uri != null)
            {
                AuthWebViewClient authWebViewClient = new AuthWebViewClient(e);
                Control.Delegate = authWebViewClient;

                if (e.HasCookiesOrHeaders)
                {
                    e.Uri = string.Format(
                        @" <!doctype html><html><body onload=""window.location.href = '{0}';""></body></html>",
                        e.Uri
                    );
                }

                if (WebViewRendererHelper.IsAnHttpUrl(Element.Uri))
                {
                    NSUrlRequest nsUrl = new NSUrlRequest(new NSUrl(Element.Uri));
                    Control.LoadRequest(nsUrl);
                }
                else
                {
                    var nsBaseUri = new NSUrl($"file://{BaseUrl}");
                    Control.LoadHtmlString(Element.Uri, nsBaseUri);
                }
            }
        }

    }

    public class AuthWebViewClient : UIWebViewDelegate
    {
        private WebViewer _webViewer;
        public AuthWebViewClient(WebViewer webViewer)
        {
            _webViewer = webViewer;
        }

        public override bool ShouldStartLoad(UIWebView webView, Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Headers.Count == 2)
            {
                //Create Mutable request and adding header values
                var requestCustom = new NSMutableUrlRequest(request.Url);

                AddHeaders(requestCustom);
                AddCookies(requestCustom);

                webView.LoadRequest(requestCustom);
            }
            return true;
        }

        private void AddHeaders(NSMutableUrlRequest request)
        {
            var header = _webViewer.Headers;

            if (header != null)
            {

                request.Headers = NSDictionary.FromObjectsAndKeys(header.Values.ToArray(),
                                                                    header.Keys.ToArray());
            }
        }

        private void AddCookies(NSMutableUrlRequest request)
        {
            var cookies = _webViewer.Cookies.GetCookies(request.Url);

            if (cookies != null)
            {

                request.ShouldHandleCookies = true;
                NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
                foreach (var cookie in NSHttpCookieStorage.SharedStorage.CookiesForUrl(request.Url))
                    NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);

                var cookiesEnumerator = cookies.GetEnumerator();

                var nsHtppcookies = new List<NSHttpCookie>();

                while (cookiesEnumerator.MoveNext())
                {
                    var cookie = cookiesEnumerator.Current as Cookie;
                    var cookieDict = new NSMutableDictionary();
                    cookieDict.Add(NSHttpCookie.KeyName, new NSString(cookie.Name));
                    cookieDict.Add(NSHttpCookie.KeyValue, new NSString(cookie.Value));
                    cookieDict.Add(NSHttpCookie.KeyPath, new NSString(cookie.Path));
                    cookieDict.Add(NSHttpCookie.KeyDomain, new NSString(cookie.Domain));
                    nsHtppcookies.Add(new NSHttpCookie(cookieDict));
                }
                NSHttpCookieStorage.SharedStorage.SetCookies(nsHtppcookies.ToArray(), request.Url, request.Url);
            }

        }

        public override void LoadFailed(UIWebView webView, NSError error)
        {
            //LoadCompletedWithErrors();
        }

        public override void LoadingFinished(UIWebView webView)
        {
            LoadCompletedSucessfully(webView);
        }

        private void LoadCompletedSucessfully(UIWebView webView)
        {
            bool fromFile = _webViewer.FromFile;
            bool httpRequest = WebViewRendererHelper.IsAnHttpUrl(webView.Request.Url.ToString());

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
