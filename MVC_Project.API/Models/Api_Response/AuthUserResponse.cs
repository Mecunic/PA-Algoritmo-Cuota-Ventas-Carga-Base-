using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MVC_Project.API.Models.Api_Response
{
    [DataContract]
    public class AuthUserResponse
    {
        [DataMember(Name = "api_key")]
        public string ApiKey { get; set; }
        [DataMember(Name = "api_key_expiration")]
        public long ApiKeyExpiration { get; set; }

        [DataMember(Name = "id")]
        public string Uuid { get; set; }
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "mobile_phone")]
        public string MobilePhone { get; set; }
    }
}