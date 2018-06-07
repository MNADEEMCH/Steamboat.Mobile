using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Application;

namespace Steamboat.Mobile.Services.App
{
    public interface IApplicationService
    {
        Task<ApplicationDeviceInfo> SendToken(ApplicationDeviceInfo applicationDeviceInfo, string sessionId);
    }
}
