using System.Text.RegularExpressions;

namespace Validator
{
    partial class Validator
    {
        private const string AllVersionsRegex = "^[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}$";
        private const string V3Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-3[0-9A-F]{3}-[0-9A-F]{4}-[0-9A-F]{12}$";
        private const string V4Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-4[0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$";
        private const string V5Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-5[0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$";

        public static bool IsUuid(string input)
        {
            return Matches(input, AllVersionsRegex, RegexOptions.IgnoreCase);
        }

        public static bool IsUuid(int version, string input)
        {
            if (version == 3)
            {
                return Matches(input, V3Regex, RegexOptions.IgnoreCase);
            }
            if (version == 4)
            {
                return Matches(input, V4Regex, RegexOptions.IgnoreCase);
            }
            if (version == 5)
            {
                return Matches(input, V5Regex, RegexOptions.IgnoreCase);
            }
            return false;
        }
    }
}