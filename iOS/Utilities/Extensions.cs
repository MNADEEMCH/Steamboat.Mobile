using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Steamboat.Mobile.iOS.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Converts the UIColor to a Xamarin Color object.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="defaultColor">The default color.</param>
        /// <returns>UIColor.</returns>
        public static UIColor ToUIColorOrDefault(this Xamarin.Forms.Color color, UIColor defaultColor)
        {
            if (color == Xamarin.Forms.Color.Default)
                return defaultColor;

            return color.ToUIColor();
        }

        /// <summary>
        /// Gets the height of a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="width">The width.</param>
        /// <returns>nfloat.</returns>
        public static nfloat StringHeight(this string text, UIFont font, nfloat width)
        {
            return text.StringRect(font, width).Height;
        }

        /// <summary>
        /// To the native string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>NSString.</returns>
        public static NSString ToNativeString(this string text)
        {
            return new NSString(text);
        }

        /// <summary>
        /// Gets the rectangle of a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="width">The width.</param>
        /// <returns>CGRect.</returns>
        public static CGRect StringRect(this string text, UIFont font, nfloat width)
        {
            return text.ToNativeString().GetBoundingRect(
                new CGSize(width, nfloat.MaxValue),
                NSStringDrawingOptions.OneShot,//.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin,
                new UIStringAttributes { Font = font },
                null);
        }

        /// <summary>
        /// Strings the size.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <returns>CGSize.</returns>
        public static CGSize StringSize(this string text, UIFont font)
        {
            return text.ToNativeString().GetSizeUsingAttributes(new UIStringAttributes { Font = font });
        }

        public static Xamarin.Forms.Size MeasureTextSize(string text, double width, double fontSize, string fontName = null)
        {
            var nsText = new NSString(text);
            var boundSize = new SizeF((float)width, float.MaxValue);
            var options = NSStringDrawingOptions.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin;

            if (fontName == null)
            {
                fontName = "HelveticaNeue";
            }

            var attributes = new UIStringAttributes
            {
                Font = UIFont.FromName(fontName, (float)fontSize)
            };

            var sizeF = nsText.GetBoundingRect(boundSize, options, attributes, null).Size;

            return new Xamarin.Forms.Size((double)sizeF.Width, (double)sizeF.Height);
        }
    }
}
