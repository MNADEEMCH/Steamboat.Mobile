using System;
using Steamboat.Mobile.Models.Participant.Photojournaling;

namespace Steamboat.Mobile.Models.NavigationParameters
{
    public class MainViewModelInitParameter
    {
        public Type DispositionStepType { get; set;}
        public bool NavigatingToDispositionStep { get; set; }
        public int UnreadMessageCount { get; set; }
        public Nutrition NutritionPlan { get; set; }
    }
}
