using System.Linq;

namespace Validator
{
     public partial class Validator
    {
        /// <summary>
        /// Indicates whether input is in correct format for a credit card number and is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsCreditCard(string input)
        {
            input = input.Replace(" ", "");
            input = input.Replace("-", "");
            if (!IsNumeric(input))
            {
                return false;
            }
            
            var sumOfDigits = input
                .Where((e) => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => (e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum((e) => e / 10 + e % 10);
            return sumOfDigits % 10 == 0;
        }
    }
}
