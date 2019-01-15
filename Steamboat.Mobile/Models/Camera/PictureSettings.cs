using System;
namespace Steamboat.Mobile.Models.Camera
{
    public class PictureSettings
    {
        public bool ApplyCompression { get; set; } = true;
        public double CompressionQuality { get; set; } = 0.8;
        public bool EnableFlash { get; set; } = true;
    }
}
