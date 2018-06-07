using System;
namespace Steamboat.Mobile.Helpers
{
    public static class WebViewRendererHelper
    {
        public static bool IsAnHttpUrl(string url)
        {
            return url.StartsWith("http", StringComparison.CurrentCulture);
        }
    }
}
