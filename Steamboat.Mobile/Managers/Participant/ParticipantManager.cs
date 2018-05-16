using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Survey;
using Steamboat.Mobile.Services.Participant;

namespace Steamboat.Mobile.Managers.Participant
{
    public class ParticipantManager : ManagerBase, IParticipantManager
    {
        private IParticipantService _participantService;
        private IAccountManager _accountManager;

        public List<ParticipantConsent> SurveyResponses { get; set; }

        public ParticipantManager(IParticipantService participantService = null,
                                  IAccountManager accountManager = null)
        {
            _participantService = participantService ?? DependencyContainer.Resolve<IParticipantService>();
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
        }

        public async Task<Status> GetStatus()
        {
            return await TryExecute<Status>(async () =>
            {
                var status = await _participantService.GetStatus(App.SessionID);
                return status;

            });
        }

        public async Task<List<Consent>> GetConsents()
        {
            return await TryExecute<List<Consent>>(async () =>
            {
                var consents = await _participantService.GetConsents(App.SessionID);
                consents.Sort((x, y) => x.ConsentID.CompareTo(y.ConsentID));
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
                return consents;

            });
        }

        public async Task<List<Event>> GetEvents()
        {
            return await TryExecute<List<Event>>(async () =>
            {
              
                List<Event> events = await _participantService.GetEvents(App.SessionID);
                events.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                return events;
            });
        }

        public async Task<Appointment> ConfirmEvent(int eventId, int eventTimeSlotId)
        {
            return await TryExecute<Appointment>(async () =>
            {
                return await _participantService.ConfirmEvent(eventId, eventTimeSlotId, App.SessionID);
            
            });
        }

        public async Task<List<Event>> CancelEvent()
        {
            return await TryExecute<List<Event>>(async () =>
            {
                return await _participantService.CancelEvent(App.SessionID);
            
            });
        }

        public async Task<List<EventTime>> GetEventTimes(int eventId)
        {
            return await TryExecute<List<EventTime>>(async () =>
            {
                var eventTimes = await _participantService.GetEventTimes(eventId, App.SessionID);
                eventTimes.Sort((x, y) => x.Start.CompareTo(y.Start));
                return eventTimes;

            });
        }

        public async Task<QuestionGroup> GetSurvey()
        {
            return await TryExecute<QuestionGroup>(async () =>
            {
                var survey = await _participantService.GetSurvey(App.SessionID);
                SurveyResponses = survey.Responses;
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
            
            });
        }

        public List<ParticipantConsent> GetSurveyResponses()
        {
            return SurveyResponses ?? new List<ParticipantConsent>();
        }
    }
}
