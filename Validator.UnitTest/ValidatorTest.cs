using System.Net.Configuration;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

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

        [Theory]
        [InlineData("123", true)]
        [InlineData("50000000000000000000000000", true)]
        [InlineData("123.123", false)]
        [InlineData("", false)]
        [InlineData("", false)]
        public void IsInt(string input, bool expected)
        {
            var actual = Validator.IsInt(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("123.123", true)]
        [InlineData("123", true)]
        [InlineData("", false)]
        public void IsFloat(string input, bool expected)
        {
            var actual = Validator.IsFloat(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("10", 5, true)]
        [InlineData("10", 2, true)]
        [InlineData("5", 2, false)]
        [InlineData("Foo", 2, false)]
        public void IsDivisibleBy(string input, int by, bool expected)
        {
            var actual = Validator.IsDivisibleBy(input, by);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ab", 1, 2, true)]
        [InlineData("abc", 1, 2, false)]
        [InlineData("", 1, 2, false)]
        public void IsLength(string input, int min, int max, bool expected)
        {
            var actual = Validator.IsLength(input, min, max);
            Assert.Equal(expected, actual);
        }

    }
}