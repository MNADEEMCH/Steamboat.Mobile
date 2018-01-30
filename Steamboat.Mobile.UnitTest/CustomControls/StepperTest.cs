using NUnit.Framework;
using Steamboat.Mobile.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.UnitTest.CustomControls
{
    [TestFixture]
    public class StepperTest
    {
        #region Properties
        private const string SourceImgActive = "ImageActive.png";
        private const string SourceImgPending = "ImagePending.png";
        private const string SourceImgDone = "ImageDone.png";
        private Style _labelInactiveStyle = new Style(typeof(Label));
        private Style _labelActiveStyle = new Style(typeof(Label));
        #endregion

        private Mobile.CustomControls.Stepper CreateStepper()
        {
            Mobile.CustomControls.Stepper stepper = new Mobile.CustomControls.Stepper();

            //Setted on View
            stepper.StepInactiveLabelStyle = _labelInactiveStyle;
            stepper.StepActiveLabelStyle = _labelActiveStyle;
            stepper.SourceImgActive = SourceImgActive;
            stepper.SourceImgPending = SourceImgPending;
            stepper.SourceImgDone = SourceImgDone;

            return stepper;
        }

        [Test]
        public void Stepper_Initialize_3StepsCurrentStep1()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 3;
            stepper.PreviousStep = 1;
            stepper.CurrentStep = 1;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(3,StepperImages.Count);
            Assert.AreEqual(SourceImgActive,((FileImageSource)StepperImages[0].Source).File );
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[2].Source).File);
            //Labels
            Assert.AreEqual(3,StepperLabels.Count);
            Assert.AreEqual(_labelActiveStyle,StepperLabels[0].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[1].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[2].Style);
            //ProgressBar
            Assert.AreEqual(0,ProgressBar.Progress);
        }

        [Test]
        public void Stepper_Initialize_3StepsCurrentStep2()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 3;
            stepper.PreviousStep = 2;
            stepper.CurrentStep = 2;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(3,StepperImages.Count);
            Assert.AreEqual(SourceImgDone,((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgActive,((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[2].Source).File);
            //Labels
            Assert.AreEqual(3,StepperLabels.Count);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[0].Style);
            Assert.AreEqual(_labelActiveStyle,StepperLabels[1].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[2].Style);
            //ProgressBar
            Assert.AreEqual(0.5,ProgressBar.Progress);
        }

        [Test]
        public void Stepper_Initialize_3StepsCurrentStep3()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 3;
            stepper.PreviousStep = 3;
            stepper.CurrentStep = 3;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(3, StepperImages.Count);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgActive, ((FileImageSource)StepperImages[2].Source).File);
            //Labels
            Assert.AreEqual(3, StepperLabels.Count);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[0].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[1].Style);
            Assert.AreEqual(_labelActiveStyle, StepperLabels[2].Style);
            //ProgressBar
            Assert.AreEqual(1, ProgressBar.Progress);
        }


        [Test]
        public void Stepper_Initialize_4StepsCurrentStep1()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 4;
            stepper.PreviousStep = 1;
            stepper.CurrentStep = 1;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(4,StepperImages.Count);
            Assert.AreEqual(SourceImgActive,((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[2].Source).File);
            Assert.AreEqual(SourceImgPending,((FileImageSource)StepperImages[3].Source).File);
            //Labels
            Assert.AreEqual(4,StepperLabels.Count);
            Assert.AreEqual(_labelActiveStyle,StepperLabels[0].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[1].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[2].Style);
            Assert.AreEqual(_labelInactiveStyle,StepperLabels[3].Style);
            //ProgressBar
            Assert.AreEqual(0,ProgressBar.Progress);
        }

        [Test]
        public void Stepper_Initialize_4StepsCurrentStep2()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 4;
            stepper.PreviousStep = 2;
            stepper.CurrentStep = 2;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(4, StepperImages.Count);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgActive, ((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgPending, ((FileImageSource)StepperImages[2].Source).File);
            Assert.AreEqual(SourceImgPending, ((FileImageSource)StepperImages[3].Source).File);
            //Labels
            Assert.AreEqual(4, StepperLabels.Count);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[0].Style);
            Assert.AreEqual(_labelActiveStyle, StepperLabels[1].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[2].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[3].Style);
            //ProgressBar
            string progressAsDecimalString = ProgressBar.Progress.ToString().Substring(0, 4);
            Assert.AreEqual("0.33",progressAsDecimalString);
        }

        [Test]
        public void Stepper_Initialize_4StepsCurrentStep3()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 4;
            stepper.PreviousStep = 3;
            stepper.CurrentStep = 3;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(4, StepperImages.Count);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgActive, ((FileImageSource)StepperImages[2].Source).File);
            Assert.AreEqual(SourceImgPending, ((FileImageSource)StepperImages[3].Source).File);
            //Labels
            Assert.AreEqual(4, StepperLabels.Count);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[0].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[1].Style);
            Assert.AreEqual(_labelActiveStyle, StepperLabels[2].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[3].Style);
            //ProgressBar
            string progressAsDecimalString = ProgressBar.Progress.ToString().Substring(0, 4);
            Assert.AreEqual("0.66",progressAsDecimalString);
        }

        [Test]
        public void Stepper_Initialize_4StepsCurrentStep4()
        {
            Mobile.CustomControls.Stepper stepper = CreateStepper();

            //Setted from View Model
            stepper.Steps = 4;
            stepper.PreviousStep = 4;
            stepper.CurrentStep = 4;
            stepper.InitializeExcecute = 1;
            stepper.RefreshExcecute = 1;

            List<Image> StepperImages = stepper.StepsProgressImages;
            List<Label> StepperLabels = stepper.StepsProgressLabels;
            ColorProgressBar ProgressBar = stepper.ColorProgressBar;

            //Images
            Assert.AreEqual(4, StepperImages.Count);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[0].Source).File);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[1].Source).File);
            Assert.AreEqual(SourceImgDone, ((FileImageSource)StepperImages[2].Source).File);
            Assert.AreEqual(SourceImgActive, ((FileImageSource)StepperImages[3].Source).File);
            //Labels
            Assert.AreEqual(4, StepperLabels.Count);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[0].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[1].Style);
            Assert.AreEqual(_labelInactiveStyle, StepperLabels[2].Style);
            Assert.AreEqual(_labelActiveStyle, StepperLabels[3].Style);
            //ProgressBar
            Assert.AreEqual(1, ProgressBar.Progress);
        }


    }
}
