using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Participant.Photojournaling
{
    public class PhotographResponse
    {
        public List<Photograph> Photographs { get; set; }
    }

    public class Photograph
    {
        public int ID { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string ParticipantComment { get; set; }
        public string ParticipantOpinionRating { get; set; }
        public string ReviewerComment { get; set; }
        public string ReviewerOpinionRating { get; set; }
        public DateTime SubmittedTimestamp { get; set; }
        public DateTime ReviewedTimestamp { get; set; }

        public bool ReviewPending { get; set; }
        public bool Acknowledged { get; set; }
    }          
}
