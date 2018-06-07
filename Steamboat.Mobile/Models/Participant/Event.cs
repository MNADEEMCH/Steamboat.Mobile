using System;
namespace Steamboat.Mobile.Models.Participant
{
    public class Event
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public string FullAddress { get; set; }
        public double Distance { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
