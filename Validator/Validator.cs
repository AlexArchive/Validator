using System.Text.RegularExpressions;

namespace Validator
{
    public class Validator
    {
        public static bool IsAlpha(string input)
        {
            return Regex.IsMatch(input, "^[A-z]+$");
        }
    }
}