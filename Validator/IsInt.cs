namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Determine whether input represents an integer
        /// Can only check numbers as large as long.MaxValue or as small as long.MinValue
        /// Commas are not valid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(string input)
        {
            long dummy;
            return long.TryParse(input, out dummy);
        }
    }
}