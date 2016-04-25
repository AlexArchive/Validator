namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Determine whether input represents a numeric number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(string input)
        {
            double dummy;
            return double.TryParse(input, out dummy);
        }
    }
}
