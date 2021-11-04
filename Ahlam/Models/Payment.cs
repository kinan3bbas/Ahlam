using Ahlam.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ahlam.Models
{
    public class Payment:BasicModel
    {

        public double Amount { get; set; }
        public string Method { get; set; }

        public string Currency { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }




    }
}