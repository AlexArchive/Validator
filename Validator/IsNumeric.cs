namespace Validator
{
    public partial class Validator
    {
        public static bool IsNumeric(string input)
        {
            if (input == null)
            {
                return false;
            }
            int length = input.Length;
            if (length == 0)
            {
                return false;
            }
            int i = 0;
            if (input[0] == '-')
            {
                if (length == 1)
                {
                    return false;
                }
                i = 1;
            }
            for (; i < length; i++)
            {
                char c = input[i];
                if (c <= '/' || c >= ':')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
