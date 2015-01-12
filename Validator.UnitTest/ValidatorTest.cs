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
        public void IsAlpha(string input, bool expected)
        {
            var actual = Validator.IsAlpha(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("Foo", false)]
        [InlineData("123Foo123", false)]
        public void IsNumeric(string input, bool expected)
        {
            var actual = Validator.IsNumeric(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("foo", true)]
        [InlineData("foo123", true)]
        [InlineData("FOO", false)]
        [InlineData("FOO123", false)]
        public void IsLowercase(string input, bool expected)
        {
            var actual = Validator.IsLowercase(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("FOO", true)]
        [InlineData("FOO123", true)]
        [InlineData("foo", false)]
        [InlineData("foo123", false)]
        public void IsUppercase(string input, bool expected)
        {
            var actual = Validator.IsUppercase(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SGk=", true)]
        [InlineData("VmFsaWRhdG9y", true)]
        [InlineData("Foo", false)]
        [InlineData("Foo\r\nBar", false)]
        [InlineData("Foo?", false)]
        public void IsBase64(string input, bool expected)
        {
            var actual = Validator.IsBase64(input);
            Assert.Equal(expected, actual);
        }
    }
}