using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahlam.Bindings
{
    public class DreamBinding
    {
       

        public double Amount { get; set; }
        public string Method { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public int ServicePathId { get; set; }

    }
}