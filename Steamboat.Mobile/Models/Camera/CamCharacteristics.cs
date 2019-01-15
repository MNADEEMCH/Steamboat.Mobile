using System;
namespace Steamboat.Mobile.Models.Camera
{
    public class CamCharacteristics
    {
        public bool HasFrontCamera { get; set; }
        public bool IsBackCameraOpened { get; set; }
        public bool IsFlashSupported { get; set; }
        public bool IsAutoFocusSupported { get; set; }
        public bool IsFlashActivated { get; set; }
    }
}
