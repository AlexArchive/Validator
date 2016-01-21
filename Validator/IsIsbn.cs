using System;
using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Indicates whether supplied input is either in ISBN-10 digit format or ISBN-13 digit format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version">Valid options are: IsbnVersion.Ten, IsbnVersion.Thirteen or IsbnVersion.Any</param>
        /// <returns></returns>
        /// IsbnVersion
        public static bool IsIsbn(string input, IsbnVersion version = IsbnVersion.Any)
        {
            input = RemoveSpacesAndHyphens(input);
            switch (version)
            {
                case IsbnVersion.Any:
                    return IsIsbn10(input) || IsIsbn13(input);
                case IsbnVersion.Thirteen:
                    return IsIsbn13(input);
                case IsbnVersion.Ten:
                    return IsIsbn10(input);
            }
            throw new ArgumentOutOfRangeException(nameof(version), $"Isbn version {version} is not supported.");
        }

        /// <summary>
        /// Indicates whether supplied input is in ISBN 13 digit format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsIsbn13(string input)
        {
            var checksum = 0;
            
            // Ensure that input only contains 13 numbers.
            if (!Regex.IsMatch(input, "^[0-9]{13}$"))
            {
                return false;
            }
            
            var factor = new[] { 1, 3 };
            for (var i = 0; i < 12; i++)
            {
                checksum += factor[i % 2] * int.Parse(input[i].ToString());
            }
            
            return int.Parse(input[12].ToString()) - ((10 - (checksum % 10)) % 10) == 0;
        }

        /// <summary>
        /// Indicates whether supplied input is in ISBN 10 digit format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsIsbn10(string input)
        {
            var checksum = 0;
            // Ensure that input only contains 10 numbers OR 9 numbers and the letter X.
            if (!Regex.IsMatch(input, "^[0-9]{9}X|[0-9]{10}$"))
            {
                return false;
            }
            
            // Automatically multiply 9 (of the 10) numbers by their weight.
            for (var i = 0; i < 9; i++)
            {
                checksum += (i + 1) * int.Parse(input[i].ToString());
            }
            
            // Manually multiply the 10th number.
            if (input[9] == 'X')
            {
                checksum += 10*10;
            }
            else
            {
                checksum += 10 * int.Parse(input[9].ToString());
            }
            
            // Ensure that the checksum is a multiple of 11.
            return checksum % 11 == 0;
        }

        /// <summary>
        /// Remove all white-space and hyphen characters from input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string RemoveSpacesAndHyphens(string input) => 
            Regex.Replace(input, "[\\s-]+", "");
    }
}