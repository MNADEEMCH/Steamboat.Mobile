using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class Checkbox : View
    {
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create(nameof(Checked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

        public static readonly BindableProperty CheckedTextProperty =
            BindableProperty.Create(nameof(CheckedText), typeof(string), typeof(Checkbox), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty UncheckedTextProperty =
            BindableProperty.Create(nameof(UncheckedText), typeof(string), typeof(Checkbox), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty DefaultTextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(Checkbox), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Checkbox), Color.Default, BindingMode.TwoWay);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Checkbox), 0.0, BindingMode.TwoWay);

        public static readonly BindableProperty FontNameProperty =
            BindableProperty.Create(nameof(FontName), typeof(string), typeof(Checkbox), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Checkbox), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Checkbox), null, BindingMode.TwoWay);

        public static readonly BindableProperty WhiteThemeProperty =
            BindableProperty.Create(nameof(WhiteTheme), typeof(bool), typeof(Checkbox), false, BindingMode.OneWay);

        public event EventHandler<bool> CheckedChanged;

        public bool Checked
        {
            get { return (bool)this.GetValue(CheckedProperty); }
            set { if (this.Checked != value) this.SetValue(CheckedProperty, value); }
        }

        public string CheckedText
        {
            get { return this.GetValue(CheckedTextProperty).ToString(); }
            set { this.SetValue(CheckedTextProperty, value); }
        }

        public string UncheckedText
        {
            get { return this.GetValue(UncheckedTextProperty).ToString(); }
            set { this.SetValue(UncheckedTextProperty, value); }
        }

        public string DefaultText
        {
            get { return this.GetValue(DefaultTextProperty).ToString(); }
            set { this.SetValue(DefaultTextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public string FontName
        {
            get { return (string)GetValue(FontNameProperty); }
            set { SetValue(FontNameProperty, value); }
        }

        public string Text
        {
            get
            {
                return this.Checked
                    ? (string.IsNullOrEmpty(this.CheckedText) ? this.DefaultText : this.CheckedText)
                        : (string.IsNullOrEmpty(this.UncheckedText) ? this.DefaultText : this.UncheckedText);
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool WhiteTheme
        {
            get { return (bool)this.GetValue(WhiteThemeProperty); }
            set { this.SetValue(WhiteThemeProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var val = bool.Parse(newvalue.ToString());
            var checkBox = (Checkbox)bindable;
            checkBox.Checked = val;
        }
    }
}
