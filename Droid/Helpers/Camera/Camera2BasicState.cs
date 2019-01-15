using System;
namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public enum Camera2BasicState : int
    {
        // Camera state: Showing camera preview.
        STATE_PREVIEW = 0,

        // Camera state: Waiting for the auto focus to be auto.
        STATE_WAITING_AF_AUTO = 1,

        // Camera state: Waiting for the focus to be locked.
        STATE_WAITING_LOCK = 2,

    }
}
