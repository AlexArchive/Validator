using System.Text.RegularExpressions;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsIsbn(string input)
        {
            var checksum = 0;
            input = RemoveSpacesAndHyphens(input);

            // 1 - Ensure that input only contains 10 numbers OR 9 numbers and the letter X.
            if (!Regex.IsMatch(input, "^[0-9]{9}X|[0-9]{10}$"))
            {
                return false;
            }

            // 2 - Automatically multiply 9 (of the 10) numbers by their weight.
            for (var index = 0; index < 9; index++)
            {
                checksum += (index + 1) * int.Parse(input[index].ToString()); 
            }

            // 3 - Manually multiply the 10th number.
            if (input[9] == 'X')
            {
                checksum += 10 * 10;
            }
            else
            {
                checksum += 10 * int.Parse(input[9].ToString());
            }

            // 4 - Ensure that the checksum is a multiple of 11.
            return checksum % 11 == 0;
        }

        private static string RemoveSpacesAndHyphens(string input)
        {
            return Regex.Replace(input, "[\\s-]+", "");
        }
    }
}