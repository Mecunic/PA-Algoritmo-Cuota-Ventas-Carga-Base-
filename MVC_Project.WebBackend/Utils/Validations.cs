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
            return word != null && Regex.Matches(word.Trim(), @"[A-ZÁ-ÚÑ]").Count > 0;
        }

        public static bool ContainsLowerCaseLetter(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[a-zá-úñ]").Count > 0;
        }

        public static bool ContainsCaractersSpecial(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[^0-9a-zA-ZÁ-Úá-ú\s]+").Count > 0;
        }

        public static bool ContainsSpaces(this string word)
        {
            return word != null && Regex.Matches(word.Trim(), @"[\s*]").Count >= 0;
        }

        public static bool ContainsOnlyLetters(this string word)
        {
            return word != null && word.ContainsSpaces() && word.ContainsLetters() && !word.ContainsNumbers() && !word.ContainsCaractersSpecial();
        }

        public static bool ContainsOnlyNumbers(this string word)
        {
            return word != null && word.ContainsSpaces() && !word.ContainsLetters() && word.ContainsNumbers() && !word.ContainsCaractersSpecial();
        }

        public static bool ContainsOnlyLetterAndNumbers(this string word)
        {
            return word != null && word.ContainsSpaces() && (word.ContainsLetters() || word.ContainsNumbers()) && !word.ContainsCaractersSpecial();
        }
    }
}