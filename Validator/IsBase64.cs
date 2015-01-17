using System.Collections.Generic;
using System.Linq;

namespace Validator
{
    public partial class Validator
    {
        private const char paddingCharacter = '=';
        private static readonly HashSet<char> base64Characters = new HashSet<char>() 
        { 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', '+', '/', 
        };

        public static bool IsBase64(string input)
        {
            input = SanetizeInput(input);

            //Base64 values must be a multiple of 4 characters.
            if (input.Length == 0 || input.Length % 4 != 0)
                return false;

            //Padding must be 0, 1 or 2 '=' characters.
            string valueWithoutPadding = input.TrimEnd(paddingCharacter);
            if (input.Length - valueWithoutPadding.Length > 2)
                return false;

            //If the given input contains a input not present in
            //the hashset it cannot be a valid Base64 string.
            if (valueWithoutPadding.All(c => base64Characters.Contains(c) != false))
                return true;

            return false;
        }

        private static string SanetizeInput(string value)
        {
            return value.Replace("\r", string.Empty).Replace("\n", string.Empty).ToLower();
        }
    }
}