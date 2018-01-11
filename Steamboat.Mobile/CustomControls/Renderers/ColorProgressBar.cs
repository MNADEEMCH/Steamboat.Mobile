using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class ColorProgressBar : ProgressBar
    {

        public static BindableProperty ProgressBarHeigthProperty
            = BindableProperty.Create(nameof(ProgressBarHeigth), typeof(float), typeof(ColorProgressBar), 0.0f);


        public static BindableProperty FilledColorProperty
            = BindableProperty.Create(nameof(FilledColor), typeof(Color), typeof(ColorProgressBar), default(Color));

        public static BindableProperty EmptyColorProperty
           = BindableProperty.Create(nameof(EmptyColor), typeof(Color), typeof(ColorProgressBar), default(Color));

        public float ProgressBarHeigth
        {
            get { return (float)GetValue(ProgressBarHeigthProperty); }
            set { SetValue(ProgressBarHeigthProperty, value); }
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
    }
}
