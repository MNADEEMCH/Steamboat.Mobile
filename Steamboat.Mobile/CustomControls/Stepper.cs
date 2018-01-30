using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class Stepper : RelativeLayout
    {
        #region Properties

        private const double ProgressBarYParentRelativePercentage = 0.25;
        private const double StepImageHalfSizeConstant = 12;
        private const double LabelYParentRelativePercentage = 0.65;

        private ColorProgressBar _colorProgressBar;
        private List<Image> _stepsProgressImages;
        private List<Label> _stepsProgressLabels;

        public ColorProgressBar ColorProgressBar { get { return _colorProgressBar; } }
        public List<Image> StepsProgressImages { get { return _stepsProgressImages; } }
        public List<Label> StepsProgressLabels { get { return _stepsProgressLabels; } }

        public static BindableProperty InitializeExcecuteProperty =
            BindableProperty.Create(nameof(InitializeExcecute), typeof(int), typeof(Stepper), 0, BindingMode.TwoWay, propertyChanged: HandleInitializeExecutePropertyChanged);
        public static void HandleInitializeExecutePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.InitializeExcecute = (int)newValue;
            if(stepper.InitializeExcecute!=0)
                stepper.Initialize();
        }
        public int InitializeExcecute
        {
            get { return (int)GetValue(InitializeExcecuteProperty); }
            set
            {
                SetValue(InitializeExcecuteProperty, value);
            }
        }

        public static BindableProperty RefreshExcecuteProperty =
            BindableProperty.Create(nameof(RefreshExcecute), typeof(int), typeof(Stepper), 0, BindingMode.TwoWay, propertyChanged: HandleRefreshExecutePropertyChanged);
        public static void HandleRefreshExecutePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.RefreshExcecute = (int)newValue;
            if (stepper.RefreshExcecute != 0)
                stepper.Refresh();

        }
        public int RefreshExcecute
        {
            get { return (int)GetValue(RefreshExcecuteProperty); }
            set
            {
                SetValue(RefreshExcecuteProperty, value);
            }
        }

        public static BindableProperty PreviousStepProperty =
           BindableProperty.Create(nameof(PreviousStep), typeof(int), typeof(Stepper), 0, BindingMode.TwoWay, propertyChanged: HandlePreviousStepPropertyChanged);
        public static void HandlePreviousStepPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.PreviousStep = (int)newValue;
        }
        public int PreviousStep
        {
            get { return (int)GetValue(PreviousStepProperty); }
            set
            {
                SetValue(PreviousStepProperty, value);
            }
        }

        public static BindableProperty CurrentStepProperty =
           BindableProperty.Create(nameof(CurrentStep), typeof(int), typeof(Stepper), 0, BindingMode.TwoWay, propertyChanged: HandleCurrentStepPropertyChanged);
        public static void HandleCurrentStepPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.CurrentStep = (int)newValue;
        }
        public int CurrentStep
        {
            get { return (int)GetValue(CurrentStepProperty); }
            set
            {
                SetValue(CurrentStepProperty, value);
            }
        }

        public static BindableProperty StepsProperty =
           BindableProperty.Create(nameof(Steps), typeof(int), typeof(Stepper), 0, BindingMode.TwoWay, propertyChanged: HandleStepsPropertyChanged);
        public static void HandleStepsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.Steps = (int)newValue;
        }
        public int Steps
        {
            get { return (int)GetValue(StepsProperty); }
            set
            {
                SetValue(StepsProperty, value);
            }
        }


        public static BindableProperty ProgressLengthProperty =
          BindableProperty.Create(nameof(ProgressLength), typeof(int), typeof(Stepper), 1500);
        public int ProgressLength
        {
            get { return (int)GetValue(ProgressLengthProperty); }
            set
            {
                SetValue(ProgressLengthProperty, value);
            }
        }

        public static BindableProperty SourceImgDoneProperty =
           BindableProperty.Create(nameof(SourceImgDone), typeof(string), typeof(Stepper), String.Empty);
        public string SourceImgDone
        {
            get { return (string)GetValue(SourceImgDoneProperty); }
            set
            {
                SetValue(SourceImgDoneProperty, value);
            }
        }

        public static BindableProperty SourceImgActiveProperty =
           BindableProperty.Create(nameof(SourceImgActive), typeof(string), typeof(Stepper), String.Empty);
        public string SourceImgActive
        {
            get { return (string)GetValue(SourceImgActiveProperty); }
            set
            {
                SetValue(SourceImgActiveProperty, value);
            }
        }

        public static BindableProperty SourceImgPendingProperty =
           BindableProperty.Create(nameof(SourceImgPending), typeof(string), typeof(Stepper), String.Empty);
        public string SourceImgPending
        {
            get { return (string)GetValue(SourceImgPendingProperty); }
            set
            {
                SetValue(SourceImgPendingProperty, value);
            }
        }


        public static BindableProperty StepInactiveLabelStyleProperty =
           BindableProperty.Create(nameof(StepInactiveLabelStyle), typeof(Style), typeof(Stepper), null);
        public Style StepInactiveLabelStyle
        {
            get { return (Style)GetValue(StepInactiveLabelStyleProperty); }
            set
            {
                SetValue(StepInactiveLabelStyleProperty, value);
            }
        }
        public static BindableProperty StepActiveLabelStyleProperty =
           BindableProperty.Create(nameof(StepActiveLabelStyle), typeof(Style), typeof(Stepper), null);
        public Style StepActiveLabelStyle
        {
            get { return (Style)GetValue(StepActiveLabelStyleProperty); }
            set
            {
                SetValue(StepActiveLabelStyleProperty, value);
            }
        }

        public static BindableProperty StepImageStyleProperty =
           BindableProperty.Create(nameof(StepImageStyle), typeof(Style), typeof(Stepper), null);
        public Style StepImageStyle
        {
            get { return (Style)GetValue(StepImageStyleProperty); }
            set
            {
                SetValue(StepImageStyleProperty, value);
            }
        }

        public static BindableProperty ProgressBarStyleProperty =
          BindableProperty.Create(nameof(ProgressBarStyle), typeof(Style), typeof(Stepper), null, BindingMode.TwoWay, propertyChanged: HandleProgressBarStylePropertyChanged);
        public static void HandleProgressBarStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stepper = (Stepper)bindable;
            stepper.ProgressBarStyle = newValue as Style;
            stepper.SetProgressBarStyle();
        }
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set
            {
                SetValue(ProgressBarStyleProperty, value);
            }
        }

        #endregion

        public Stepper()
        {
            _colorProgressBar = new ColorProgressBar();

            this.Children.Add(_colorProgressBar,
               yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height * ProgressBarYParentRelativePercentage; }),
               widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; })
            );
        }

        public void SetProgressBarStyle()
        {
            _colorProgressBar.Style = ProgressBarStyle;
        }

        public void Initialize()
        {
            if (Steps > 0)
                AddSteps();

        }
        public void Refresh(){
            ProgressFromStepToStep(PreviousStep, CurrentStep);
        }

        private void AddSteps()
        {

            _stepsProgressImages = new List<Image>();
            _stepsProgressLabels = new List<Label>();
            if (Steps == 4)
            {
                string imgSource = GetImageSourceForStep(PreviousStep, 1);
                AddStepImage(imgSource, 0);
                AddStepLabel("INTERVIEW", 0, -28);

                imgSource = GetImageSourceForStep(PreviousStep, 2);
                AddStepImage(imgSource, 0.33);
                AddStepLabel("SCHEDULING", 0.33, -33);

                imgSource = GetImageSourceForStep(PreviousStep, 3);
                AddStepImage(imgSource, 0.66);
                AddStepLabel("SCREENING", 0.66, -28);

                imgSource = GetImageSourceForStep(PreviousStep, 4);
                AddStepImage(imgSource, 1);
                AddStepLabel("REPORT", 1, -19);
            }
            else if (Steps == 3)
            {
                string imgSource = GetImageSourceForStep(PreviousStep, 1);
                AddStepImage(imgSource, 0);
                AddStepLabel("SCHEDULING", 0, -33);

                imgSource = GetImageSourceForStep(PreviousStep, 2);
                AddStepImage(imgSource, 0.5);
                AddStepLabel("SCREENING", 0.5, -30);

                imgSource = GetImageSourceForStep(PreviousStep, 3);
                AddStepImage(imgSource, 1);
                AddStepLabel("REPORT", 1, -19);
            }

        }

        private async void ProgressFromStepToStep(int fromStep, int toStep)
        {
            _colorProgressBar.Progress = GetProgressForStep(Steps, fromStep);
            Label labelFrom = GetLabelByStep(fromStep);
            Label labelTo = GetLabelByStep(toStep);

            labelFrom.Style = StepInactiveLabelStyle;
            labelTo.Style = StepActiveLabelStyle;

            while (fromStep < toStep)
            {
                GetImageByStep(fromStep).Source = GetImageSourceForStep(toStep, fromStep);
                fromStep++;
                var progressTo = GetProgressForStep(Steps, fromStep);
                await _colorProgressBar.ProgressTo(progressTo, (uint)ProgressLength, Easing.SinOut);
            }

            GetImageByStep(toStep).Source = GetImageSourceForStep(toStep, fromStep);
        }

        private void AddStepImage(string imgSource, double parentXRelativePercentage)
        {
            Image stepImage = new Image();
            stepImage.Style = StepImageStyle;
            stepImage.Source = imgSource;

            this.Children.Add(stepImage, yConstraint: Constraint.RelativeToView
                                                            (_colorProgressBar,(parent,progressbar)=>
                                                                { return this.PositionImageAtVerticalCenterOfProgressBar(progressbar.Y, progressbar.Height / 2, StepImageHalfSizeConstant);
                                                                }
                                                             ),
                                         xConstraint: Constraint.RelativeToParent
                                                            ((parent) => 
                                                                { return this.PositionImageAtHorizontalPercentageOfProgressBar(parent.Width,parentXRelativePercentage,StepImageHalfSizeConstant);
                                                                }
                                                             ));

            _stepsProgressImages.Add(stepImage);
        }

        private double PositionImageAtVerticalCenterOfProgressBar(double progressBarY,double progressBarHalfHeight,double imageHalfSize)
        {
            return progressBarY + progressBarHalfHeight - imageHalfSize;
        }
        private double PositionImageAtHorizontalPercentageOfProgressBar(double stepperWith,double stepperXRelativePercentage, double imageHalfSize)
        {
            return (stepperWith * stepperXRelativePercentage) - imageHalfSize;
        }

        private void AddStepLabel(string label, double parentXRelativePercentage, double constantForLabel)
        {
            Label stepLabel = new Label();
            stepLabel.Style = StepInactiveLabelStyle;
            stepLabel.Text = label;

            this.Children.Add(stepLabel,
               yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height * LabelYParentRelativePercentage); }),
               xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width * parentXRelativePercentage) + constantForLabel; }));

            _stepsProgressLabels.Add(stepLabel);
        }

        private string GetImageSourceForStep(int currentStep, int step)
        {
            string imageSource = "";

            if (step == currentStep)
                imageSource= SourceImgActive;
            else if (step < currentStep)
                imageSource= SourceImgDone;
            else
                imageSource= SourceImgPending;

            return imageSource;
        }
        private Image GetImageByStep(int step)
        {
            return _stepsProgressImages[step - 1];
        }
        private Label GetLabelByStep(int step)
        {
            return _stepsProgressLabels[step - 1];
        }
        private double GetProgressForStep(int steps, int step)
        {
            return (double)(step - 1) / (double)(steps - 1);
        }
    }
}
