using System.Linq;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Indicates whether input represents a fully qualified domain name.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requireTld"></param>
        /// <param name="allowUnderscore"></param>
        /// <param name="allowTrailingDot"></param>
        /// <returns></returns>
        public static bool IsFqdn(string input, bool requireTld = true, 
            bool allowUnderscore = false, bool allowTrailingDot = false)
        {
            if (allowTrailingDot && input.EndsWith("."))
            {
                input = input.Remove(input.Length - 1);
            }

            var parts = input.Split('.');
            if (requireTld)
            {
                if (parts.Length == 1)
                {
                    return false;
                }
                
				// validate.js utilizes the pop() method which both modifies the source array and returns the last element
				// c# won't let us do that directly, so use the Last() method to get the last element, then trim it off
	            var tld = parts.Last();
	            parts = parts.Except(Enumerable.Repeat(tld, 1)).ToArray();
                if (!Regex.IsMatch(tld, "^([a-z\u00a1-\uffff]{2,}|xn[a-z0-9-]{2,})$"))
                {
                    return false;
                }
            }

            foreach (var part in parts)
            {
                var copy = part;
                if (allowUnderscore)
                {
                    if (copy.Contains("__"))
                    {
                        return false;
                    }
                    copy = copy.Replace("_", "");
                }
                
				// the JS regex had that magic "i" at the end, signifying to ignore case, so let's match that here
                if (!Regex.IsMatch(copy, "^[a-z\u00a1-\uffff0-9-]+$", RegexOptions.IgnoreCase))
                {
                    return false;
                }

                if (copy[0] == '-' || copy.EndsWith("-") || copy.Contains("---"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}