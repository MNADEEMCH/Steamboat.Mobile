using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Models.Stepper
{
    public class StepperParam
    {
        public int Steps { get; set; }
        public int CurrentStep { get; set; }
        public int PreviousStep { get; set; }
    }
}
