using System.Numerics;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
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

        public static bool IsUppercase(string input)
        {
            return input == input.ToUpper();
        }

        public static bool IsInt(string input)
        {
            BigInteger value;
            return BigInteger.TryParse(input, out value);
        }
    }
}