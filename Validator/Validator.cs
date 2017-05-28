using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Determine whether input is composed of one or more alphabetic characters only.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlpha(string input) => 
            Regex.IsMatch(input, "^[a-zA-Z]+$");

        /// <summary>
        /// Determine whether input is in lower case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLowercase(string input) => 
            input == input.ToLower();

        /// <summary>
        /// Determine whether input is in upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUppercase(string input) => 
            input == input.ToUpper();
        
        /// <summary>
        /// Determine whether input represents a numeric number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(string input) => IsNumeric(input);

        /// <summary>
        /// Determine whether input represents a valid floating point number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFloat(string input)
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
        public static bool IsDivisibleBy(string input, int @by)
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
        public static bool IsLength(string input, int min, int max) =>
            input.Length > min && input.Length < max;

        /// <summary>
        /// Determine whether all characters within string are ASCII based.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAscii(string input) => 
            input.Select(c => (int)c).All(c => c <= 127);

        /// <summary>
        /// Determiner whether the string contains one or more multibyte chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMultiByte(string input) => 
            Regex.IsMatch(input, "[^\x00-\x7F]");

        /// <summary>
        /// Determine whether the string contains any half-width chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHalfWidth(string input) => 
            Regex.IsMatch(input, "[\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");

        /// <summary>
        /// Determine if the string contains any full-width chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFullWidth(string input) => 
            Regex.IsMatch(input, "[^\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");

        /// <summary>
        /// Determine if the string contains a mixture of full and half-width chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVariableWidth(string input) => 
            IsHalfWidth(input) && IsFullWidth(input);

        /// <summary>
        /// Determine if the string contains any surrogate pairs chars.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSurrogatePair(string input) => 
            Regex.IsMatch(input, "[\uD800-\uDBFF][\uDC00-\uDFFF]");
        
        /// <summary>
        /// Determine whether any strings in values, appear in the input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="values">Array of string values to search for.</param>
        /// <returns></returns>
        public static bool IsIn(string input, string[] values) => 
            values.Any(value => value == input);
        
        /// <summary>
        /// Determine whether input is a valid IPv4 or IPv6 address.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid values are: IpVersion.Four and IpVersion.Six</param>
        /// <returns></returns>
        public static bool IsIp(string input, IpVersion version)
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
        public static bool IsEmail(string input)
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
        public static bool IsHexadecimal(string input) => 
            Regex.IsMatch(input, "^[0-9a-fA-F]+$");
        
        /// <summary>
        /// Determine whether string is composed of alphanumeric characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphanumeric(string input) => 
            Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        
        /// <summary>
        /// Determine whether input matches a valid hexadecimal colour.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsHexColor(string input) => 
            Regex.IsMatch(input, "^#?(?:[0-9a-fA-F]{3}){1,2}$");
        
        /// <summary>
        /// Determine whether two inputs are equal.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool Equals(string input, string comparison) => 
            input.Equals(comparison);

        /// <summary>
        /// Determine whether input string matches a valid date format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDate(string input)
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
        public static bool IsAfter(string input, DateTime date)
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
        public static bool IsBefore(string input, DateTime date)
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
        public static bool IsJson(string input)
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
        public static bool IsNull(string input) => input == null;
        
        /// <summary>
        /// Determine whether element appears in supplied input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Contains(string input, string element) => 
            input.Contains(element);
        
        /// <summary>
        /// Determine whether input matches supplied regular expression pattern, with optional options.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">Pattern to match against.</param>
        /// <param name="options">Supplied options. Default is None.</param>
        /// <returns></returns>
        public static bool Matches(string input, string pattern, RegexOptions options = RegexOptions.None) 
            => Regex.IsMatch(input, pattern, options);

        /// <summary>
        /// Determine whether input is a valid Mongo ID.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMongoId(string input) => 
            input.Length == 24 && IsHexadecimal(input);
        
        /// <summary>
        /// Determine whether length of supplied input is between min and max (inclusive).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns></returns>
        public static bool IsByteLength(string input, int min, int max = int.MaxValue) => 
            input.Length >= min && input.Length <= max;
    }
}
