using System;
using System.Linq;
using AVFoundation;
using Foundation;
using Steamboat.Mobile.iOS.Utilities.Files;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Services.Orientation;
using UIKit;

namespace Steamboat.Mobile.iOS.Utilities.Camera
{
    public class CameraManager : NSObject
    {
        #region Properties

        private bool _isCameraSetup;

        private CameraDevice _cameraDevice = CameraDevice.Back;
        private CameraDevice CameraDevice
        {
            get
            {
                return _cameraDevice;
            }
            set
            {
                if (_cameraDevice != value)
                {
                    _cameraDevice = value;

                    if (_isCameraSetup)
                    {
                        UpdateCameraDevice();
                    }

                }
            }
        }

        private bool _isFlashSupported
        {
            get
            {
                var device = _currentDevice;

                return device.IsFlashModeSupported(AVCaptureFlashMode.Off) && device.IsFlashModeSupported(AVCaptureFlashMode.On);//The off may not be necesary
            }
        }

        private CameraOutputMode _outputMode = CameraOutputMode.StillImage;
        public CameraOutputMode OutputMode
        {
            get
            {
                return _outputMode;
            }
            set
            {
                if (_isCameraSetup)
                {
                    SetupOutputMode(value, _outputMode);
                    _outputMode = value;
                }
            }
        }

        private bool _hasFrontCamera
        {
            get
            {
                var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
                return devices.Any(x => x.Position == AVCaptureDevicePosition.Front);
            }
        }
        private AVCaptureDevice _frontCameraDevice
        {
            get
            {
                var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
                return devices.FirstOrDefault(x => x.Position == AVCaptureDevicePosition.Front);
            }
        }
        private AVCaptureDevice _backCameraDevice
        {
            get
            {
                var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
                return devices.FirstOrDefault(x => x.Position == AVCaptureDevicePosition.Back);
            }
        }

        private bool _IsCameraObservingDeviceOrientation;
        private NSObject _orientationObserver;

        private AVCaptureDevice _currentDevice
        {
            get
            {
                var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

                var devicePosition = CameraDevice == CameraDevice.Back ? AVCaptureDevicePosition.Back : AVCaptureDevicePosition.Front;

                var device = devices.Where(x => x.Position == devicePosition).FirstOrDefault();

                return device;
            }
        }

        AVCaptureSession _captureSession;
        private AVCaptureVideoPreviewLayer _previewLayer;
        private UIView _embeddingView;

        private AVCaptureStillImageOutput _stillImageOutput;

        public EventHandler<CamCharacteristics> CameraReadyEventHandler;
        public EventHandler<bool> FlashToggledEventHandler;

        #endregion

        public CameraState AddPreviewLayerToView(UIView view, CameraOutputMode cameraOutputMode = CameraOutputMode.StillImage, Action completion = null)
        {
            if (CanLoadCamera())
            {
                if (_isCameraSetup)
                {
                    AfterCameraSetup(view, cameraOutputMode, completion);
                }
                else
                {
                    SetupCamera(
                        () =>
                        {
                            AfterCameraSetup(view, cameraOutputMode, completion);
                        }
                    );
                }
            }

            return CheckIfCameraIsAvailable();
        }

        private bool CanLoadCamera()
        {
            var currentCameraState = CheckIfCameraIsAvailable();

            return currentCameraState == CameraState.Ready ||
                   currentCameraState == CameraState.NotDetermined;
        }

        private CameraState CheckIfCameraIsAvailable()
        {
            var deviceHasCamera =
                   UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Rear)
                || UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Front);

