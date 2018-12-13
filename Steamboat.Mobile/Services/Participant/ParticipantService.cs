using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Messaging;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.Models.Participant.Survey;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.Participant
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRequestProvider _requestProvider;
		private readonly string ApiUrlBase;

		public ParticipantService(IRequestProvider requestProvider = null, ISettings settings = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
			var _settings = settings ?? DependencyContainer.Resolve<ISettings>();
			ApiUrlBase = _settings.BaseUrl + "participant/";
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

            return await _requestProvider.PostAsync<List<Consent>, CompletedConsents>(url, completedConsents, sessionId);
        }

        public async Task<List<EventTime>> GetEventTimes(int eventId, string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}/{1}", "event", eventId);

            return await _requestProvider.GetAsync<List<EventTime>>(url, sessionID);
        }

        public async Task<List<Event>> GetEvents(string sessionId){

            string url = string.Format(ApiUrlBase + "{0}", "event");
            return await _requestProvider.GetAsync<List<Event>>(url, sessionId);
        }

        public async Task<List<Event>> CancelEvent(string sessionId){
            string url = string.Format(ApiUrlBase + "{0}", "event/cancel");
            return await _requestProvider.PostAsync<List<Event>>(url, sessionId);
        }

        public async Task<Appointment> ConfirmEvent(int eventId,int eventTimeSlotId,string sessionId)
        {

            string url = string.Format(ApiUrlBase + "{0}/{1}/{2}", "event", eventId, eventTimeSlotId);
            return await _requestProvider.PostAsync<Appointment>(url, sessionId);
        }

        public async Task<SurveyRequest> GetSurvey(string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "survey");
            return await _requestProvider.GetAsync<SurveyRequest>(url, sessionId);
        }

        public async Task<SurveyRequest> PostSurvey(int groupID, SurveyResponse response, string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}/{1}", "survey", groupID);
            return await _requestProvider.PostAsync<SurveyRequest, SurveyResponse>(url, response, sessionID);
        }

        public async Task CompleteSurvey(string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}/{1}", "survey", "complete");
            await _requestProvider.PostAsync(url, sessionID:sessionID);
        }

        public async Task<CoachMessages> GetAllMessages(string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}", "messages");
            return await _requestProvider.GetAsync<CoachMessages>(url, sessionID:sessionID);
        }

		public async Task<CoachMessages> GetNewMessages(string dateFrom, string sessionID)
		{			
			string url = string.Format(ApiUrlBase + "{0}/{1}", "messages", dateFrom);
			return await _requestProvider.GetAsync<CoachMessages>(url, sessionID: sessionID);
		}

		public async Task<CoachMessages.Message> SendMessage(UserMessage messageText, string sessionID)
		{
			string url = string.Format(ApiUrlBase + "{0}", "message");
			return await _requestProvider.PostAsync<CoachMessages.Message,UserMessage>(url, messageText, sessionID: sessionID);
		}

        public async Task<PhotoResponse> UploadPhoto(byte[] media, string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}/{1}", "photograph", "upload");
            return await _requestProvider.PostImageAsync<PhotoResponse>(url, media, sessionID);
        }

        public async Task<PhotographResponse> GetPhotographs(string sessionID)
        {
            string url = string.Format(ApiUrlBase + "{0}", "photographs");
            return await _requestProvider.GetAsync<PhotographResponse>(url, sessionID: sessionID);
        }
    }
}
