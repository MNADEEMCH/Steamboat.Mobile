using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.Services.Participant
{
    public interface IParticipantService
    {
        Task<Status> GetStatus(string sessionId);
    }
}
