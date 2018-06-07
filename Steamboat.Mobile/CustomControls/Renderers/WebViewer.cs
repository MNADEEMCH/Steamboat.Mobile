using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class WebViewer : WebView
    {
        Action<string> action;

        public static BindableProperty UriProperty =
            BindableProperty.Create(nameof(Uri), typeof(string), typeof(WebViewer), null, BindingMode.Default);

        public static BindableProperty CookiesProperty =
            BindableProperty.Create(nameof(Cookies), typeof(CookieContainer), typeof(WebViewer), null, BindingMode.Default);

        public static BindableProperty HeadersProperty =
            BindableProperty.Create(nameof(Headers), typeof(Dictionary<string,string>), typeof(WebViewer), null, BindingMode.Default);

        public static BindableProperty FromFileProperty =
            BindableProperty.Create(nameof(FromFile), typeof(bool), typeof(WebViewer), false, BindingMode.Default);
        
        public static BindableProperty LoadFinishedCommandProperty =
            BindableProperty.Create(nameof(LoadFinishedCommand), typeof(ICommand), typeof(WebViewer), null, BindingMode.Default);

        public string Uri{ 
            get{ return (string)GetValue(UriProperty); } 
            set{ SetValue(UriProperty, value);}
        }

        public CookieContainer Cookies{ 
            get { return (CookieContainer)GetValue(CookiesProperty); } 
            set { SetValue(CookiesProperty, value); }
        }

        public Dictionary<string, string> Headers{ 
            get { return (Dictionary<string, string>)GetValue(HeadersProperty); } 
            set { SetValue(HeadersProperty, value);}
        }

        public bool HasCookiesOrHeaders { 
            get { return Headers != null || Cookies != null; } 
        }

        public bool FromFile{ 
            get { return (bool)GetValue(FromFileProperty); } 
            set {SetValue(FromFileProperty, value);} 
        }

        public ICommand LoadFinishedCommand{ 
            get { return (ICommand)GetValue(LoadFinishedCommandProperty); } 
            set { SetValue(LoadFinishedCommandProperty, value);}
        }

        public void Cleanup()
        {
            action = null;
        }

    }
}
