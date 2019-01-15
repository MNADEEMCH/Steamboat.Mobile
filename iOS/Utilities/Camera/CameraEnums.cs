using System;
namespace Steamboat.Mobile.iOS.Utilities.Camera
{
    public enum CameraState
    {
        Ready, AccessDenied, NoDeviceFound, NotDetermined
    }

    public enum CameraOutputMode
    {
        StillImage, VideoWithMic, VideoOnly
    }

    public enum CameraDevice
    {
        Back, Front
    }

    public enum CameraFlashMode : int
    {
        Off = 0, On = 1, Auto = 2
    }

    public enum CameraOutputQuality : int
    {
        Low = 0, Medium = 1, High = 2
    }
}
