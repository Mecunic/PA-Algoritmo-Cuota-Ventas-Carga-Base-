using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC_Project.WebBackend.Utils
{
    public static class Validations
    {
        public static bool ContainsLetters(this string word)
        {
            return (ContainsCapitalLetter(word) || ContainsLowerCaseLetter(word));
        }

        public static bool ContainsNumbers(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[0-9]").Count > 0;
        }

        public static bool ContainsCapitalLetter(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[A-Z]").Count > 0;
        }

        public static bool ContainsLowerCaseLetter(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[a-z]").Count > 0;
        }

        public static bool ContainsCaractersSpecial(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[^0-9a-zA-Z]+").Count > 0;
        }
    }
}