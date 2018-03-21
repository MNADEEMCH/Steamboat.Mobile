using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.Survey;
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
                consents.Sort((x, y) => x.ConsentID.CompareTo(y.ConsentID));
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

                var consents = await _participantService.SendConsents(completed, App.SessionID);
                return consents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<List<Event>> GetEvents()
        {
            try
            {
                List<Event> events = await _participantService.GetEvents(App.SessionID);
                events.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                return events;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting events: {ex}");
                throw ex;
            }
        }

        public async Task<Appointment> ConfirmEvent(int eventId, int eventTimeSlotId)
        {
            try
            {
                return await _participantService.ConfirmEvent(eventId,eventTimeSlotId,App.SessionID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error at confirm events: {ex}");
                throw ex;
            }
        }

        public async Task<List<Event>> CancelEvent()
        {
            try
            {
                return await _participantService.CancelEvent(App.SessionID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error at confirm events: {ex}");
                throw ex;
            }
        }

        public async Task<List<EventTime>> GetEventTimes(int eventId)
        {
            try
            {
                var eventTimes = await _participantService.GetEventTimes(eventId, App.SessionID);
                eventTimes.Sort((x, y) => x.Start.CompareTo(y.Start));
                return eventTimes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<QuestionGroup> GetSurvey()
        {
            try
            {
                var survey = await _participantService.GetSurvey(App.SessionID);
                return survey.QuestionGroup;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting the survey: {ex}");
                throw ex;
            }
        }

        public async Task SendSurvey(int groupID, List<ParticipantConsent> answers)
        {
            try
            {
                var response = new SurveyResponse();
                response.Responses = answers;
                await _participantService.PostSurvey(groupID, response, App.SessionID);
                await _participantService.CompleteSurvey(App.SessionID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting the survey: {ex}");
                throw ex;
            }
        }
    }
}
