using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.Logger
{
    public interface ILoggerService
    {
        Task LogErrorAsync(Exception exception, IDictionary<string, string> context = null);
    }
}
