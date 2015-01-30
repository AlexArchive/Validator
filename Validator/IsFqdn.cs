using System.Linq;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsFqdn(
            string input,
            bool requireTld = true,
            bool allowUnderscore = false,
            bool allowTrailingDot = false)
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
                var tld = parts.Last();
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
                if (!Regex.IsMatch(copy, "^[a-z\u00a1-\uffff0-9-]+$"))
                {
                    return false;
                }
                if (copy[0] == '-' || copy.EndsWith("--") || copy.Contains("---"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}