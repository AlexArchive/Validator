namespace Validator
{
    public partial class Validator
    {
        public static bool IsNull(string input)
        {
            if (input == null)
            {
                return true;
            }
            return false;
        }
    }
}
