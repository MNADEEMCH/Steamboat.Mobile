using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Participant
{
    public class CompletedConsents
    {
        public List<ConsentResponse> ParticipantConsents { get; set; }
    }

    public class ConsentResponse
    {
        public int ParticpantID { get; set; }
        public int ConsentID { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
    }
}
