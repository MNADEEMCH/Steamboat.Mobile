using System;
namespace Steamboat.Mobile.Models.User
{
    public class CurrentUser : EntityBase
    {
        public string Email { get; set; }
        public string AvatarUrl { get; set; } 

        public CurrentUser()
        {
        }
    }
}
