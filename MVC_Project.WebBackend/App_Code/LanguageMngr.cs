using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.App_Code
{
    public class LanguageMngr
    {
        public static bool IsMultiLanguage = false;
        public static string DefaultLanguageCulture = "";

        public static List<Language> AvailableLanguages = new List<Language> {
            new Language {
                LanguageFullName = "English", LanguageCultureName = "en",
            },
            new Language {
                LanguageFullName = "Español", LanguageCultureName = "es-MX"
            }
        };
        public static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguages.Where(a => a.LanguageCultureName.Equals(lang)).FirstOrDefault() != null ? true : false;
        }
        public static string GetDefaultLanguage()
        {
            return (!DefaultLanguageCulture.Trim().Any()) ? AvailableLanguages[0].LanguageCultureName : AvailableLanguages.Where(AL => AL.LanguageCultureName.Equals(DefaultLanguageCulture)).FirstOrDefault().LanguageCultureName;
        }
        public static void SetDefaultLanguage()
        {
            SetLanguage(GetDefaultLanguage());
        }
        public static void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang)) lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie langCookie = new HttpCookie("culture", lang);
                langCookie.Expires = DateTime.Now.AddYears(1);
                langCookie.Secure = true;
                langCookie.HttpOnly = true;
                DefaultLanguageCulture = lang;
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch (Exception e)
            {
                //DONT HANDLE IF FAILS
            }
        }
    }
    public class Language
    {
        public string LanguageFullName
        {
            get;
            set;
        }
        public string LanguageCultureName
        {
            get;
            set;
        }
    }
}