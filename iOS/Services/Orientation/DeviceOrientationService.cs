using System;
using CoreMotion;
using Foundation;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Services.Orientation;

namespace Steamboat.Mobile.iOS.Services.Orientation
{
    public class DeviceOrientationService : IDeviceOrientationService
    {
        private CMMotionManager _motionManager;

        public DeviceOrientation PhoneOrientation { get; set; } = DeviceOrientation.UNKNOWN;

        public void RegisterListener()
        {
            _motionManager = new CMMotionManager();
            _motionManager.AccelerometerUpdateInterval = 0.5;
            _motionManager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue,
            delegate (CMAccelerometerData data, NSError error)
            {
                if (data.Acceleration.X >= 0.75)
                {
                    PhoneOrientation = DeviceOrientation.LSLEFT;
                }
                else if (data.Acceleration.X <= -0.75)
                {
                    PhoneOrientation = DeviceOrientation.LSRIGHT;
                }
                else if (data.Acceleration.Y <= -0.75)
                {
                    PhoneOrientation = DeviceOrientation.PORTRAIT;
                }
                else if (data.Acceleration.Y >= 0.75)
                {
                    PhoneOrientation = DeviceOrientation.UPSIDEDOWN;
                }
            });
        }

        public void UnregisterListener()
        {
            if (_motionManager != null)
            {
                PhoneOrientation = DeviceOrientation.UNKNOWN;
                _motionManager.StopAccelerometerUpdates();
                _motionManager = null;
            }
        }
    }
}
