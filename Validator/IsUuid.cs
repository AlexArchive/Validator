using System;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        private const string AllVersionsRegex = "^[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}$";
        private const string V3Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-3[0-9A-F]{3}-[0-9A-F]{4}-[0-9A-F]{12}$";
        private const string V4Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-4[0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$";
        private const string V5Regex = "^[0-9A-F]{8}-[0-9A-F]{4}-5[0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$";

        /// <summary>
        /// Determine whether input is a valid Universal Unique Identifier.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid options are: UuidVersion.Any, UuidVersion.Three, UuidVersion.Four or UuidVersion.Five</param>
        /// <returns></returns>
        public static bool IsUuid(string input, UuidVersion version = UuidVersion.Any)
        {
            switch (version)
            {
                case UuidVersion.Three:
                    return Matches(input, V3Regex, RegexOptions.IgnoreCase);
                case UuidVersion.Four:
                    return Matches(input, V4Regex, RegexOptions.IgnoreCase);
                case UuidVersion.Five:
                    return Matches(input, V5Regex, RegexOptions.IgnoreCase);
                case UuidVersion.Any:
                    return Matches(input, AllVersionsRegex, RegexOptions.IgnoreCase);
                default:
                    throw new ArgumentOutOfRangeException(nameof(version), $"Uuid version {version} is not supported.");
            }
        }
    }
}