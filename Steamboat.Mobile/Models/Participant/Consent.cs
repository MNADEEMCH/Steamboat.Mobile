using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Participant
{
    public class Consent
    {
        public int ParticpantID { get; set; }
        public int ConsentID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Affirmation { get; set; }
        public string Rejection { get; set; }
        public bool IsRequired { get; set; }
        public bool IsAffirmationRequired { get; set; }
        public bool IsRejectionAllowed { get; set; }
        //Internal Attributes
        public bool IsCompleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
    }
}
