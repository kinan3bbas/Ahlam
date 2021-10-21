using Ahlam.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ahlam.Models
{
    public class ServicePath:BasicModel
    {

        [Required]
        [Display(Name = "Number of interpreters")]
        public int NumberOfInterpreters { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Enabled")]
        public bool Enabled { get; set; }


    }
}