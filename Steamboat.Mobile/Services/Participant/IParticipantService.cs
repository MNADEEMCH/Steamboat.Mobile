using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.Services.Participant
{
    public interface IParticipantService
    {
        Task<Status> GetStatus(string sessionId);

        Task<List<Consent>> GetConsents(string sessionId);

        Task<List<Consent>> SendConsents(CompletedConsents completedConsents, string sessionID);
    }
}
