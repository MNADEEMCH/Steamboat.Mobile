using System;
using System.Collections.Generic;
using Steamboat.Mobile.Exceptions;

namespace Steamboat.Mobile.Helpers
{
    public class ErrorCodesHelper
    {
        public static readonly Dictionary<int, Type> ErrorDictionary = new Dictionary<int, Type> 
        {
            {1004, typeof(PasswordExpiredException)},
            {6009, typeof(SessionExpiredException)},
            {8, typeof(SessionExpiredException)}
        };
    }
}
