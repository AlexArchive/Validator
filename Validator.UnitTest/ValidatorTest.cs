using Xunit;
using Xunit.Extensions;

namespace Validator.UnitTest
{
    public class ValidatorTest
    {
        [Theory]
        [InlineData("Foo", true)]
        [InlineData("1Foo", false)]
        [InlineData("123", false)]
        [InlineData("1Foo\r\n12", false)]
        public void IsAlpha_ReturnsCorrectResult(string input, bool expected)
        {
            var actual = Validator.IsAlpha(input);
            Assert.Equal(expected, actual);
        }
    }
}