using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class ColorProgressBar : ProgressBar
    {
        public ColorProgressBar()
        {
            ChangeProgressCommand = new Command<double>((progress) =>
            {
                this.ProgressChanged(progress);
            });
        }

        private void ProgressChanged(double progress)
        {
            this.ProgressTo(progress, 800, Easing.SinOut);
        }


		public static BindableProperty ProgressBarHeigthProperty
        = BindableProperty.Create(nameof(ProgressBarHeigth), typeof(double), typeof(ColorProgressBar), 0.0);


        public static BindableProperty FilledColorProperty
            = BindableProperty.Create(nameof(FilledColor), typeof(Color), typeof(ColorProgressBar), default(Color));

        public static BindableProperty EmptyColorProperty
           = BindableProperty.Create(nameof(EmptyColor), typeof(Color), typeof(ColorProgressBar), default(Color));


        public static BindableProperty ChangeProgressCommandProperty
        = BindableProperty.Create(nameof(ChangeProgressCommand), typeof(ICommand), typeof(ColorProgressBar), null, BindingMode.OneWayToSource);


        public ICommand ChangeProgressCommand
        {
            get { return (ICommand)GetValue(ChangeProgressCommandProperty); }
            set { SetValue(ChangeProgressCommandProperty, value); }
        }

        public Color FilledColor
        {
            get { return (Color)GetValue(FilledColorProperty); }
            set { SetValue(FilledColorProperty, value); }
        }

        public Color EmptyColor
        {
            get { return (Color)GetValue(EmptyColorProperty); }
            set { SetValue(EmptyColorProperty, value); }
        }

        public double ProgressBarHeigth
        {
            get { return (double)GetValue(ProgressBarHeigthProperty); }
            set { SetValue(ProgressBarHeigthProperty, value); }
        }
    }
}
