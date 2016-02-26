using System;
using System.Text.RegularExpressions;

namespace Validator.ExtensionMethods
{
    public static class ValidatorExtensions
    {
        public static bool IsAlpha(this string input) => 
            Validator.IsAlpha(input);

        public static bool IsLowercase(this string input) =>
            Validator.IsLowercase(input);

        public static bool IsUppercase(this string input) =>
            Validator.IsUppercase(input);

        public static bool IsInt(this string input) => 
            IsNumeric(input);

        public static bool IsFloat(this string input) => 
            IsFloat(input);

        public static bool IsDivisibleBy(this string input, int @by) =>
            Validator.IsDivisibleBy(input, @by);

        public static bool IsLength(this string input, int min, int max)
            => Validator.IsLength(input, min, max);

        public static bool IsAscii(this string input) =>
            Validator.IsAscii(input);

        public static bool IsMultiByte(this string input) =>
            Validator.IsMultiByte(input);

        public static bool IsHalfWidth(this string input) =>
            Validator.IsHalfWidth(input);

        public static bool IsFullWidth(this string input) =>
            Validator.IsFullWidth(input);

        public static bool IsVariableWidth(this string input) =>
            Validator.IsVariableWidth(input);

        public static bool IsSurrogatePair(this string input) =>
            Validator.IsSurrogatePair(input);

        public static bool IsIn(this string input, string[] values) =>
            Validator.IsIn(input, values);

        public static bool IsIp(this string input, IpVersion version) =>
            Validator.IsIp(input, version);

        public static bool IsEmail(this string input) =>
            Validator.IsEmail(input);

        public static bool IsHexadecimal(this string input) =>
            Validator.IsHexadecimal(input);

        public static bool IsAlphanumeric(this string input) =>
            Validator.IsAlphanumeric(input);

        public static bool IsHexColor(this string input) =>
            Validator.IsHexColor(input);

        public static bool Equals(this string input, string comparison) =>
            Validator.Equals(input, comparison);

        public static bool IsDate(this string input) =>
            Validator.IsDate(input);

        public static bool IsAfter(this string input, DateTime date) =>
            Validator.IsAfter(input, date);

        public static bool IsBefore(this string input, DateTime date) =>
            Validator.IsBefore(input, date);

        public static bool IsJson(this string input) =>
            Validator.IsJson(input);

        public static bool IsNull(this string input) =>
            Validator.IsNull(input);

        public static bool Contains(this string input, string element) =>
            Validator.Contains(input, element);

        public static bool Matches(this string input, string pattern, RegexOptions options = RegexOptions.None) =>
            Validator.Matches(input, pattern, options);

        public static bool IsMongoId(this string input) =>
            Validator.IsMongoId(input);

        public static bool IsByteLength(this string input, int min, int max = int.MaxValue) =>
            Validator.IsByteLength(input, min, max);

        public static bool IsCreditCard(this string input) => 
            Validator.IsCreditCard(input);

        public static bool IsBase64(this string input) => 
            Validator.IsBase64(input);

        public static bool IsFqdn(this string input, bool requireTld = true, 
            bool allowUnderscore = false, bool allowTrailingDot = false) => 
                Validator.IsFqdn(input, requireTld, allowUnderscore, allowTrailingDot);

        public static bool IsIsbn(this string input, IsbnVersion version = IsbnVersion.Any) => 
            Validator.IsIsbn(input, version);

        public static bool IsMobilePhone(this string input, string locale) => 
            Validator.IsMobilePhone(input, locale);

        public static bool IsNumeric(this string input) => 
            Validator.IsNumeric(input);

        public static bool IsUrl(this string input, Validator.UrlOptions options = null) => 
            Validator.IsUrl(input, options);

        public static bool IsUuid(this string input, UuidVersion version = UuidVersion.Any) => 
            Validator.IsUuid(input, version);
    }
}
