using Ahlam.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ahlam.Models
{
    public class Dream:BasicModel
    {
        [Display(Name = "Status")]
        public String Status { get; set; } //Active, Done,Deleted


        [Display(Name = "Description")]
        public string Description { get; set; }


        public string ServiceProviderId { get; set; }


        public ApplicationUser ServiceProvider { get; set; }

        public int ServicePathId { get; set; }

        public ServicePath ServicePath { get; set; }

        [Display(Name = "Explanation")]
        public string Explanation { get; set; }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Explanation Date")]
        public DateTime? ExplanationDate { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        public int PaymentId { get; set; }

        public Payment Payment { get; set; }
    }
}