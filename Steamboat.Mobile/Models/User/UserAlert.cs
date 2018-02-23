
namespace Steamboat.Mobile.Models.User
{
    public class UserAlert:EntityBase
    {
        public string UserName{ get; set; }

        public int AlertId { get; set; } 

        public UserAlert(){
            
        }

        public UserAlert(string username){
            UserName = username;
        }

    }
}
