using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Models.Stepper;
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

        private int _steps;
        private int _previousStep;
        private int _currentStep;

        public int Steps { set { _steps = value; } get { return _steps; } }
        public int PreviousStep { set { _previousStep = value; } get { return _previousStep; } }
        public int CurrentStep { set { _currentStep = value; } get { return _currentStep; } }

        public ColorProgressBar ColorProgressBar { get { return _colorProgressBar; } }
        public List<Image> StepsProgressImages { get { return _stepsProgressImages; } }
        public List<Label> StepsProgressLabels { get { return _stepsProgressLabels; } }


        public static BindableProperty DrawStepperCommandProperty
       = BindableProperty.Create(nameof(DrawStepperCommand), typeof(ICommand), typeof(Stepper), null, BindingMode.OneWayToSource);


        public ICommand DrawStepperCommand
        {
            get { return (ICommand)GetValue(DrawStepperCommandProperty); }
            set { SetValue(DrawStepperCommandProperty, value); }
        }


        public static BindableProperty AnimateProgressProperty =
            BindableProperty.Create(nameof(AnimateProgress), typeof(bool), typeof(Stepper), false, BindingMode.TwoWay);
        public bool AnimateProgress
        {
            get { return (bool)GetValue(AnimateProgressProperty); }
            set
            {
                SetValue(AnimateProgressProperty, value);
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
            DrawStepperCommand = new Command<StepperParam>((stepperParam) =>
            {
                this.DrawStepper(stepperParam);
            });

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

        private void SetParameters(StepperParam stepperParam){
            if (stepperParam != null)
            {
                this.Steps = stepperParam.Steps;
                this.CurrentStep = stepperParam.CurrentStep;
                this.PreviousStep = stepperParam.PreviousStep;
            }
        }

        public void DrawStepper(StepperParam stepperParam){
            
            this.SetParameters(stepperParam);

            if (AnimateProgress)
            {
                AddSteps(Steps, PreviousStep);
                ProgressFromStepToStep(PreviousStep, CurrentStep);
            }
            else{
                AddSteps(Steps, CurrentStep);
                _colorProgressBar.Progress = GetProgressForStep(Steps, CurrentStep);
            }
        }

        private void AddSteps(int steps, int currentStep)
        {

            _stepsProgressImages = new List<Image>();
            _stepsProgressLabels = new List<Label>();
            if (steps == 4)
            {
                int step = 1;
                string imgSource = GetImageSourceForStep(currentStep, step);
                Style labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 0);
                AddStepLabel(labelStyle,"INTERVIEW", 0, -28);

                step = 2;
                imgSource = GetImageSourceForStep(currentStep, step);
                labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 0.33);
                AddStepLabel(labelStyle,"SCHEDULING", 0.33, -33);

                step = 3;
                imgSource = GetImageSourceForStep(currentStep, step);
                labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 0.66);
                AddStepLabel(labelStyle,"SCREENING", 0.66, -28);

                step = 4;
                imgSource = GetImageSourceForStep(currentStep, step);
                labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 1);
                AddStepLabel(labelStyle,"REPORT", 1, -19);
            }
            else if (steps == 3)
            {
                int step = 1;
                string imgSource = GetImageSourceForStep(currentStep, step);
                Style labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 0);
                AddStepLabel(labelStyle,"SCHEDULING", 0, -33);

                step = 2;
                imgSource = GetImageSourceForStep(currentStep, step);
                labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 0.5);
                AddStepLabel(labelStyle,"SCREENING", 0.5, -30);

                step = 3;
                imgSource = GetImageSourceForStep(currentStep, step);
                labelStyle = currentStep == step ? StepActiveLabelStyle : StepInactiveLabelStyle;
                AddStepImage(imgSource, 1);
                AddStepLabel(labelStyle,"REPORT", 1, -19);
            }

        }

        private async void ProgressFromStepToStep(int fromStep, int toStep)
        {
            _colorProgressBar.Progress = GetProgressForStep(Steps, fromStep);

            if (fromStep < toStep)
                await DrawProgress(fromStep, toStep);
            else if(fromStep > toStep)
                await DrawUnprogress(fromStep, toStep);
                
        }

        private async Task DrawProgress(int fromStep, int toStep){
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

        private async Task DrawUnprogress(int fromStep, int toStep){
            Label labelFrom = GetLabelByStep(fromStep);
            Label labelTo = GetLabelByStep(toStep);

            labelFrom.Style = StepInactiveLabelStyle;
            labelTo.Style = StepActiveLabelStyle;

            while (fromStep > toStep)
            {
                GetImageByStep(fromStep).Source = GetImageSourceForStep(toStep,fromStep);
                fromStep--;
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

        private void AddStepLabel(Style labelStyle, string label, double parentXRelativePercentage, double constantForLabel)
        {
            Label stepLabel = new Label();
            stepLabel.Style = labelStyle;
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
