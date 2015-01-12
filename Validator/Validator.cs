using System.Text.RegularExpressions;

namespace Validator
{
    public class Validator
    {
        public static bool IsAlpha(string input)
        {
            return Regex.IsMatch(input, "^[A-z]+$");
        }

        public static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, "^[0-9]+$");
        }


        public static bool IsLowercase(string input)
        {
            return input == input.ToLower();
        }
    }
}