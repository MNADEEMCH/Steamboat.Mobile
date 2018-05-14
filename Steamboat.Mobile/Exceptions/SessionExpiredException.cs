using System;

namespace Steamboat.Mobile.Exceptions
{
    public class SessionExpiredException : ExceptionBase
    {
        public SessionExpiredException()
        {
        }

        public SessionExpiredException(string message)
        : base(message)
        {
        }

        public SessionExpiredException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
