using System;
using System.Linq;
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

        public static bool IsFloat(string input)
        {
            float value;
            return float.TryParse(input, out value);
        }

        public static bool IsDivisibleBy(string input, int @by)
        {
            int value;
            if (!int.TryParse(input, out value))
            {
                return false;
            }
            return value % @by == 0;
        }

        public static bool IsLength(string input, int min, int max)
        {
            if (input.Length < min)
            {
                return false;
            }

            if (input.Length > max)
            {
                return false;
            }

            return true;
        }

        public static bool IsAscii(string input)
        {
            return input.Select(c => (int) c)
                .All(c => c <= 127);
        }
    }
}