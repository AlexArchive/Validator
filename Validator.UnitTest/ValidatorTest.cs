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

        [Theory]
        [InlineData("123", true)]
        [InlineData("Foo", false)]
        [InlineData("123Foo123", false)]
        public void IsNumeric_ReturnsCorrectResult(string input, bool expected)
        {
            var actual = Validator.IsNumeric(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("foo", true)]
        [InlineData("foo123", true)]
        [InlineData("Foo", false)]
        [InlineData("Foo123", false)]
        public void IsLowercase(string input, bool expected)
        {
            var actual = Validator.IsLowercase(input);
            Assert.Equal(expected, actual);
        }
    }
}