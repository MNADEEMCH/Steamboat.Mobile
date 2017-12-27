using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.Managers.Participant
{
    public interface IParticipantManager
    {
        Task<Status> GetStatus();
    }
}
