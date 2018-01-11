using System;
namespace Steamboat.Mobile.Models.Account
{
    public class AccountInfo
    {
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public DateTime AuthenticatedTimestamp { get; set; }
        public string Culture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Session { get; set; }
        public bool IsPasswordExpired { get; set; }
        public bool AreConsentsAccepted { get; set; }
    }
}
