using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.API.Models.Api_Request
{
    public class DocumentRequest
    {
        public List<DocumentObject> Documents { set; get; }
    }

    public class DocumentObject
    {
        public string Name { set; get; }
        public string Type { set; get; }

        public string URL { set; get; }

        public string Base64 { set; get; }

    }
}