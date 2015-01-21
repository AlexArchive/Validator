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
        [InlineData("Foo_Bar", false)]
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

        [Theory]
        [InlineData("Foo", true)]
        [InlineData("123", true)]
        [InlineData("Foo@example.com", true)]
        [InlineData("ｆｏｏ", false)]
        [InlineData("１２３", false)]
        public void IsAscii(string input, bool expected)
        {
            var actual = Validator.IsAscii(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Foo", new[] {"Foo", "Bar"}, true)]
        [InlineData("Bar", new[] {"Foo", "Bar"}, true)]
        [InlineData("Baz", new[] {"Foo", "Bar"}, false)]
        public void IsIn(string input, string[] values, bool expected)
        {
            var actual = Validator.IsIn(input, values);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("::1", IpVersion.Four, false)]
        [InlineData("127.0.0.1", IpVersion.Four, true)]
        [InlineData("0.0.0.0", IpVersion.Four, true)]
        [InlineData("255.255.255.255", IpVersion.Four, true)]
        [InlineData("abc", IpVersion.Four, false)]
        [InlineData("256.0.0.0", IpVersion.Four, false)]
        [InlineData("26.0.0.256", IpVersion.Four, false)]
        [InlineData("::1", IpVersion.Six, true)]
        [InlineData("2001:db8:0000:1:1:1:1:1", IpVersion.Six, true)]
        [InlineData("127.0.0.1", IpVersion.Six, false)]
        [InlineData("0.0.0.0", IpVersion.Six, false)]
        [InlineData("::1", IpVersion.Six, true)]
        public void IsIp(string input, IpVersion version, bool expected)
        {
            var actual = Validator.IsIp(input, version);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("foo@bar.com", true)]
        [InlineData("foo@bar.com.au", true)]
        [InlineData("foo+bar@bar.com", true)]
        [InlineData("invalidemail@", false)]
        [InlineData("invalid.com", false)]
        [InlineData("@invalid.com", false)]
        public void IsEmail(string input, bool expected)
        {
            var actual = Validator.IsEmail(input);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("deadBEEF", true)]
        [InlineData("ff0044", true)]
        [InlineData("abcdefg", false)]
        [InlineData("", false)]
        [InlineData("..", false)]
        public void IsHexadecimal(string input, bool expected)
        {
            var actual = Validator.IsHexadecimal(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Foo1", true)]
        [InlineData("foo1", true)]
        [InlineData("Foo 1", false)]
        [InlineData("Foo_", false)]
        public void IsAlphanumeric(string input, bool expected)
        {
            var actual = Validator.IsAlphanumeric(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("375556917985515", true)]
        [InlineData("36050234196908", true)]
        [InlineData("4716461583322103", true)]
        [InlineData("4716-2210-5188-5662", true)]
        [InlineData("4929 7226 5379 7141", true)]
        [InlineData("5398228707871527", true)]
        [InlineData("Foo", false)]
        [InlineData("Bar123", false)]
        [InlineData("5398228707871528", false)]
        public void IsCreditCard(string input, bool expected)
        {
            var actual = Validator.IsCreditCard(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#ff0034", true)]
        [InlineData("#CCCCCC", true)]
        [InlineData("fff", true)]
        [InlineData("#fff", true)]
        [InlineData("#ff", false)]
        [InlineData("fff0", false)]
        [InlineData("#ff12FG", false)]
        public void IsHexColor(string input, bool expected)
        {
            var actual = Validator.IsHexColor(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Foo", true)]
        [InlineData("Bar", false)]
        [InlineData("Baz", false)]
        public void IsEqual(string input, bool expected)
        {
            var actual = Validator.Equals(input, "Foo");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("Not a JSON string", false)]
        [InlineData("{\"username\":\"Admin\"}", true)]
        [InlineData("{username:\"Admin\"", false)]
        public void IsJson(string input, bool expected)
        {
            var actual = Validator.IsJson(input);
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("Not a date", false)]
        [InlineData("01/01/2001", true)]
        [InlineData("50/20/2017", false)]
        [InlineData("01-01-2001", true)]
        [InlineData("2001/01/01", true)]
        [InlineData("01.01.2001", true)]
        [InlineData("Not05/01A/date/2001", false)]
        public void IsDate(string input, bool expected)
        {
            var actual = Validator.IsDate(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", false)]
        [InlineData("  ", false)]
        [InlineData("NULL", false)]
        public void IsNull(string input, bool expected)
        {
            var actual = Validator.IsNull(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0596004427", true)]
        [InlineData("0-596-00442-7", true)]
        [InlineData("0 596 00442 7", true)]
        [InlineData("161729134X", true)]
        [InlineData("1-617291-34-X", true)]
        [InlineData("1 617291 34 X", true)]
        [InlineData("3423214", false)]
        [InlineData("342321412122", false)]
        [InlineData("3423214121", false)]
        [InlineData("3-423-21412-1", false)]
        [InlineData("3 423 21412 1", false)]
        [InlineData("Foo", false)]
        public void IsIsbnVersion10(string input, bool expected)
        {
            var actual = Validator.IsIsbn(input);
            Assert.Equal(expected, actual);
        }
    }
}