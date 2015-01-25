namespace Validator
{
    public partial class Validator
    {
        public static bool IsMongoId(string input)
        {
            return input.Length == 24 && IsHexadecimal(input);
        }
    }
}
