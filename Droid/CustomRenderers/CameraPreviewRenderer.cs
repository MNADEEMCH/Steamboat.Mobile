using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Views;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers.Camera;
using Java.Lang;
using Java.Util.Concurrent;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Exception = Java.Lang.Exception;
using Size = Android.Util.Size;
using Image = Android.Media.Image;
using System.IO;
using System.Linq;
using Android.Widget;
using Android.Util;
using Steamboat.Mobile.Droid.Helpers.Camera.Listeners;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Droid.Helpers.Camera.Callbacks;
using Steamboat.Mobile.Droid.Helpers.Files;
using Steamboat.Mobile.Services.Orientation;
using Steamboat.Mobile.Models.NavigationParameters;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, AutoFitTextureView>, ICamera2
    {

        #region Properties

        private static readonly string BACK_CAMERA_ID = "0";
        private static readonly string FRONT_CAMERA_ID = "1";

        private bool _isDisposing;
        private bool _isCameraOpened;
        private bool _isAutoFocusSupported;
        private bool _hasFrontCamera = false;
        private bool _isFlashSupported = false;
        private string _selectedCamera = BACK_CAMERA_ID;

        private Handler _backgroundHandler { get; set; }
        private SurfaceTextureListener _surfaceTextureListener;
        private CameraStateListener _cameraStateListener;
        private HandlerThread _backgroundThread;
        private Size _previewSize;
        private CaptureRequest.Builder _previewBuilder;
        private CameraCaptureListener _captureCallback { get; set; }

        private CameraCharacteristics _characteristics;

        private ImageAvailableListener _imageAvailableListener;
        private ImageReader _imageReader;
        private Java.IO.File _pictureFile;
        private bool _isTakingPicture;
        private int _jpegOrientation;
        private PictureSettings _takingPictureSettings;
        private Size _outputSize;

        private List<Surface> _surfaces;
        private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();

        private string _manufacturer;

        #endregion

        #region ICamera2 - Properties

        public Activity mActivity { get; set; }
        public AutoFitTextureView mAutoFitTextureView { get; set; }
        public CameraDevice mCameraDevice { get; set; }
        public Semaphore mCameraOpenCloseLock { get; set; }
        public CameraCaptureSession mCaptureSession { get; set; }
        public Camera2BasicState mState { get; set; } = Camera2BasicState.STATE_PREVIEW;

        #endregion

        public CameraPreviewRenderer(Context context) : base(context)
        {
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    mActivity = this.Context as Activity;
                    mAutoFitTextureView = new AutoFitTextureView(mActivity);
                    SetNativeControl(mAutoFitTextureView);

                    _surfaceTextureListener = new SurfaceTextureListener(this);
                    _cameraStateListener = new CameraStateListener(this);
                    mCameraOpenCloseLock = new Semaphore(1);
                    _captureCallback = new CameraCaptureListener(this);

                    _manufacturer = Android.OS.Build.Manufacturer;

                    e.NewElement.StartCamera = (() => { StartCamera(); });
                    e.NewElement.CloseCamera = (() => { CloseCamera(); });
                    e.NewElement.ToggleCamera = (async () => { await ToggleCamera(); });
                    e.NewElement.TakePicture = (TakePicture);
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (mState == Camera2BasicState.STATE_PREVIEW && _isAutoFocusSupported && _isCameraOpened)
                AutoFocusHandling();

            return base.OnTouchEvent(e);
        }

        #region Start-Close Camera

        private void StartCamera()
        {
            CheckAvailableCameras();

            StartBackgroundThread();
            if (mAutoFitTextureView.IsAvailable)
                OpenCamera(mAutoFitTextureView.Width, mAutoFitTextureView.Height);
            else
                mAutoFitTextureView.SurfaceTextureListener = _surfaceTextureListener;
        }

        private void CloseCamera()
        {
            try
            {
                mCameraOpenCloseLock.Acquire();

                if (mCaptureSession != null)
                {
                    mCaptureSession.Close();
                    mCaptureSession = null;
                }

                if (mCameraDevice != null)
                {
                    mCameraDevice.Close();
                    mCameraDevice = null;
                }

                if (_imageReader != null)
                {
                    _imageReader.Close();
                    _imageReader = null;
                }

                _isCameraOpened = false;
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera closing.");
            }
            finally
            {
                mCameraOpenCloseLock.Release();
            }
        }

        private void StartBackgroundThread()
        {
            _backgroundThread = new HandlerThread("CameraBackground");
            _backgroundThread.Start();
            _backgroundHandler = new Handler(_backgroundThread.Looper);
        }

        private void StopBackgroundThread()
        {

            try
            {
                _backgroundThread.QuitSafely();
                _backgroundThread.Join();
                _backgroundThread = null;
                _backgroundHandler = null;
            }
            catch (InterruptedException ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                CloseCamera();
                StopBackgroundThread();
                base.Dispose(true);
            }
        }

        #endregion

        #region Preview

        public void OpenCamera(int viewWidth, int viewHeight)
        {
            if (mActivity == null || mActivity.IsFinishing)
                return;

            try
            {
                if (_isCameraOpened)
                    CloseCamera();

                if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
                    throw new RuntimeException("Time out waiting to lock camera opening.");

                CameraManager manager = (CameraManager)mActivity.GetSystemService(Context.CameraService);

                _characteristics = manager.GetCameraCharacteristics(_selectedCamera);

                CheckAutoFocusSupported(_characteristics);

                CheckFlashSupported(_characteristics);

                SetPreviewSize(_characteristics, viewWidth, viewHeight);

                ConfigForTakingPicture();

                manager.OpenCamera(_selectedCamera, _cameraStateListener, null);

                _isCameraOpened = true;

                EvalCameraCharacteristicsAfterCameraIsOpened();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StartPreview()
        {
            if (null == mCameraDevice || !mAutoFitTextureView.IsAvailable || null == _previewSize)
                return;

            try
            {
                _surfaces = new List<Surface>();

                AddPreviewSurface();
                AddPictureSurface();

                LoadSurfaces();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void AddPreviewSurface()
        {
            SurfaceTexture texture = mAutoFitTextureView.SurfaceTexture;
            texture.SetDefaultBufferSize(_previewSize.Width, _previewSize.Height);
            _previewBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);

            var previewSurface = new Surface(texture);
            _previewBuilder.AddTarget(previewSurface);

            _surfaces.Add(previewSurface);
        }

        private void AddPictureSurface()
        {
            _surfaces.Add(_imageReader.Surface);
        }

        private void LoadSurfaces()
        {

            mCameraDevice.CreateCaptureSession(_surfaces, new PreviewCaptureStateCallback(this), _backgroundHandler);
        }

        public void UpdatePreview()
        {
            if (null == mCameraDevice)
                return;

            try
            {
                HandlerThread thread = new HandlerThread("CameraPreview");
                thread.Start();

                if (_isAutoFocusSupported)
                {
                    AutoFocusHandling();
                }
                else
                {
                    _previewBuilder.Set(CaptureRequest.ControlMode, new Java.Lang.Integer((int)ControlMode.Auto));
                    mCaptureSession.SetRepeatingRequest(_previewBuilder.Build(), _captureCallback, _backgroundHandler);
                }

            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }

        }

        #endregion

        #region Camera-Characteristics

        private void CheckAvailableCameras()
        {
            CameraManager manager = (CameraManager)mActivity.GetSystemService(Context.CameraService);

            var availableCameras = manager.GetCameraIdList();

            _hasFrontCamera = availableCameras.Length > 1;
        }

        private void CheckFlashSupported(CameraCharacteristics characteristics)
        {
            var available = characteristics.Get(CameraCharacteristics.FlashInfoAvailable);
            if (available == null)
                _isFlashSupported = false;
            else
                _isFlashSupported = (bool)available;

        }

        private void CheckAutoFocusSupported(CameraCharacteristics characteristics)
        {
            var afAvailableModes = (int[])characteristics.Get(CameraCharacteristics.ControlAfAvailableModes);
            if (afAvailableModes.Length == 0 || (afAvailableModes.Length == 1 && afAvailableModes[0] == (int)ControlAFMode.Off))
                _isAutoFocusSupported = false;
            else
                _isAutoFocusSupported = true;
        }

        public void EvalCameraCharacteristicsAfterCameraIsOpened()
        {

            var cameraCharacteristics = new CamCharacteristics();

            cameraCharacteristics.HasFrontCamera = _hasFrontCamera;
            cameraCharacteristics.IsBackCameraOpened = _selectedCamera.Equals(BACK_CAMERA_ID);
            cameraCharacteristics.IsFlashSupported = _isFlashSupported;
            cameraCharacteristics.IsAutoFocusSupported = _isAutoFocusSupported;
            cameraCharacteristics.IsFlashActivated = true;

            Element.OnCameraReady(cameraCharacteristics);
        }

        #endregion

        #region Size-Calculations

        private void SetPreviewSize(CameraCharacteristics characteristics, int viewWidth, int viewHeight)
        {
            StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

            if (viewWidth > viewHeight)
            {
                var aux = viewHeight;
                viewHeight = viewWidth;
                viewWidth = aux;
            }

            _previewSize = GetOptimalPreviewSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))), viewWidth, viewHeight);

            ConfigureTransform(viewWidth, viewHeight);
        }

        private Size GetOptimalPreviewSize(Size[] sizes, int w, int h)
        {
            double ASPECT_TOLERANCE = 0.1;
            double targetRatio = (double)h / w;

            if (sizes == null) return null;

            Size optimalSize = null;
            double minDiff = System.Double.MaxValue;

            int targetHeight = h;

            foreach (Size size in sizes)
            {
                double ratio = (double)size.Width / size.Height;
                if (System.Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE) continue;
                if (System.Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = System.Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = System.Double.MaxValue;
                foreach (Size size in sizes)
                {
                    if (System.Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = System.Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;
        }

        public void ConfigureTransform(int viewWidth, int viewHeight)
        {
            if (null == mActivity || null == _previewSize || null == mAutoFitTextureView)
                return;

            int rotation = (int)mActivity.WindowManager.DefaultDisplay.Rotation;
            var matrix = new Matrix();
            var viewRect = new RectF(0, 0, viewWidth, viewHeight);
            var bufferRect = new RectF(0, 0, _previewSize.Height, _previewSize.Width);
            float centerX = viewRect.CenterX();
            float centerY = viewRect.CenterY();

            var surfaceRotationIsVertical = (int)SurfaceOrientation.Rotation90 == rotation || (int)SurfaceOrientation.Rotation270 == rotation;
            var valueToCalcScale = surfaceRotationIsVertical ? viewHeight : viewWidth;
            var valueToRotate = surfaceRotationIsVertical ? 90 * (rotation - 2) : 0;


            bufferRect.Offset((centerX - bufferRect.CenterX()), (centerY - bufferRect.CenterY()));
            matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
            float scale = System.Math.Max(
                (float)valueToCalcScale / _previewSize.Height,
                (float)valueToCalcScale / _previewSize.Width);
            matrix.PostScale(scale, scale, centerX, centerY);
            matrix.PostRotate(valueToRotate, centerX, centerY);


            mAutoFitTextureView.SetTransform(matrix);
        }

        private Size GetSizeForPicture()
        {
            StreamConfigurationMap map = (StreamConfigurationMap)_characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            var sizes = map.GetOutputSizes((int)ImageFormatType.Jpeg);
            var sizesToChose = sizes.Where(s => s.Width >= _previewSize.Width && s.Height >= _previewSize.Height).OrderBy(s => s.Width).ToList();

            var pictureSize = sizes[sizes.Length - 1];//lower size

            if (sizesToChose.Count > 0)
            {
                pictureSize = sizesToChose[0];//the lower size that is bigger than preview
                var sizeSameRatio = sizesToChose.FirstOrDefault(s => SameRatio(s, _previewSize));
                if (sizeSameRatio != null)
                    pictureSize = sizeSameRatio;
            }

            return pictureSize;
        }

        #endregion

        #region Focus-Handling

        private void AutoFocusHandling()
        {

            mState = Camera2BasicState.STATE_WAITING_AF_AUTO;
            _previewBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
            mCaptureSession.SetRepeatingRequest(_previewBuilder.Build(), _captureCallback,
                       _backgroundHandler);
        }

        public void LockFocus()
        {
            mState = Camera2BasicState.STATE_WAITING_LOCK;
            _previewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);
            _previewBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
            mCaptureSession.Capture(_previewBuilder.Build(), _captureCallback,
                        _backgroundHandler);

            _previewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Idle);
            _previewBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
            mCaptureSession.SetRepeatingRequest(_previewBuilder.Build(), _captureCallback,
                    _backgroundHandler);

        }

        public void UnlockFocus()
        {
            mState = Camera2BasicState.STATE_PREVIEW;
        }

        public void GetRidOfNotFocusedLock()
        {
            UnlockFocus();
        }

        #endregion

        #region Camera-Features

        private async Task ToggleCamera()
        {
            if (_hasFrontCamera)
            {
                _selectedCamera = _selectedCamera.Equals(BACK_CAMERA_ID) ? FRONT_CAMERA_ID : BACK_CAMERA_ID;
                OpenCamera(mAutoFitTextureView.Width, mAutoFitTextureView.Height);
            }
        }

        #endregion

        #region Take-Picture

        private void ConfigForTakingPicture()
        {
            _outputSize = GetSizeForPicture();
            _imageReader = ImageReader.NewInstance(_outputSize.Width, _outputSize.Height, ImageFormatType.Jpeg, 2);
            _imageAvailableListener = new ImageAvailableListener(this);
            _imageReader.SetOnImageAvailableListener(_imageAvailableListener, _backgroundHandler);

        }

        private void TakePicture(PictureSettings pictureSettings)
        {
            try
            {
                if (!_isTakingPicture)
                {
                    _isTakingPicture = true;

                    _takingPictureSettings = pictureSettings;

                    var sensorOrientation = (int)_characteristics.Get(CameraCharacteristics.SensorOrientation);

                    var accelerometerReading = (int)AndroidDependencyContainer.Resolve<IDeviceOrientationService>().PhoneOrientation;

                    var phoneOrientation = accelerometerReading;

                    if (_selectedCamera == FRONT_CAMERA_ID)
                        phoneOrientation = phoneOrientation == 180 || phoneOrientation == 0 ? Math.Abs(phoneOrientation - 180) : phoneOrientation;

                    _jpegOrientation = (sensorOrientation + phoneOrientation - 90) % 360;

                    var stillCaptureBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
                    stillCaptureBuilder.AddTarget(_imageReader.Surface);

                    stillCaptureBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);

                    if (pictureSettings.EnableFlash)
                        stillCaptureBuilder.Set(CaptureRequest.FlashMode, (int)FlashMode.Torch);

                    stillCaptureBuilder.Set(CaptureRequest.JpegQuality, (sbyte)100);

                    mCaptureSession.StopRepeating();
                    PlayShutterClickSound();
                    mCaptureSession.Capture(stillCaptureBuilder.Build(), new CameraCaptureStillPictureSessionCallback(this), null);
                }


            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                _isTakingPicture = false;
            }
        }

        private void PlayShutterClickSound()
        {

            MediaActionSound sound = new MediaActionSound();
            sound.Play(MediaActionSoundType.ShutterClick);
        }

        public void OnPictureTaken(Image image)
        {

            _backgroundHandler.Post(() =>
            {
                var imageArray = ImageBytes.GetByteArray(image);

                //CUT IMAGE
                if (!SameRatio(_previewSize, _outputSize))
                {

                    Bitmap croppedBmp = null;

                    var bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);

                    var isPreviewPortrait = _jpegOrientation.Equals(90) || _jpegOrientation.Equals(270);
                    var previewHeight = isPreviewPortrait ? _previewSize.Width : _previewSize.Height;
                    var previewWidth = isPreviewPortrait ? _previewSize.Height : _previewSize.Width;

                    var isImageInPortrait = bitmap.Height >= bitmap.Width;
                    var widthCut = isImageInPortrait;

                    if (isImageInPortrait.Equals(isPreviewPortrait))
                    {
                        croppedBmp = ImageTransformations.CroppImage(bitmap, widthCut, previewWidth, previewHeight);
                    }
                    else
                    {
                        croppedBmp = ImageTransformations.CroppImage(bitmap, widthCut, previewHeight, previewWidth);
                    }

                    imageArray = ImageBytes.GetByteArray(croppedBmp);

                    bitmap.Dispose();
                    croppedBmp.Dispose();
                }

                //COMPRESSION
                if (_takingPictureSettings.ApplyCompression)
                {
                    imageArray = ImageCompression.CompressFile(imageArray, (int)(_takingPictureSettings.CompressionQuality * 100));
                }

                //ROTATION
                var name = "temp.jpg";
                _pictureFile = new Java.IO.File($"{System.IO.Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath)}/{name}");
                ImageSaver.SaveImage(imageArray, _pictureFile);
                ImageMetadata.SetPictureOrientation(_pictureFile.AbsolutePath, _jpegOrientation);
                imageArray = ImageBytes.GetByteArray(_pictureFile.AbsolutePath);

                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageArray));

                var pictureTakenOutput = new PhotoTakenParameter()
                {
                    ImageSource = imageSource,
                    Media = imageArray
                };

                _isTakingPicture = false;
                image.Close();
                _imageReader.Close();
                Element.OnPictureTaken(pictureTakenOutput);
            });
        }

        private bool SameRatio(Size size1, Size size2)
        {
            return ((double)size1.Width / size1.Height).Equals((double)size2.Width / size2.Height);
        }

        #endregion

    }
}