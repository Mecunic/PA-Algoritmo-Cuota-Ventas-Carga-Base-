using MVC_Project.WebBackend.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class MessageView
    {
        public TypeMessageView type { get; set; }
        public String description { get; set; }

    }
}