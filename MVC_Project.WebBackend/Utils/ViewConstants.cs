using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC_Project.WebBackend.App_Code;

namespace MVC_Project.WebBackend.Utils
{
    public class ViewConstants
    {
        public static readonly string AUTHENTICATED_USER_PROFILE = "8af092bb-a8e0-437f-a7eb-8c1ac1f233a2_USER_PROFILE";
        public static readonly string ESPANOL = "es-MX";
        public static readonly string INGLES = "en";
        public static bool IsMultiLanguage()
        {
            return LanguageMngr.IsMultiLanguage;
        }

        public static Language GetDefaultLanguage()
        {
            return LanguageMngr.AvailableLanguages.Where(AL => AL.LanguageCultureName.Equals(LanguageMngr.DefaultLanguageCulture)).FirstOrDefault();
        } 
    }
}