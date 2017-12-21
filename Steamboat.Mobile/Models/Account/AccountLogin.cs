using System;
namespace Steamboat.Mobile.Models.Account
{
    public class AccountLogin
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceLocalID { get; set; }
    }
}
