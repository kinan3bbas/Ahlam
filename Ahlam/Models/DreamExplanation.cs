using Ahlam.Extras;
using Ahlam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ahlam.Models
{
    public class DreamExplanation:BasicModel
    {
        [Display(Name = "Text")]
        public String Explanation { get; set; }

        public int DreamId { get; set; }

        public Dream Dream { get; set; }

        public ApplicationUser Interpreter { get; set; }

        public String InterpreterId { get; set; }

        public String Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Explanation Date")]
        public DateTime ExplanationDate { get; set; }
    }
}