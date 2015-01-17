using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsAlpha(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z]+$");
        }

        public static bool IsLowercase(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsLower(input[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsUppercase(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsInt(string input)
        {
            if (!IsNumeric(input))
            {
                return false;
            }
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
            if (!IsNumeric(input))
            {
                return false;
            }
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
            return input.Select(c => (int)c).All(c => c <= 127);
        }

        public static bool IsIn(string input, string[] values)
        {
            return values.Any(value => value == input);
        }

        public static bool IsIp(string input, IpVersion version)
        {
            IPAddress address;
            if (IPAddress.TryParse(input, out address))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork && version == IpVersion.Four)
                {
                    return true;
                }
                if (address.AddressFamily == AddressFamily.InterNetworkV6 && version == IpVersion.Six)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsEmail(string input)
        {
            return new EmailAddressAttribute().IsValid(input);
        }

        public static bool IsHexadecimal(string input)
        {
            return Regex.IsMatch(input, "^[0-9a-fA-F]+$");
        }

        public static bool IsAlphanumeric(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        }

        public static bool IsHexColor(string input)
        {
            return Regex.IsMatch(input, "^#?(?:[0-9a-fA-F]{3}){1,2}$");
        }

        public static bool Equals(string input, string comparison)
        {
            return input.Equals(comparison);
        }
    }
}