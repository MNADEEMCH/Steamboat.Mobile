﻿using System;
using System.ComponentModel;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.Controls;
using Steamboat.Mobile.iOS.CustomRenderers;
using Steamboat.Mobile.iOS.Utilities;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CheckboxRenderer : ViewRenderer<Checkbox, CheckBoxView>
    {
        private UIColor defaultTextColor;

        public CheckboxRenderer()
        {
            SetNeedsLayout();
        }

        /// <summary>
        /// Handles the Element Changed event
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);

            if (Element == null) return;

            BackgroundColor = Element.BackgroundColor.ToUIColor();
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var checkBox = new CheckBoxView(Bounds);
                    checkBox.TouchUpInside += OnTouchUp;
                    defaultTextColor = checkBox.TitleColor(UIControlState.Normal);
                    SetNativeControl(checkBox);
                }
                Control.LineBreakMode = UILineBreakMode.WordWrap;
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                Control.CheckedTitle = string.IsNullOrEmpty(e.NewElement.CheckedText) ? e.NewElement.DefaultText : e.NewElement.CheckedText;
                Control.UncheckedTitle = string.IsNullOrEmpty(e.NewElement.UncheckedText) ? e.NewElement.DefaultText : e.NewElement.UncheckedText;
                Control.Checked = e.NewElement.Checked;
                UpdateTextColor();
            }

            Control.Frame = Frame;
            Control.Bounds = Bounds;

            UpdateFont();
        }

        /// <summary>
        /// Resizes the text.
        /// </summary>
        private void ResizeText()
        {
            if (Element == null)
                return;

            var text = Element.Checked ? string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText :
                string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;

            var bounds = Control.Bounds;

            var width = Control.TitleLabel.Bounds.Width;

            var height = Utilities.Extensions.MeasureTextSize(text, width + 30, 14.0);
                      
            var minHeight = string.Empty.StringHeight(Control.Font, width);

            var requiredLines = Math.Round(height.Height / minHeight, MidpointRounding.AwayFromZero);

            var supportedLines = Math.Round(bounds.Height / minHeight, MidpointRounding.ToEven);

            if (supportedLines != requiredLines)
            {
                bounds.Height += (float)(minHeight * (requiredLines - supportedLines));
                Control.Bounds = bounds;
                Element.HeightRequest = bounds.Height;
            }
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            ResizeText();
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty(Element.FontName))
            {
                var font = UIFont.FromName(Element.FontName, (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f);
                if (font != null)
                {
                    Control.Font = font;
                }
            }
            else if (Element.FontSize > 0)
            {
                var font = UIFont.FromName(Control.Font.Name, (float)Element.FontSize);
                if (font != null)
                {
                    Control.Font = font;
                }
            }
        }

        private void UpdateTextColor()
        {
            Control.SetTitleColor(Element.TextColor.ToUIColorOrDefault(defaultTextColor), UIControlState.Normal);
            Control.SetTitleColor(Element.TextColor.ToUIColorOrDefault(defaultTextColor), UIControlState.Selected);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    UpdateTextColor();
                    break;
                case "CheckedText":
                    Control.CheckedTitle = string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText;
                    SetNeedsDisplay();
                    break;
                case "UncheckedText":
                    Control.UncheckedTitle = string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;
                    break;
                case "FontSize":
                    UpdateFont();
                    break;
                case "FontName":
                    UpdateFont();
                    break;
                case "Element":
                    break;
                default:
                    return;
            }
        }

        private void OnTouchUp(object sender, EventArgs e)
        {
            Element.Checked = Control.Checked;

            if (Element.Command.CanExecute(null))
                Element.Command.Execute(null);
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.TouchUpInside -= OnTouchUp;
            }

            base.Dispose(disposing);
        }
    }
}