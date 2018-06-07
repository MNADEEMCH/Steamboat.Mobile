using System;
namespace Steamboat.Mobile.Exceptions
{
    public class PasswordExpiredException : ExceptionBase
    {
        public PasswordExpiredException()
        {
        }

        public PasswordExpiredException(string message)
        : base(message)
        {
        }

        public PasswordExpiredException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
