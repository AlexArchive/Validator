using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Validator
{
    public static partial class Validator
    {
        /// <summary>
        /// Determine whether input is composed of one or more alphabetic characters only.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlpha(this string input) => 
            Regex.IsMatch(input, "^[a-zA-Z]+$");

        /// <summary>
        /// Determine whether input is in lower case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLowercase(this string input) => 
            input == input.ToLower();

        /// <summary>
        /// Determine whether input is in upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUppercase(this string input) => 
            input == input.ToUpper();
        
        /// <summary>
        /// Determine whether input represents a numeric number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(this string input) => IsNumeric(input);

        /// <summary>
        /// Determine whether input represents a valid floating point number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFloat(this string input)
        {
            float value;
            return float.TryParse(input, out value);
        }

        /// <summary>
        /// Determine whether supplied input (once converted to an integer) is a multiple of @by.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="by">The divisor.</param>
        /// <returns></returns>
        public static bool IsDivisibleBy(this string input, int @by)
        {
            int value;
            if (!int.TryParse(input, out value))
            {
                return false;
            }
            
            return value % @by == 0;
        }

        /// <summary>
        /// Determine whether length of input is between (inclusive) min and max.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns></returns>
        public static bool IsLength(this string input, int min, int max)
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

        /// <summary>
        /// Determine whether all characters within string are ASCII based.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAscii(this string input) => input.Select(c => (int)c).All(c => c <= 127);

        public static bool IsMultiByte(this string input) => 
            Regex.IsMatch(input, "[^\x00-\x7F]");

        public static bool IsHalfWidth(this string input) => 
            Regex.IsMatch(input, "[\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");

        public static bool IsFullWidth(this string input) => 
            Regex.IsMatch(input, "[^\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");

        public static bool IsVariableWidth(this string input) => 
            IsHalfWidth(input) && IsFullWidth(input);

        public static bool IsSurrogatePair(this string input) => 
            Regex.IsMatch(input, "[\uD800-\uDBFF][\uDC00-\uDFFF]");
        
        /// <summary>
        /// Determin whether any strings in values, appear in the input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="values">Array of string values to search for.</param>
        /// <returns></returns>
        public static bool IsIn(this string input, string[] values) => 
            values.Any(value => value == input);
        
        /// <summary>
        /// Determine whether input is a valid IPv4 or IPv6 address.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid values are: IpVersion.Four and IpVersion.Six</param>
        /// <returns></returns>
        public static bool IsIp(this string input, IpVersion version)
        {
            const string ipv4MaybePattern = @"^(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)$";
            const string ipv6Pattern = @"^::|^::1|^([a-fA-F0-9]{1,4}::?){1,7}([a-fA-F0-9]{1,4})$";

            if (version == IpVersion.Four)
            {
                if (!Validator.Matches(input, ipv4MaybePattern))
                {
                    return false;
                }

                var parts = input.Split('.').Select(p => Convert.ToInt32(p));
                return parts.Max() <= 255;
            }

            return Validator.Matches(input, ipv6Pattern);
        }

        /// <summary>
        /// Determine whether input matches a valid email address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            try
            {
                return new MailAddress(input).Address == input;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determine whether input matches a hexadecimal formatted string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHexadecimal(this string input) => 
            Regex.IsMatch(input, "^[0-9a-fA-F]+$");
        
        /// <summary>
        /// Determine whether string is composed of alphanumeric characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphanumeric(this string input) => 
            Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        
        /// <summary>
        /// Determine whether input matches a valid hexadecimal colour.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHexColor(this string input) => 
            Regex.IsMatch(input, "^#?(?:[0-9a-fA-F]{3}){1,2}$");
        
        /// <summary>
        /// Determine whether two inputs are equal.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool Equals(this string input, string comparison) => 
            input.Equals(comparison);

        /// <summary>
        /// Determine whether input string matches a valid date format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDate(this string input)
        {
            DateTime date;
            return DateTime.TryParse(input, out date);
        }

        /// <summary>
        /// Determine whether input string matches a valid date AND occurs after supplied date.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsAfter(this string input, DateTime date)
        {
            DateTime inputDate;
            if (!DateTime.TryParse(input, out inputDate))
            {
                return false;
            }
            
            return inputDate > date;
        }

        /// <summary>
        /// Determine whether input string matches a valid date AND occurs before supplied date.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsBefore(this string input, DateTime date)
        {
            DateTime inputDate;
            if (!DateTime.TryParse(input, out inputDate))
            {
                return false;
            }
            
            return inputDate < date;
        }

        /// <summary>
        /// Determine whether input is a valid JSON string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsJson(this string input)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                serializer.Deserialize<dynamic>(input);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determine whether input is null.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNull(this string input) => input == null;
        
        /// <summary>
        /// Determine whether element appears in supplied input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Contains(this string input, string element) => 
            input.Contains(element);
        
        /// <summary>
        /// Determine whether input matches supplied regular expression pattern, with optional options.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">Pattern to match against.</param>
        /// <param name="options">Supplied options. Default is None.</param>
        /// <returns></returns>
        public static bool Matches(this string input, string pattern, RegexOptions options = RegexOptions.None) 
            => Regex.IsMatch(input, pattern, options);

        /// <summary>
        /// Determine whether input is a valid Mongo ID.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMongoId(this string input) => 
            input.Length == 24 && IsHexadecimal(input);
        
        /// <summary>
        /// Determine whether length of supplied input is between min and max (inclusive).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns></returns>
        public static bool IsByteLength(this string input, int min, int max = int.MaxValue) => 
            input.Length >= min && input.Length <= max;
    }
}