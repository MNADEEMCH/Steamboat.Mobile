using System;
namespace Steamboat.Mobile.Models.Participant.Photojournaling
{
    public class Nutrition
    {
        public bool IsEnabled { get; set; }
        public bool IsActivePhase { get; set; }
        public string Strategy { get; set; }
        public int RequiredPhotographCount { get; set; }
    }
}
