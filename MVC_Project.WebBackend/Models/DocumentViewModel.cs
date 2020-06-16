using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }

        public string URL { get; set; }

        public string Username { get; set; }

        public string DocumentType { get; set; }

        public string CreationDate { get; set; }

        [UIHint("DateRange")]
        public DateRangeViewModel FilterDate { get; set; }
    }
}