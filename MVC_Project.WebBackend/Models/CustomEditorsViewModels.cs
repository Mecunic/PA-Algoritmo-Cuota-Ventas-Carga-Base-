using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class DateRangeViewModel
    {
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
    }
}