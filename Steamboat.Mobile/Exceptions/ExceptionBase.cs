using System;
namespace Steamboat.Mobile.Exceptions
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase()
        {
        }

        public ExceptionBase(string message)
        : base(message)
        {
        }

        public ExceptionBase(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
