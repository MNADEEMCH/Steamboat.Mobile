using System;
using Android.Hardware;
using Android.Runtime;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Services.Orientation;

namespace Steamboat.Mobile.Droid.Services.Orientation
{
    public class DeviceOrientationService : Java.Lang.Object, ISensorEventListener2, IDeviceOrientationService
    {
        private SensorManager _sensorManager;
        private Sensor _accelerometer;
        public DeviceOrientation PhoneOrientation { get; set; } = DeviceOrientation.UNKNOWN;

        public void RegisterListener()
        {
            if (_sensorManager == null || _accelerometer == null)
            {
                _sensorManager = (SensorManager)MainActivity.Context.GetSystemService(Android.Content.Context.SensorService);
                _accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            }
            _sensorManager.RegisterListener(this, _accelerometer, SensorDelay.Normal);
        }

        public void UnregisterListener()
        {
            _sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnFlushCompleted(Sensor sensor)
        {

        }

        public static readonly int UPSIDE_DOWN = 3;
        public static readonly int LANDSCAPE_RIGHT = 4;
        public static readonly int PORTRAIT = 1;
        public static readonly int LANDSCAPE_LEFT = 2;
        public int mOrientationDeg; //last rotation in degrees
        private static readonly int _DATA_X = 0;
        private static readonly int _DATA_Y = 1;
        private static readonly int _DATA_Z = 2;
        private int ORIENTATION_UNKNOWN = -1;

        public void OnSensorChanged(SensorEvent e)
        {
            var tempOrientRounded = PhoneOrientation;
            var values = e.Values;
            int orientation = ORIENTATION_UNKNOWN;
            float X = -values[_DATA_X];
            float Y = -values[_DATA_Y];
            float Z = -values[_DATA_Z];
            float magnitude = X * X + Y * Y;
            // Don't trust the angle if the magnitude is small compared to the y value
            if (magnitude * 4 >= Z * Z)
            {
                float OneEightyOverPi = 57.29577957855f;
                float angle = (float)Math.Atan2(-Y, X) * OneEightyOverPi;
                orientation = 90 - (int)Math.Round(angle);
                // normalize to 0 - 359 range
                while (orientation >= 360)
                {
                    orientation -= 360;
                }
                while (orientation < 0)
                {
                    orientation += 360;
                }
            }
            //^^ thanks to google for that code
            //now we must figure out which orientation based on the degrees
            if (orientation != mOrientationDeg)
            {
                mOrientationDeg = orientation;
                //figure out actual orientation
                if (orientation == -1)
                {//basically flat

                }
                else if (orientation > 225 && orientation <= 315)
                {//round to 0
                    tempOrientRounded = DeviceOrientation.LSRIGHT;//lsright
                }
                else if (orientation <= 45 || orientation > 315)
                {//round to 90
                    tempOrientRounded = DeviceOrientation.PORTRAIT;//portrait
                }
                else if (orientation > 45 && orientation <= 135)
                {//round to 180
                    tempOrientRounded = DeviceOrientation.LSLEFT;//lsleft
                }
                else if (orientation > 135 && orientation <= 225)
                {//round to 270
                    tempOrientRounded = DeviceOrientation.UPSIDEDOWN; //upside down
                }
            }

            if (PhoneOrientation != tempOrientRounded)
            {
                //Orientation changed, handle the change here
                PhoneOrientation = tempOrientRounded;

            }
        }
    }
}
