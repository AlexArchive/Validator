using System.Collections.Generic;
using System.Linq;

namespace Validator
{
    public partial class Validator
    {
        private const char PaddingCharacter = '=';
        private static readonly HashSet<char> Base64Characters = new HashSet<char>() 
        { 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', '+', '/', 
        };

        /// <summary>
        /// Indicates whether string is in base 64.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsBase64(string input)
        {
            input = SanitizeInput(input);

            //Base64 values must be a multiple of 4 characters.
            if (input.Length == 0 || input.Length%4 != 0)
            {
                return false;
            }
            
            //Padding must be 0, 1 or 2 '=' characters.
            var valueWithoutPadding = input.TrimEnd(PaddingCharacter);
            if (input.Length - valueWithoutPadding.Length > 2)
            {
                return false;
            }

            //If the given input contains a input not present in
            //the hashset it cannot be a valid Base64 string.
            if (valueWithoutPadding.All(c => Base64Characters.Contains(c)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove any occurrences of carriage returns and newlines, before forcing all letters to lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string SanitizeInput(string value) => 
            value.Replace("\r", string.Empty).Replace("\n", string.Empty).ToLower();
    }
}