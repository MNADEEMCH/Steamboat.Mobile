using System;
using System.Diagnostics;
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
    }
}
