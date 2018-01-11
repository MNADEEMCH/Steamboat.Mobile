using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.Participant
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRequestProvider _requestProvider;
        private const string ApiUrlBase = "https://dev.momentumhealth.co/participant/";

        public ParticipantService(IRequestProvider requestProvider = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
        }

        public async Task<Status> GetStatus(string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "dashboard");

            return await _requestProvider.GetAsync<Status>(url, sessionId);        
        }

        public async Task<List<Consent>> GetConsents(string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "consent");

            return await _requestProvider.GetAsync<List<Consent>>(url, sessionId);   
        }

        public async Task<List<Consent>> SendConsents(CompletedConsents completedConsents, string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "consent");

            return await _requestProvider.PostAsync<List<Consent>,CompletedConsents>(url, completedConsents, sessionId);
        }
    }
}
