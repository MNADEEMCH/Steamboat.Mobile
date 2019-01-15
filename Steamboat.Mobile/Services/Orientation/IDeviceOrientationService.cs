using System;
using Steamboat.Mobile.Models.Camera;

namespace Steamboat.Mobile.Services.Orientation
{
    public interface IDeviceOrientationService
    {
        void RegisterListener();
        void UnregisterListener();

        DeviceOrientation PhoneOrientation { get; set; }
    }
}
