using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Messaging;
using Steamboat.Mobile.Models.Participant.Survey;

namespace Steamboat.Mobile.Services.Participant
{
    public interface IParticipantService
    {
        Task<Status> GetStatus(string sessionId);
        Task<List<Consent>> GetConsents(string sessionId);
        Task<List<Consent>> SendConsents(CompletedConsents completedConsents, string sessionID);
        Task<List<Event>> GetEvents(string sessionId);
        Task<Appointment> ConfirmEvent(int eventId, int eventTimeSlotId, string sessionId);
        Task<List<Event>> CancelEvent(string sessionId);
        Task<List<EventTime>> GetEventTimes(int eventId, string sessionID);
        Task<SurveyRequest> GetSurvey(string sessionID);
        Task<SurveyRequest> PostSurvey(int groupID, SurveyResponse response, string sessionID);
        Task CompleteSurvey(string sessionID);
        Task<CoachMessages> GetAllMessages(string sessionID);
		Task<CoachMessages> GetNewMessages(string dateFrom, string sessionID);
		Task<CoachMessages.Message> SendMessage(UserMessage messageText, string sessionID);
    }
}
