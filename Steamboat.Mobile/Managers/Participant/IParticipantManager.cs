using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Messaging;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.Models.Participant.Survey;

namespace Steamboat.Mobile.Managers.Participant
{
    public interface IParticipantManager
    {
        Task<Status> GetStatus();
        Task<List<Consent>> GetConsents();
        Task<List<Consent>> SendConsents(List<Consent> completedConsents);
        Task<List<Event>> GetEvents();
        Task<Appointment> ConfirmEvent(int eventId, int eventTimeSlotId);
        Task<List<Event>> CancelEvent();
        Task<List<EventTime>> GetEventTimes(int eventId);
        Task<QuestionGroup> GetSurvey();
        Task<QuestionGroup> SendSurvey(int groupID, List<ParticipantConsent> answers);
        Task CompleteSurvey(int groupID, List<ParticipantConsent> answers);
        List<ParticipantConsent> GetSurveyResponses();
        Task<CoachMessages> GetAllMessages();
		Task<CoachMessages> GetNewMessages(string dateFrom);
		Task<CoachMessages.Message> SendMessage(string messageText);
        Task<PhotoResponse> UploadPhoto(byte[] media, string comment, string opinion);
        Task<List<Photograph>> GetPhotographs();
    }
}
