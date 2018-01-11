using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Services.Participant;

namespace Steamboat.Mobile.Managers.Participant
{
    public class ParticipantManager : IParticipantManager
    {
        private IParticipantService _participantService;

        public ParticipantManager(IParticipantService participantService = null)
        {
            _participantService = participantService ?? DependencyContainer.Resolve<IParticipantService>();
        }

        public async Task<Status> GetStatus()
        {
            try
            {
                var status = await _participantService.GetStatus(App.SessionID);
                return status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<List<Consent>> GetConsents()
        {
            try
            {
                var consents = await _participantService.GetConsents(App.SessionID);
                consents.Sort((x,y) => x.ConsentID.CompareTo(y.ConsentID));
                return consents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<List<Consent>> SendConsents(List<Consent> completedConsents)
        {
            try
            {
                var completed = new CompletedConsents();
                completed.ParticipantConsents = completedConsents.Select(x => new ConsentResponse
                { 
                    ConsentID = x.ConsentID,
                    ParticpantID = x.ParticpantID,
                    IsAccepted = x.IsAccepted,
                    IsRejected = x.IsRejected
                }).ToList();

                var consents = await _participantService.SendConsents(completed ,App.SessionID);
                return consents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }
    }
}
