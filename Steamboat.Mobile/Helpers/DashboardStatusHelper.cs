using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile.Helpers
{
    public class DashboardStatusHelper
    {
        public readonly static Dictionary<string, Type> StatusViewModelsDictionary = new Dictionary<string, Type>
        {
            { "Survey" , typeof(InterviewViewModel)}
        };

        public static Type GetViewModelForStatus(NextStepContent status)
        {
            if (!string.IsNullOrEmpty(status.Type))
            {
                Type vmType;
                StatusViewModelsDictionary.TryGetValue(status.Type,out vmType);
                return vmType;
            }
            else
                throw new Exception("Status page not available");
        }
    }
}
