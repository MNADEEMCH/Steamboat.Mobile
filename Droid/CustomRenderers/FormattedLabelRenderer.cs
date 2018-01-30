
using System.ComponentModel;
using System.Reflection;
using Android.Text;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Graphics;
using Android.Text.Style;
using Android.Util;
using Android.Widget;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(FormattedLabel), typeof(FormattedLabelRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class FormattedLabelRenderer:LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            if (Element?.FormattedText == null)
                return;

            var extensionType = typeof(FormattedStringExtensions);
            var type = extensionType.GetNestedType("FontSpan", BindingFlags.NonPublic);
            var ss = new SpannableString(Control.TextFormatted);
            var spans = ss.GetSpans(0, ss.ToString().Length, Class.FromType(type));
            foreach (var span in spans)
            {
                var font = (Font)type.GetProperty("Font").GetValue(span, null);
                if ((font.FontFamily ?? Element.FontFamily) != null)
                {
                    var start = ss.GetSpanStart(span);
                    var end = ss.GetSpanEnd(span);
                    var flags = ss.GetSpanFlags(span);
                    ss.RemoveSpan(span);
                    var newSpan = new CustomTypefaceSpan(Control, Element, font);
                    ss.SetSpan(newSpan, start, end, flags);
                }
            }
            Control.TextFormatted = ss;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.FormattedTextProperty.PropertyName ||
                e.PropertyName == Label.TextProperty.PropertyName ||
                e.PropertyName == Label.FontAttributesProperty.PropertyName ||
                e.PropertyName == Label.FontProperty.PropertyName ||
                e.PropertyName == Label.FontSizeProperty.PropertyName ||
                e.PropertyName == Label.FontFamilyProperty.PropertyName ||
                e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateFormattedText();
            }
        }
    }

    public class CustomTypefaceSpan : MetricAffectingSpan
    {
        private readonly Typeface _typeFace;
        private readonly TextView _textView;
        private Font _font;

        public CustomTypefaceSpan(TextView textView, Label label, Font font)
        {
            _textView = textView;
            _font = font;
            _typeFace = Typeface.CreateFromAsset(Forms.Context.Assets, GetFontName(_font.FontFamily ?? label.FontFamily, _font.FontAttributes));
        }

        private static string GetFontName(string fontFamily, FontAttributes fontAttributes)
        {
            var postfix = "Regular";
            var bold = fontAttributes.HasFlag(FontAttributes.Bold);
            var italic = fontAttributes.HasFlag(FontAttributes.Italic);
            if (bold && italic) { postfix = "BoldItalic"; }
            else if (bold) { postfix = "Bold"; }
            else if (italic) { postfix = "Italic"; }

            return $"fonts/{fontFamily}-{postfix}.ttf";
        }

        public override void UpdateDrawState(TextPaint paint)
        {
            ApplyCustomTypeFace(paint);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            ApplyCustomTypeFace(paint);
        }

        private void ApplyCustomTypeFace(Paint paint)
        {
            paint.SetTypeface(_typeFace);
            paint.TextSize = TypedValue.ApplyDimension(ComplexUnitType.Sp, _font.ToScaledPixel(), _textView.Resources.DisplayMetrics);
        }
    }
}
