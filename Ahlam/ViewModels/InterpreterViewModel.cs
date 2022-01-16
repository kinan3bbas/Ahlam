using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahlam.ViewModels
{
    public class InterpreterViewModel
    {
        public string name { get; set; }
        public string username { get; set; }

        public int numberOfDoneDreams { get; set; }

        public int numberOfActiveDreams { get; set; }

        public string password { get; set; }

        public double avgInterpretionTime { get; set; }

        public double availableBalance { get; set; }

        public double sentBalance { get; set; }
    }
}