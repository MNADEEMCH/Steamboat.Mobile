using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace Steamboat.Mobile.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public Task LogErrorAsync(Exception exception, IDictionary<string, string> context = null)
        {
            Crashes.TrackError(exception, context);
            return Task.FromResult(false);
        }
    }
}
