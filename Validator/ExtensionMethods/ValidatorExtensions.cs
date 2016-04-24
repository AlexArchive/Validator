using System;
using System.Text.RegularExpressions;

namespace Validator.ExtensionMethods
{
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Determine whether input is composed of one or more alphabetic characters only.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlpha(this string input) => 
            Validator.IsAlpha(input);

        /// <summary>
        /// Determine whether input is in lower case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLowercase(this string input) =>
            Validator.IsLowercase(input);

        /// <summary>
        /// Determine whether input is in upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUppercase(this string input) =>
            Validator.IsUppercase(input);

        /// <summary>
        /// Determine whether input represents a numeric number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(this string input) => 
            IsNumeric(input);

        /// <summary>
        /// Determine whether input represents a valid floating point number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFloat(this string input) => 
            IsFloat(input);

        /// <summary>
        /// Determine whether supplied input (once converted to an integer) is a multiple of @by.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="by">The divisor.</param>
        /// <returns></returns>
        public static bool IsDivisibleBy(this string input, int @by) =>
            Validator.IsDivisibleBy(input, @by);

        /// <summary>
        /// Determine whether length of input is between (inclusive) min and max.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns></returns>
        public static bool IsLength(this string input, int min, int max)
            => Validator.IsLength(input, min, max);

        /// <summary>
        /// Determine whether all characters within string are ASCII based.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAscii(this string input) =>
            Validator.IsAscii(input);

        /// <summary>
        /// Determiner whether the string contains one or more multibyte chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMultiByte(this string input) =>
            Validator.IsMultiByte(input);

        /// <summary>
        /// Determine whether the string contains any half-width chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHalfWidth(this string input) =>
            Validator.IsHalfWidth(input);

        public static bool IsFullWidth(this string input) =>
            Validator.IsFullWidth(input);

        /// <summary>
        /// Determine if the string contains any full-width chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVariableWidth(this string input) =>
            Validator.IsVariableWidth(input);

        /// <summary>
        /// Determine if the string contains any surrogate pairs chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSurrogatePair(this string input) =>
            Validator.IsSurrogatePair(input);

        /// <summary>
        /// Determine whether any strings in values, appear in the input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="values">Array of string values to search for.</param>
        /// <returns></returns>
        public static bool IsIn(this string input, string[] values) =>
            Validator.IsIn(input, values);

        /// <summary>
        /// Determine whether input is a valid IPv4 or IPv6 address.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid values are: IpVersion.Four and IpVersion.Six</param>
        /// <returns></returns>
        public static bool IsIp(this string input, IpVersion version) =>
            Validator.IsIp(input, version);

        /// <summary>
        /// Determine whether input matches a valid email address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input) =>
            Validator.IsEmail(input);

        /// <summary>
        /// Determine whether input matches a hexadecimal formatted string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHexadecimal(this string input) =>
            Validator.IsHexadecimal(input);

        /// <summary>
        /// Determine whether string is composed of alphanumeric characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphanumeric(this string input) =>
            Validator.IsAlphanumeric(input);

        /// <summary>
        /// Determine whether input matches a valid hexadecimal colour.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHexColor(this string input) =>
            Validator.IsHexColor(input);

        /// <summary>
        /// Determine whether two inputs are equal.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool Equals(this string input, string comparison) =>
            Validator.Equals(input, comparison);

        /// <summary>
        /// Determine whether input string matches a valid date format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDate(this string input) =>
            Validator.IsDate(input);

        /// <summary>
        /// Determine whether input string matches a valid date AND occurs after supplied date.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsAfter(this string input, DateTime date) =>
            Validator.IsAfter(input, date);

        /// <summary>
        /// Determine whether input string matches a valid date AND occurs before supplied date.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsBefore(this string input, DateTime date) =>
            Validator.IsBefore(input, date);

        /// <summary>
        /// Determine whether input is a valid JSON string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsJson(this string input) =>
            Validator.IsJson(input);

        /// <summary>
        /// Determine whether input is null.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNull(this string input) =>
            Validator.IsNull(input);

        /// <summary>
        /// Determine whether element appears in supplied input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Contains(this string input, string element) =>
            Validator.Contains(input, element);

        /// <summary>
        /// Determine whether input matches supplied regular expression pattern, with optional options.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">Pattern to match against.</param>
        /// <param name="options">Supplied options. Default is None.</param>
        /// <returns></returns>
        public static bool Matches(this string input, string pattern, RegexOptions options = RegexOptions.None) =>
            Validator.Matches(input, pattern, options);

        /// <summary>
        /// Determine whether input is a valid Mongo ID.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMongoId(this string input) =>
            Validator.IsMongoId(input);

        /// <summary>
        /// Determine whether length of supplied input is between min and max (inclusive).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns></returns>
        public static bool IsByteLength(this string input, int min, int max = int.MaxValue) =>
            Validator.IsByteLength(input, min, max);

        /// <summary>
        /// Indicates whether input is in correct format for a credit card number and is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsCreditCard(this string input) => 
            Validator.IsCreditCard(input);

        /// <summary>
        /// Indicates whether string is in base 64.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsBase64(this string input) => 
            Validator.IsBase64(input);

        /// <summary>
        /// Indicates whether input represents a fully qualified domain name.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requireTld"></param>
        /// <param name="allowUnderscore"></param>
        /// <param name="allowTrailingDot"></param>
        /// <returns></returns>
        public static bool IsFqdn(this string input, bool requireTld = true, 
            bool allowUnderscore = false, bool allowTrailingDot = false) => 
                Validator.IsFqdn(input, requireTld, allowUnderscore, allowTrailingDot);

        /// <summary>
        /// Indicates whether supplied input is either in ISBN-10 digit format or ISBN-13 digit format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid options are: IsbnVersion.Ten, IsbnVersion.Thirteen or IsbnVersion.Any</param>
        /// <returns></returns>
        /// IsbnVersion
        public static bool IsIsbn(this string input, IsbnVersion version = IsbnVersion.Any) => 
            Validator.IsIsbn(input, version);

        /// <summary>
        /// Determines whether the given phone number is a mobile phone number or not.
        /// </summary>
        /// <param name="input">The phone number to check.</param>
        /// <param name="locale">The locale to look in.</param>
        /// <returns>True if it is a mobile phone number, false otherwise.</returns>
        /// <remarks>
        /// Relies on locales that use specific blocks of numbers for mobile phone numbers.
        /// </remarks>
        public static bool IsMobilePhone(this string input, string locale) => 
            Validator.IsMobilePhone(input, locale);

        /// <summary>
        /// Determine whether input represents a numeric number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string input) => 
            Validator.IsNumeric(input);

        /// <summary>
        /// Determines whether the given string value, <paramref name="url"/>, qualifies as a Url.
        /// </summary>
        /// <param name="url">Value to check.</param>
        /// <param name="options">Options to consider.</param>
        /// <returns>True if a Url, false otherwise.</returns>
        public static bool IsUrl(this string input, Validator.UrlOptions options = null) => 
            Validator.IsUrl(input, options);

        /// <summary>
        /// Determine whether input is a valid Universal Unique Identifier.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid options are: UuidVersion.Any, UuidVersion.Three, UuidVersion.Four or UuidVersion.Five</param>
        /// <returns></returns>
        public static bool IsUuid(this string input, UuidVersion version = UuidVersion.Any) => 
            Validator.IsUuid(input, version);
    }
}
