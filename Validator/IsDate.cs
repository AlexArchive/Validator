using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsDate(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            try
            {
                DateTime.Parse(input);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }
    }
}
