using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsCreditCard(string input)
        {
            input = input.Replace(" ", "");
            input = input.Replace("-", "");
            if (!IsNumeric(input))
            {
                return false;
            }
            int sumOfDigits = input
                .Where((e) => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum((e) => e / 10 + e % 10);
            return sumOfDigits % 10 == 0;
        }
    }
}
