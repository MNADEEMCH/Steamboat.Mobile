using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Messaging;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.Models.Participant.Survey;
using Steamboat.Mobile.Services.Participant;

namespace Steamboat.Mobile.Managers.Participant
{
	public class ParticipantManager : ManagerBase, IParticipantManager
	{
		private IApplicationManager _applicationManager;
		private IParticipantService _participantService;
		private IAccountManager _accountManager;

		public List<ParticipantConsent> SurveyResponses { get; set; }

		public ParticipantManager(IApplicationManager applicationManager = null,
								  IParticipantService participantService = null,
								  IAccountManager accountManager = null)
		{
			_applicationManager = applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
			_participantService = participantService ?? DependencyContainer.Resolve<IParticipantService>();
			_accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
		}

		public async Task<Status> GetStatus()
		{
			return await TryExecute<Status>(async () =>
			{
				var status = await _participantService.GetStatus(App.SessionID);
				_applicationManager.RestartTimer();
				return status;
			});
		}

		public async Task<List<Consent>> GetConsents()
		{
			return await TryExecute<List<Consent>>(async () =>
			{
				var consents = await _participantService.GetConsents(App.SessionID);
				consents.Sort((x, y) => x.ConsentID.CompareTo(y.ConsentID));
				_applicationManager.RestartTimer();
				return consents;

			});
		}

		public async Task<List<Consent>> SendConsents(List<Consent> completedConsents)
		{
			return await TryExecute<List<Consent>>(async () =>
			{
				var completed = new CompletedConsents();
				completed.ParticipantConsents = completedConsents.Select(x => new ConsentResponse
				{
					ConsentID = x.ConsentID,
					ParticpantID = x.ParticpantID,
					IsAccepted = x.IsAccepted,
					IsRejected = x.IsRejected
				}).ToList();

				var consents = await _participantService.SendConsents(completed, App.SessionID);
				_applicationManager.RestartTimer();
				return consents;
			});
		}

		public async Task<List<Event>> GetEvents()
		{
			return await TryExecute<List<Event>>(async () =>
			{
				List<Event> events = await _participantService.GetEvents(App.SessionID);
				events.Sort((x, y) => x.Distance.CompareTo(y.Distance));
				_applicationManager.RestartTimer();
				return events;
			});
		}

		public async Task<Appointment> ConfirmEvent(int eventId, int eventTimeSlotId)
		{
			return await TryExecute<Appointment>(async () =>
			{
				_applicationManager.RestartTimer();
				return await _participantService.ConfirmEvent(eventId, eventTimeSlotId, App.SessionID);
			});
		}

		public async Task<List<Event>> CancelEvent()
		{
			return await TryExecute<List<Event>>(async () =>
			{
				_applicationManager.RestartTimer();
				return await _participantService.CancelEvent(App.SessionID);
			});
		}

		public async Task<List<EventTime>> GetEventTimes(int eventId)
		{
			return await TryExecute<List<EventTime>>(async () =>
			{
				var eventTimes = await _participantService.GetEventTimes(eventId, App.SessionID);
				eventTimes.Sort((x, y) => x.Start.CompareTo(y.Start));
				_applicationManager.RestartTimer();
				return eventTimes;

			});
		}

		public async Task<QuestionGroup> GetSurvey()
		{
			return await TryExecute<QuestionGroup>(async () =>
			{
				var survey = await _participantService.GetSurvey(App.SessionID);
				SurveyResponses = survey.Responses;
				_applicationManager.RestartTimer();
				return survey.QuestionGroup;
			});
		}

		public async Task<QuestionGroup> SendSurvey(int groupID, List<ParticipantConsent> answers)
		{
			return await TryExecute<QuestionGroup>(async () =>
			{
				var response = new SurveyResponse();
				response.Responses = answers;

				var ret = await _participantService.PostSurvey(groupID, response, App.SessionID);
				_applicationManager.RestartTimer();
				return ret.QuestionGroup;
			});
		}

		public async Task CompleteSurvey(int groupID, List<ParticipantConsent> answers)
		{
			await TryExecute(async () =>
			{
				var response = new SurveyResponse();
				response.Responses = answers;
				await _participantService.PostSurvey(groupID, response, App.SessionID);
				await _participantService.CompleteSurvey(App.SessionID);
				_applicationManager.RestartTimer();
			});
		}

		public List<ParticipantConsent> GetSurveyResponses()
		{
			_applicationManager.RestartTimer();
			return SurveyResponses ?? new List<ParticipantConsent>();
		}

		public async Task<CoachMessages> GetAllMessages()
		{
			return await TryExecute<CoachMessages>(async () =>
			{
				_applicationManager.RestartTimer();
				return await _participantService.GetAllMessages(App.SessionID);
			});
		}

		public async Task<CoachMessages> GetNewMessages(string dateFrom)
		{
			return await TryExecute<CoachMessages>(async () =>
			{
				_applicationManager.RestartTimer();
				return await _participantService.GetNewMessages(dateFrom, App.SessionID);
			});
		}

		public async Task<CoachMessages.Message> SendMessage(string messageText)
		{
			return await TryExecute<CoachMessages.Message>(async () =>
			{
				var text = new UserMessage() { Text = messageText };
				_applicationManager.RestartTimer();
				return await _participantService.SendMessage(text, App.SessionID);
			});
		}

        public async Task<PhotoResponse> UploadPhoto(byte[] media)
        {
            return await TryExecute<PhotoResponse>(async () =>
            {
                return await _participantService.UploadPhoto(media, App.SessionID);
            });
        }
    }
}
