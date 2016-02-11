using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        private static readonly Dictionary<string, Regex> LocaleMobilePhoneRegexes = new Dictionary<string, Regex>
		{
            ["zh-CN"] = new Regex(@"^(\+?0?86\-?)?1[345789][0-9]{9}$", RegexOptions.Compiled),
            ["zh-TW"] = new Regex(@"^(\+?886\-?|0)?9\d{8}$", RegexOptions.Compiled),
            ["en-ZA"] = new Regex(@"^(\+?27|0)(\d{9})$", RegexOptions.Compiled),
            ["en-AU"] = new Regex(@"^(\+?61|0)4(\d{8})$", RegexOptions.Compiled), 
            ["fr-FR"] = new Regex(@"^(\+?33|0)(6|7)\d{8}$", RegexOptions.Compiled),
            ["en-HK"] = new Regex(@"^(\+?852\-?)?[569]\d{3}\-?\d{4}$", RegexOptions.Compiled),
            ["pt-PT"] = new Regex(@"^(\+351)?9[1236]\d{7}$", RegexOptions.Compiled), 
            ["el-GR"] = new Regex(@"^(\+30)?((2\d{9})|(69\d{8}))$", RegexOptions.Compiled), 
            ["en-GB"] = new Regex(@"^(\+?44|0)7\d{9}$", RegexOptions.Compiled),
            ["en-US"] = new Regex(@"^(\+?1)?[2-9]\d{2}[2-9](?!11)\d{6}$", RegexOptions.Compiled), 
            ["en-ZM"] = new Regex(@"^(\+26)?09[567]\d{7}$", RegexOptions.Compiled),
            ["ru-RU"] = new Regex(@"^(\+?7|8)?9\d{9}$", RegexOptions.Compiled), 
            ["nb-NO"] = new Regex(@"^(\+?47)?[49]\d{7}$", RegexOptions.Compiled), 
            ["nn-NO"] = new Regex(@"^(\+?47)?[49]\d{7}$", RegexOptions.Compiled)
		};

        /// <summary>
        /// Determines whether the given phone number is a mobile phone number or not.
        /// </summary>
        /// <param name="input">The phone number to check.</param>
        /// <param name="locale">The locale to look in.</param>
        /// <returns>True if it is a mobile phone number, false otherwise.</returns>
        /// <remarks>
        /// Relies on locales that use specific blocks of numbers for mobile phone numbers.
        /// </remarks>
        public static bool IsMobilePhone(string input, string locale)
        {
            Regex localeRegex;
            if (LocaleMobilePhoneRegexes.TryGetValue(locale, out localeRegex))
            {
                return localeRegex.IsMatch(input);
            }
            
            return false;
        }
    }
}
