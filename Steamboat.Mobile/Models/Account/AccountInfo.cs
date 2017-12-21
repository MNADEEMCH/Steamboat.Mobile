using System;
namespace Steamboat.Mobile.Models.Account
{
    public class AccountInfo
    {
        public AuthenticatedAccount AuthenticatedAccount { get; set; }
    }

    public class AuthenticatedAccount
    {
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string AuthenticatedTimestamp { get; set; }
        public string Culture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Session { get; set; }
    }
}
