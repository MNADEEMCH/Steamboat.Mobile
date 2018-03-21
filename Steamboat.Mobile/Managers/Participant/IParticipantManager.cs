﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant;
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
        Task SendSurvey(int groupID, List<ParticipantConsent> answers); 
    }
}