            if (deviceHasCamera)
            {
                var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
                var userAgreedToUseIt = authorizationStatus == AVAuthorizationStatus.Authorized;
                if (userAgreedToUseIt)
                {
                    return CameraState.Ready;
                }
                else if (authorizationStatus == AVAuthorizationStatus.NotDetermined)
                {
                    return CameraState.NotDetermined;
                }
                else
                {
                    return CameraState.AccessDenied;
                }
            }
            else
            {
                return CameraState.NoDeviceFound;
            }
        }

        #region Setup-Camera

        private void SetupCamera(Action completion)
        {
            _captureSession = new AVCaptureSession();

            _captureSession.BeginConfiguration();

            _captureSession.SessionPreset = AVCaptureSession.PresetHigh;
            UpdateCameraDevice();
            SetupPreviewLayer();

            _captureSession.CommitConfiguration();

            _captureSession.StartRunning();

            StartFollowingDeviceOrientation();

            _isCameraSetup = true;

            OrientationChanged();

            completion();
        }

        void UpdateCameraDevice()
        {

            _captureSession.BeginConfiguration();

            RemoveSessionInputs();

            AddInputsToSession();

            _captureSession.CommitConfiguration();

            CameraReadyEventHandler?.Invoke(this, GetCameraCharacteristics());
        }

        private void RemoveSessionInputs()
        {
            var inputs = _captureSession.Inputs;

            foreach (var input in inputs)
            {
                if (input != null)
                {
                    var deviceInput = input as AVCaptureDeviceInput;
                    if (deviceInput.Device == _backCameraDevice && CameraDevice == CameraDevice.Front)
                    {
                        _captureSession.RemoveInput(deviceInput);
                        break;
                    }
                    else if (deviceInput.Device == _frontCameraDevice && CameraDevice == CameraDevice.Back)
                    {
                        _captureSession.RemoveInput(deviceInput);
                        break;
                    }
                }
            }
        }

        private void AddInputsToSession()
        {
            var inputs = _captureSession.Inputs;

            switch (CameraDevice)
            {
                case CameraDevice.Front:
                    if (_hasFrontCamera)
                    {
                        var validFrontDevice = GetDeviceInputFromDevice(_frontCameraDevice);
                        if (validFrontDevice != null)
                        {
                            if (!inputs.Contains(validFrontDevice))
                            {
                                _captureSession.AddInput(validFrontDevice);
                            }
                        }
                    }
                    break;
                case CameraDevice.Back:
                    var validBackDevice = GetDeviceInputFromDevice(_backCameraDevice);
                    if (validBackDevice != null)
                    {
                        if (!inputs.Contains(validBackDevice))
                        {
                            _captureSession.AddInput(validBackDevice);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private AVCaptureDeviceInput GetDeviceInputFromDevice(AVCaptureDevice device)
        {
            try
            {
                NSError err;
                return new AVCaptureDeviceInput(device, out err);
            }
            catch
            {
                return null;
            }
        }

        private void SetupPreviewLayer()
        {
            _previewLayer = new AVCaptureVideoPreviewLayer(_captureSession);
            _previewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
        }

        private void AfterCameraSetup(UIView view, CameraOutputMode cameraOutputMode, Action completion = null)
        {
            AddPreviewLayerToView(view);
            OutputMode = cameraOutputMode;

            if (completion != null)
                completion();
        }

        private void AddPreviewLayerToView(UIView view)
        {
            if (_embeddingView != null)
            {
                if (_previewLayer != null)
                {
                    _previewLayer.RemoveFromSuperLayer();
                }
            }

            _embeddingView = view;

            this._previewLayer.Frame = view.Layer.Bounds;
            view.ClipsToBounds = false;
            view.Layer.AddSublayer(this._previewLayer);
        }

        private void SetupOutputMode(CameraOutputMode newCameraOutputMode, CameraOutputMode? oldCameraOutputMode)
        {
            _captureSession.BeginConfiguration();

            // remove current setting
            if (oldCameraOutputMode != null)
            {

                switch (oldCameraOutputMode)
                {
                    case CameraOutputMode.StillImage:
                        if (_stillImageOutput != null)
                        {
                            _captureSession.RemoveOutput(_stillImageOutput);
                        }
                        break;
                    default:
                        break;
                }
            }

            // configure new devices
            switch (newCameraOutputMode)
            {
                case CameraOutputMode.StillImage:
                    _stillImageOutput = _stillImageOutput ?? new AVCaptureStillImageOutput();
                    _captureSession.AddOutput(_stillImageOutput);
                    break;
                default:
                    break;
            }

            _captureSession.CommitConfiguration();
            UpdateCameraQualityMode(CameraOutputQuality.High);
            OrientationChanged();
        }

        private void UpdateCameraQualityMode(CameraOutputQuality newCameraOutputQuality)
        {
            if (_captureSession != null)
            {
                var sessionPreset = AVCaptureSession.PresetLow;

                switch (newCameraOutputQuality)
                {
                    case CameraOutputQuality.Low:
                        sessionPreset = AVCaptureSession.PresetLow;
                        break;
                    case CameraOutputQuality.Medium:
                        sessionPreset = AVCaptureSession.PresetMedium;
                        break;
                    case CameraOutputQuality.High:
                        if (OutputMode == CameraOutputMode.StillImage)
                            sessionPreset = AVCaptureSession.PresetPhoto;
                        else
                            sessionPreset = AVCaptureSession.PresetHigh;
                        break;
                    default:
                        break;
                }

                if (_captureSession.CanSetSessionPreset(sessionPreset))
                {
                    _captureSession.BeginConfiguration();
                    _captureSession.SessionPreset = sessionPreset;
                    _captureSession.CommitConfiguration();
                }
            }
        }

        #endregion

        #region Close-Camera

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CloseCamera();
        }

        private void CloseCamera()
        {
            StopAndRemoveCaptureSession();
            StopFollowingDeviceOrientation();
        }

        private void StopAndRemoveCaptureSession()
        {
            StopCaptureSession();
            _cameraDevice = CameraDevice.Back;
            _isCameraSetup = false;
            _previewLayer = null;
            _captureSession = null;
            _IsCameraObservingDeviceOrientation = false;
            _embeddingView = null;
        }

        private void StopCaptureSession()
        {
            if (_captureSession != null)
                _captureSession.StopRunning();
        }

        #endregion

        #region Camera-Characteristics

        private CamCharacteristics GetCameraCharacteristics()
        {

            var cameraCharacteristics = new CamCharacteristics();

            cameraCharacteristics.HasFrontCamera = _hasFrontCamera;
            cameraCharacteristics.IsBackCameraOpened = CameraDevice == CameraDevice.Back;
            cameraCharacteristics.IsFlashSupported = _isFlashSupported;
            cameraCharacteristics.IsAutoFocusSupported = true;
            cameraCharacteristics.IsFlashActivated = _currentDevice.FlashMode.Equals(AVCaptureFlashMode.On);

            return cameraCharacteristics;
        }

        #endregion

        #region Orientation

        private void StartFollowingDeviceOrientation()
        {
            if (!_IsCameraObservingDeviceOrientation)
            {
                _IsCameraObservingDeviceOrientation = true;
                _orientationObserver = UIDevice.Notifications.ObserveOrientationDidChange(OnOrientationChanged);
                //NSNotificationCenter.DefaultCenter.AddObserver(new NSString(), _orientationObserver);

            }
        }

        private void StopFollowingDeviceOrientation()
        {
            if (_IsCameraObservingDeviceOrientation)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_orientationObserver);
                _IsCameraObservingDeviceOrientation = false;
            }
        }

        private void OrientationChanged()
        {

            var validPreviewLayerConnection = _previewLayer.Connection;
            if (validPreviewLayerConnection != null)
            {
                if (validPreviewLayerConnection.SupportsVideoOrientation)
                {
                    validPreviewLayerConnection.VideoOrientation = CurrentVideoOrientation();
                }
            }

            if (_embeddingView != null)
                _previewLayer.Frame = _embeddingView.Bounds;
        }

        private void OnOrientationChanged(object sender, NSNotificationEventArgs e)
        {
            if (_IsCameraObservingDeviceOrientation)
            {
                OrientationChanged();
            }
        }

        private AVCaptureVideoOrientation CurrentVideoOrientation()
        {
            switch (UIDevice.CurrentDevice.Orientation)
            {
                case UIDeviceOrientation.LandscapeLeft:
                    return AVCaptureVideoOrientation.LandscapeRight;
                case UIDeviceOrientation.LandscapeRight:
                    return AVCaptureVideoOrientation.LandscapeLeft;
                //case UIDeviceOrientation.PortraitUpsideDown:
                //    return AVCaptureVideoOrientation.PortraitUpsideDown;
                default:
                    return AVCaptureVideoOrientation.Portrait;
            }
        }

        #endregion

        #region Camera-Features

        public bool ToggleCamera()
        {
            if (_hasFrontCamera)
            {
                if (CameraDevice == CameraDevice.Back)
                    CameraDevice = CameraDevice.Front;
                else
                    CameraDevice = CameraDevice.Back;

                return true;
            }

            return false;
        }

        public void ToggleFlash(bool turnOnFlash)
        {       
            if (_isFlashSupported)
            {
                if (turnOnFlash)
                    TurnFlashOn();
                else
                    TurnFlashOff();
            }

            FlashToggledEventHandler?.Invoke(this, !turnOnFlash);
        }

        private void TurnFlashOn()
        {
            if (_isFlashSupported)
            {
                SetFlashMode(AVCaptureFlashMode.On);
            }
        }

        private void TurnFlashOff()
        {
            if (_isFlashSupported)
            {
                SetFlashMode(AVCaptureFlashMode.Off);
            }
        }

        private bool SetFlashMode(AVCaptureFlashMode flashMode)
        {

            var device = _currentDevice;

            if (device.IsFlashModeSupported(flashMode))
            {
                _captureSession.BeginConfiguration();

                try
                {
                    NSError err;
                    device.LockForConfiguration(out err);
                    device.FlashMode = flashMode;
                    device.UnlockForConfiguration();
                }
                catch
                {
                    return false;
                }

                _captureSession.CommitConfiguration();

                return true;
            }

            return false;
        }

        private bool SetTorchMode(AVCaptureTorchMode torchMode)
        {
            var device = _currentDevice;

            if (_isFlashSupported)
            {
                _captureSession.BeginConfiguration();

                try
                {
                    NSError err;
                    device.LockForConfiguration(out err);
                    device.TorchMode = torchMode;
                    device.UnlockForConfiguration();
                }
                catch
                {
                    return false;
                }

                _captureSession.CommitConfiguration();

                return true;
            }

            return false;
        }

        #endregion

        #region Take-Picture

        public void TakePicture(Action<UIImage, NSError> completion)
        {
            if (!_isCameraSetup)
                return;


            var orientation = IOSDependencyContainer.Resolve<IDeviceOrientationService>().PhoneOrientation;

            if (CameraDevice == CameraDevice.Front && (orientation.Equals(DeviceOrientation.LSLEFT) || orientation.Equals(DeviceOrientation.LSRIGHT)))
                orientation = orientation.Equals(DeviceOrientation.LSLEFT) ? DeviceOrientation.LSRIGHT : DeviceOrientation.LSLEFT;

            GetStillImageOutput().CaptureStillImageAsynchronously(
            _stillImageOutput.ConnectionFromMediaType(AVMediaType.Video),
            (imageDataSampleBuffer, error) =>
            {
                var imageData = AVCaptureStillImageOutput.JpegStillToNSData(imageDataSampleBuffer);
                var image = new UIImage(imageData);

                //ROTATION

                image = ImageTransformations.RotateImage(image, (int)orientation);

                //CUT IMAGE
                var viewWidth = this._previewLayer.Frame.Width;
                var viewHeight = this._previewLayer.Frame.Height;
                var viewRatio = viewHeight > viewWidth ? viewHeight / viewWidth : viewWidth / viewHeight;

                var imageHeight = (int)image.Size.Height;
                var imageWidth = (int)image.Size.Width;


                if (orientation.Equals(DeviceOrientation.LSLEFT) || orientation.Equals(DeviceOrientation.LSRIGHT))
                {
                    var cropedImageHeight = (int)(imageWidth / viewRatio);
                    var heightSideToCrop = (imageHeight - cropedImageHeight) / 2;
                    image = ImageTransformations.CropImage(image, 0, heightSideToCrop, imageWidth, cropedImageHeight);
                }
                else
                {
                    var cropedImageWidth = (int)(imageHeight / viewRatio);
                    var widthSideToCrop = (imageWidth - cropedImageWidth) / 2;
                    image = ImageTransformations.CropImage(image, widthSideToCrop, 0, cropedImageWidth, imageHeight);
                }


                //LEAVE THIS ORDER
                CameraReadyEventHandler?.Invoke(this, GetCameraCharacteristics());
                completion(image, error);

            });


        }

        private AVCaptureStillImageOutput GetStillImageOutput()
        {
            var shouldReinitializeStillImageOutput = (_stillImageOutput == null);
            if (!shouldReinitializeStillImageOutput)
            {
                var connection = _stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
                if (connection != null)
                {
                    shouldReinitializeStillImageOutput = shouldReinitializeStillImageOutput || !connection.Active;
                }
            }
            if (shouldReinitializeStillImageOutput)
            {
                _stillImageOutput = new AVCaptureStillImageOutput();

                _captureSession.BeginConfiguration();
                _captureSession.AddOutput(_stillImageOutput);
                _captureSession.CommitConfiguration();
            }

            return _stillImageOutput;
        }

        #endregion
    }
}
