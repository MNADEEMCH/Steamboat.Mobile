using System;

namespace Steamboat.Mobile.Exceptions
{
    public class ServiceException : ExceptionBase
    {
        public ServiceException()
        {
        }

        public ServiceException(string message)
        : base(message)
        {
        }

        public ServiceException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
