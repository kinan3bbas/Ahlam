using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahlam.ViewModels
{
    public class DreamsViewModel
    {
        public string interpreterName { get; set; }

        public String dream { get; set; }

        public DateTime createtionDate { get; set; }

        public DateTime interpretationTime { get; set; }

        public Double interpretationPeriod { get; set; }
    }
}