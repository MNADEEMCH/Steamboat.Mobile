// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CheckBoxRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CheckboxRenderer : ViewRenderer<Checkbox, Android.Widget.CheckBox>
    {
        private ColorStateList defaultTextColor;

        public CheckboxRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var checkBox = new Android.Widget.CheckBox(this.Context);
                checkBox.CheckedChange += CheckBoxCheckedChange;

                defaultTextColor = checkBox.TextColors;
                this.SetNativeControl(checkBox);
            }

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;
            UpdateTextColor();

            ChangeThemeIfNeeded(e.NewElement);

            if (e.NewElement.FontSize > 0)
            {
                Control.TextSize = (float)e.NewElement.FontSize;
            }

            if (!string.IsNullOrEmpty(e.NewElement.FontName))
            {
                Control.Typeface = TrySetFont(e.NewElement.FontName);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Text = Element.Text;
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    UpdateTextColor();
                    break;
                case "FontName":
                    if (!string.IsNullOrEmpty(Element.FontName))
                    {
                        Control.Typeface = TrySetFont(Element.FontName);
                    }
                    break;
                case "FontSize":
                    if (Element.FontSize > 0)
                    {
                        Control.TextSize = (float)Element.FontSize;
                    }
                    break;
                case "CheckedText":
                case "UncheckedText":
                    Control.Text = Element.Text;
                    break;
                default:
                    break;
            }
        }

        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            if (this.Element.Checked != e.IsChecked)
            {
                this.Element.Checked = e.IsChecked;

                if (Element.Command != null && Element.Command.CanExecute(Element.CommandParameter))
                    Element.Command.Execute(Element.CommandParameter);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.CheckedChange -= CheckBoxCheckedChange;
            }

            base.Dispose(disposing);
        }

        private Typeface TrySetFont(string fontName)
        {
            Typeface tf = Typeface.Default;

            fontName = fontName.Split('#')[0];

            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, fontName);
                return tf;
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);
                    return tf;
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);
                    return Typeface.Default;
                }
            }
        }

        private void UpdateTextColor()
        {
            if (Control == null || Element == null)
                return;

            if (Element.TextColor == Xamarin.Forms.Color.Default){
                Control.SetTextColor(defaultTextColor);
            }
            else
                Control.SetTextColor(Element.TextColor.ToAndroid());
        }

        private void ChangeThemeIfNeeded(Checkbox e)
        {
            if (e.WhiteTheme)
            {
                AddButtonColor();
            }
        }

        private void AddButtonColor(){
            
            int[][] states = {
                    new int[] { Android.Resource.Attribute.StateEnabled},
                    new int[] {Android.Resource.Attribute.StateChecked},
                    new int[] { Android.Resource.Attribute.StatePressed }
                };
            int[] colors = {
                    System.Drawing.Color.White.ToArgb(),
                    System.Drawing.Color.White.ToArgb(),
                    System.Drawing.Color.White.ToArgb()
                };

            if (Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.Lollipop)
            {
                Control.ButtonTintList = new ColorStateList(states, colors);
            }
            else
            {
                ViewCompat.SetBackgroundTintList(Control, new ColorStateList(states, colors));
            }
        }
    }
}
