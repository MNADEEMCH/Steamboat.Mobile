using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using Java.Util.Concurrent;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Steamboat.Mobile.Droid.Helpers.Camera;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Size = Android.Util.Size;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, AutoFitTextureView>, ICamera2
    {
        private SparseIntArray ORIENTATIONS = new SparseIntArray();
        int _camera = 0;
        string[] _camerasIds = null;
        private int DSI_height;
        private int DSI_width;
        private Size videoSize;
        private Size previewSize;
        private bool mCameraOpened;
        private MyCameraStateCallback stateListener;
        private MySurfaceTextureListener surfaceTextureListener;
        private HandlerThread backgroundThread;
        private CaptureRequest.Builder previewBuilder;
        private ImageReader mImageReader;
        public Java.IO.File mFile;
        private ImageAvailableListener mOnImageAvailableListener;

        public CameraDevice mCameraDevice { get; set; }
        public Semaphore mCameraOpenCloseLock { get; set; }
        public CameraCaptureSession mCaptureSession { get; set; }
        public Activity mActivity { get; set; }
        public AutoFitTextureView mTextureView { get; set; }
        public Handler mBackgroundHandler { get; set; }
        //public MediaRecorder mediaRecorder;

        public CameraPreviewRenderer(Context context) : base(context)
        {
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            if (Xamarin.Forms.DesignMode.IsDesignModeEnabled)
            {
                return;
            }

            if (e.OldElement != null) // Clear old element event
            {

            }

            if (e.NewElement != null)
            {
                e.NewElement.StartRecording = (() => { TakePicture(); });
                //e.NewElement.StopRecording = (() => { StopRecordingVideo(); });
                //e.NewElement.RetrySending = (() => { RetrySendingVideo(); });
                e.NewElement.SwapCamera = (() => { SwapCamera(); });
                e.NewElement.Dispose = (() => { Dispose(true); });

                if (Control == null)
                {
                    mActivity = this.Context as Activity;
                    SetNativeControl(new AutoFitTextureView(Context));
                }

                mTextureView = Control as AutoFitTextureView;
                surfaceTextureListener = new MySurfaceTextureListener(this);
                stateListener = new MyCameraStateCallback(this);
                mCameraOpenCloseLock = new Semaphore(1);
                mFile = new Java.IO.File("");
                mOnImageAvailableListener = new ImageAvailableListener(this, mFile);

                StartTheCamera();
            }

            base.OnElementChanged(e);
        }

        private void TakePicture()
        {
            try
            {
                if (null == mActivity || null == mCameraDevice)
                {
                    return;
                }
                var stillCaptureBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
                stillCaptureBuilder.AddTarget(mImageReader.Surface);

                stillCaptureBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
                int rotation = (int)mActivity.WindowManager.DefaultDisplay.Rotation;
                stillCaptureBuilder.Set(CaptureRequest.JpegOrientation, ORIENTATIONS.Get(rotation));

                mCaptureSession.StopRepeating();
                MediaActionSound sound = new MediaActionSound();
                sound.Play(MediaActionSoundType.ShutterClick);
                mCaptureSession.Capture(stillCaptureBuilder.Build(), new CameraCaptureStillPictureSessionCallback(this), null);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
            }
        }

        public void OnCaptureComplete(byte[] imageArray)
        {
            Element.OnPhotoTaken(imageArray);
        }

        private void StartTheCamera()
        {
            StartBackgroundThread();
            if (mTextureView.IsAvailable)
                OpenCamera(mTextureView.Width, mTextureView.Height);
            else
                mTextureView.SurfaceTextureListener = surfaceTextureListener;
        }

        public void ConfigureTransform(int viewWidth, int viewHeight)
        {
            if (null == mActivity || null == previewSize || null == mTextureView)
                return;

            int rotation = (int)mActivity.WindowManager.DefaultDisplay.Rotation;
            var matrix = new Matrix();
            var viewRect = new RectF(0, 0, viewWidth, viewHeight);
            var bufferRect = new RectF(0, 0, previewSize.Height, previewSize.Width);
            float centerX = viewRect.CenterX();
            float centerY = viewRect.CenterY();
            if ((int)SurfaceOrientation.Rotation90 == rotation || (int)SurfaceOrientation.Rotation270 == rotation)
            {
                bufferRect.Offset((centerX - bufferRect.CenterX()), (centerY - bufferRect.CenterY()));
                matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
                float scale = System.Math.Max(
                    (float)viewHeight / previewSize.Height,
                    (float)viewHeight / previewSize.Width);
                matrix.PostScale(scale, scale, centerX, centerY);
                matrix.PostRotate(90 * (rotation - 2), centerX, centerY);
            }
            mTextureView.SetTransform(matrix);
        }

        public async Task SwapCamera()
        {
            _camera = (_camera + 1) % _camerasIds.Length;
            OpenCamera(mTextureView.Width, mTextureView.Height);
        }

        public void OpenCamera(int width, int height)
        {
            if (null == mActivity || mActivity.IsFinishing)// || mCameraOpened
                return;

            if (mCameraOpened)
                CloseCamera();

            CameraManager manager = (CameraManager)mActivity.GetSystemService(Context.CameraService);
            try
            {
                if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
                    throw new RuntimeException("Time out waiting to lock camera opening.");


                if (_camerasIds == null)
                    _camerasIds = manager.GetCameraIdList();

                string cameraId = manager.GetCameraIdList()[_camera];
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

                //videoSize = ChooseVideoSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))));
                //previewSize = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))), width, height, videoSize);

                //ConfigureTransform(width, height);
                //mediaRecorder = new MediaRecorder();

                videoSize = ChooseVideoSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))));
                previewSize = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))), width, height, videoSize);
                ConfigureTransform(width, height);
                mImageReader = ImageReader.NewInstance(width, height, ImageFormatType.Jpeg, 2);
                mImageReader.SetOnImageAvailableListener(mOnImageAvailableListener, mBackgroundHandler);

                SetDisplayMetrics();
                SetAspectRatioTextureView(DSI_width, DSI_height);

                manager.OpenCamera(cameraId, stateListener, null);

                mCameraOpened = true;

            }
            catch (CameraAccessException)
            {
                Toast.MakeText(mActivity, "Cannot access the camera.", ToastLength.Short).Show();
                mActivity.Finish();
            }
            catch (InterruptedException)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.");
            }
        }

        public void StartPreview()
        {
            if (null == mCameraDevice || !mTextureView.IsAvailable || null == previewSize)
                return;

            try
            {
                //SetUpMediaRecorder();
                SurfaceTexture texture = mTextureView.SurfaceTexture;
                //Assert.IsNotNull(texture);
                texture.SetDefaultBufferSize(previewSize.Width, previewSize.Height);
                previewBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                var surfaces = new List<Surface>();
                var previewSurface = new Surface(texture);
                surfaces.Add(previewSurface);
                previewBuilder.AddTarget(previewSurface);

                surfaces.Add(mImageReader.Surface);

                //var recorderSurface = mediaRecorder.Surface;
                //surfaces.Add(recorderSurface);
                //previewBuilder.AddTarget(recorderSurface);

                mCameraDevice.CreateCaptureSession(surfaces, new PreviewCaptureStateCallback(this), mBackgroundHandler);

            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                //e.PrintStackTrace();
            }
        }

        private void SetUpMediaRecorder()
        {
            try
            {
                if (null == mActivity)
                    return;
                //mediaRecorder.SetAudioSource(AudioSource.Mic);
                //mediaRecorder.SetVideoSource(VideoSource.Surface);
                //mediaRecorder.SetProfile(CamcorderProfile.Get(CamcorderQuality.Low));
                ////ROCKO
                ////if I set quality I cant set SetOutputFormat,SetVideoEncoder,SetAudioEncoder
                //mediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);
                ////mediaRecorder.SetOutputFile(GetVideoFile(mActivity).AbsolutePath);
                //mediaRecorder.SetVideoEncodingBitRate(10000000);
                //mediaRecorder.SetVideoFrameRate(30);
                //mediaRecorder.SetVideoSize(videoSize.Width, videoSize.Height);
                //mediaRecorder.SetVideoEncoder(VideoEncoder.H264);
                //mediaRecorder.SetAudioEncoder(AudioEncoder.Aac);

                //int rotation = (int)mActivity.WindowManager.DefaultDisplay.Rotation;
                //int orientation = ORIENTATIONS.Get(rotation);
                //mediaRecorder.SetOrientationHint(orientation);
                //mediaRecorder.Prepare();
            }
            catch (Java.Lang.Exception ex)
            {

            }
        }

        public void UpdatePreview()
        {
            if (null == mCameraDevice)
                return;

            try
            {
                SetUpCaptureRequestBuilder(previewBuilder);
                HandlerThread thread = new HandlerThread("CameraPreview");
                thread.Start();
                mCaptureSession.SetRepeatingRequest(previewBuilder.Build(), null, mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        private void SetUpCaptureRequestBuilder(CaptureRequest.Builder builder)
        {
            builder.Set(CaptureRequest.ControlMode, new Java.Lang.Integer((int)ControlMode.Auto));

        }

        private void CloseCamera()
        {
            try
            {
                mCameraOpenCloseLock.Acquire();
                if (null != mCameraDevice)
                {
                    mCameraDevice.Close();
                    mCameraDevice = null;
                }
                //if (null != mediaRecorder)
                //{
                //    mediaRecorder.Release();
                //    mediaRecorder = null;
                //}
                mCameraOpened = false;
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

        private Size ChooseVideoSize(Size[] choices)
        {
            foreach (Size size in choices)
            {
                if (size.Width == size.Height * 4 / 3 && size.Width <= 1000)
                    return size;
            }
            return choices[choices.Length - 1];
        }

        private Size ChooseOptimalSize(Size[] choices, int width, int height, Size aspectRatio)
        {
            var bigEnough = new List<Size>();
            int w = aspectRatio.Width;
            int h = aspectRatio.Height;
            foreach (Size option in choices)
            {
                if (option.Height == option.Width * h / w &&
                    option.Width >= width && option.Height >= height)
                    bigEnough.Add(option);
            }

            if (bigEnough.Count > 0)
                return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
            else
            {
                return choices[0];
            }
        }

        private void SetDisplayMetrics()
        {
            DisplayMetrics displayMetrics = Resources.DisplayMetrics;
            DSI_height = displayMetrics.HeightPixels;
            DSI_width = displayMetrics.WidthPixels;
        }

        private void SetAspectRatioTextureView(int ResolutionWidth, int ResolutionHeight)
        {
            if (ResolutionWidth > ResolutionHeight)
            {
                int newWidth = DSI_width;
                int newHeight = ((DSI_width * ResolutionWidth) / ResolutionHeight);
                UpdateTextureViewSize(newWidth, newHeight);

            }
            else
            {
                int newWidth = DSI_width;
                int newHeight = ((DSI_width * ResolutionHeight) / ResolutionWidth);
                UpdateTextureViewSize(newWidth, newHeight);
            }

        }

        private void UpdateTextureViewSize(int viewWidth, int viewHeight)
        {
            mTextureView.LayoutParameters = new FrameLayout.LayoutParams(viewWidth, viewHeight);
        }

        private void StartBackgroundThread()
        {
            backgroundThread = new HandlerThread("CameraBackground");
            backgroundThread.Start();
            mBackgroundHandler = new Handler(backgroundThread.Looper);
        }
    }
}
