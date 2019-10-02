using System;
using System.Collections;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Extensions;

namespace Validator.UnitTest
{
    public class ValidatorTest
    {
        [Theory]
        [InlineData("9783836221191", true)]
        [InlineData("978-3-8362-2119-1", true)]
        [InlineData("978 3 8362 2119 1", true)]
        [InlineData("9784873113685", true)]
        [InlineData("978-4-87311-368-5", true)]
        [InlineData("978 4 87311 368 5", true)]
        [InlineData("9783836221190", false)]
        [InlineData("978-3-8362-2119-0", false)]
        [InlineData("978 3 8362 2119 0", false)]
        [InlineData("Foo", false)]
        public void IsIsbnVersion13(string input, bool expected)
        {
            var actual = Validator.IsIsbn(input, IsbnVersion.Thirteen);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("9784873113685")]
        public void IsIsbnnThrowsWhenSuppliedUnknownVersion(string input)
        {
            const int version = 42;
            var message = Assert.Throws<ArgumentOutOfRangeException>(() => Validator.IsIsbn(input, (IsbnVersion)version));
            Assert.Contains("Isbn version " + version + " is not supported.", message.Message);
        }

        [Theory]
        [InlineData(UuidVersion.Three, "A987FBC9-4BED-3078-CF07-9141BA07C9F3", true)]

        [InlineData(UuidVersion.Three, "", false)]
        [InlineData(UuidVersion.Three, "xxxA987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]
        [InlineData(UuidVersion.Three, "934859", false)]
        [InlineData(UuidVersion.Three, "AAAAAAAA-1111-1111-AAAG-111111111111", false)]
        [InlineData(UuidVersion.Three, "A987FBC9-4BED-4078-8F07-9141BA07C9F3", false)]
        [InlineData(UuidVersion.Three, "A987FBC9-4BED-5078-AF07-9141BA07C9F3", false)]

        [InlineData(UuidVersion.Four, "713ae7e3-cb32-45f9-adcb-7c4fa86b90c1", true)]
        [InlineData(UuidVersion.Four, "625e63f3-58f5-40b7-83a1-a72ad31acffb", true)]
        [InlineData(UuidVersion.Four, "57b73598-8764-4ad0-a76a-679bb6640eb1", true)]
        [InlineData(UuidVersion.Four, "9c858901-8a57-4791-81fe-4c455b099bc9", true)]

        [InlineData(UuidVersion.Four, "", false)]
        [InlineData(UuidVersion.Four, "xxxA987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]
        [InlineData(UuidVersion.Four, "934859", false)]
        [InlineData(UuidVersion.Four, "AAAAAAAA-1111-1111-AAAG-111111111111", false)]
        [InlineData(UuidVersion.Four, "A987FBC9-4BED-5078-AF07-9141BA07C9F3", false)]
        [InlineData(UuidVersion.Four, "A987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]

        [InlineData(UuidVersion.Five, "987FBC97-4BED-5078-AF07-9141BA07C9F3", true)]
        [InlineData(UuidVersion.Five, "987FBC97-4BED-5078-BF07-9141BA07C9F3", true)]
        [InlineData(UuidVersion.Five, "987FBC97-4BED-5078-8F07-9141BA07C9F3", true)]
        [InlineData(UuidVersion.Five, "987FBC97-4BED-5078-9F07-9141BA07C9F3", true)]

        [InlineData(UuidVersion.Five, "", false)]
        [InlineData(UuidVersion.Five, "xxxA987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]
        [InlineData(UuidVersion.Five, "934859", false)]
        [InlineData(UuidVersion.Five, "AAAAAAAA-1111-1111-AAAG-111111111111", false)]
        [InlineData(UuidVersion.Five, "9c858901-8a57-4791-81fe-4c455b099bc9", false)]
        [InlineData(UuidVersion.Five, "A987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]
        public void IsUuidWithVersion(UuidVersion version, string input, bool expectedValid)
        {
            var actual = Validator.IsUuid(input, version);
            Assert.Equal(actual, expectedValid);
        }

        [Theory]
        [InlineData("A987FBC9-4BED-3078-CF07-9141BA07C9F3")]
        public void IsUuidThrowsWhenSuppliedUnknownVersion(string input)
        {
            const int invalidVersion = 99;
            var message = Assert.Throws<ArgumentOutOfRangeException>(() => Validator.IsUuid(input, (UuidVersion)invalidVersion));
            Assert.Contains("Uuid version " + invalidVersion + " is not supported.", message.Message);
        }

        [Theory]
        [InlineData("example.com.")]
        public void IsFqdnWithTrailingDotOption(string input)
        {
            var actual = Validator.IsFqdn(input, allowTrailingDot: true);
            Assert.True(actual);
        }

        [Theory]
        [InlineData("test_.com")]
        public void IsFqdnWithUnderscoreOption(string input)
        {
            var actual = Validator.IsFqdn(input, allowUnderscore: true);
            Assert.True(actual);
        }

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
        [InlineData("ひらがな・カタカナ、．漢字", true)]
        [InlineData("あいうえお foobar", true)]
        [InlineData("Foo＠example.com", true)]
        [InlineData("1234abcDEｘｙｚ", true)]
        [InlineData("ｶﾀｶﾅ", true)]
        [InlineData("中文", true)]
        [InlineData("æøå", true)]
        [InlineData("abc", false)]
        [InlineData("abc123", false)]
        [InlineData("<>@\" *.", false)]
        public void IsMultibyte(string input, bool expected)
        {
            var actual = Validator.IsMultiByte(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("!\"#$%&()<>/+=-_? ~^|.,@`{}[]", true)]
        [InlineData("l-btn_02--active", true)]
        [InlineData("abc123い", true)]
        [InlineData("ｶﾀｶﾅﾞﾬ￩", true)]
        [InlineData("あいうえお", false)]
        [InlineData("００１１", false)]
        public void IsHalfWidth(string input, bool expected)
        {
            var actual = Validator.IsHalfWidth(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ひらがな・カタカナ、．漢字", true)]
        [InlineData("３ー０　ａ＠ｃｏｍ", true)]
        [InlineData("Ｆｶﾀｶﾅﾞﾬ", true)]
        [InlineData("Good＝Parts", true)]
        [InlineData("abc", false)]
        [InlineData("abc123", false)]
        [InlineData("!\"#$%&()<>/+=-_? ~^|.,@`{}[]", false)]
        public void IsFullWidth(string input, bool expected)
        {
            var actual = Validator.IsFullWidth(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ひらがなカタカナ漢字ABCDE", true)]
        [InlineData("３ー０123", true)]
        [InlineData("Ｆｶﾀｶﾅﾞﾬ", true)]
        [InlineData("Good＝Parts", true)]
        [InlineData("abc", false)]
        [InlineData("abc123", false)]
        [InlineData("!\"#$%&()<>/+=-_? ~^|.,@`{}[]", false)]
        [InlineData("ひらがな・カタカナ、．漢字", false)]
        [InlineData("１２３４５６", false)]
        [InlineData("ｶﾀｶﾅﾞﾬ", false)]
        public void IsVariableWidth(string input, bool expected)
        {
            var actual = Validator.IsVariableWidth(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("𠮷野𠮷", true)]
        [InlineData("𩸽", true)]
        [InlineData("ABC千𥧄1-2-3", true)]
        [InlineData("吉野竈", false)]
        [InlineData("鮪", false)]
        [InlineData("ABC1-2-3", false)]
        public void IsSurrogatePair(string input, bool expected)
        {
            var actual = Validator.IsSurrogatePair(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Foo", new[] { "Foo", "Bar" }, true)]
        [InlineData("Bar", new[] { "Foo", "Bar" }, true)]
        [InlineData("Baz", new[] { "Foo", "Bar" }, false)]
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

        public static IEnumerable IsAfterData
        {
            get
            {
                return new[]
                {
                    new object[] {null, new DateTime(2011, 8, 4), false},
                    new object[] {"", new DateTime(2011, 8, 4), false},
                    new object[] {"2011-08-04", new DateTime(2011, 8, 3), true},
                    new object[] {"2011-08-10", new DateTime(2011, 8, 3), true},
                    new object[] {"2010-07-02", new DateTime(2011, 8, 3), false},
                    new object[] {"2011-08-03", new DateTime(2011, 8, 3), false},
                    new object[] {"foo", new DateTime(2011, 8, 3), false}
                };
            }
        }

        [Theory]
        [PropertyData("IsAfterData")]
        public void IsAfter(string input, DateTime date, bool expected)
        {
            var actual = Validator.IsAfter(input, date);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable IsBeforeData
        {
            get
            {
                return new[]
                {
                    new object[] {null, new DateTime(2011, 8, 4), false},
                    new object[] {"", new DateTime(2011, 8, 4), false},
                    new object[] {"2010-07-02", new DateTime(2011, 8, 4), true},
                    new object[] {"2010-08-04", new DateTime(2011, 8, 4), true},
                    new object[] {"2011-08-04", new DateTime(2011, 8, 4), false},
                    new object[] {"2011-09-10", new DateTime(2011, 8, 4), false},
                    new object[] {"2010-07-02", new DateTime(2011, 7, 4), true},
                    new object[] {"2010-08-04", new DateTime(2011, 7, 4), true},
                    new object[] {"2011-08-04", new DateTime(2011, 7, 4), false},
                    new object[] {"2011-09-10", new DateTime(2011, 7, 4), false},
                    new object[] {"foo", new DateTime(2011, 7, 4), false}
                };
            }
        }

        [Theory]
        [PropertyData("IsBeforeData")]
        public void IsBefore(string input, DateTime date, bool expected)
        {
            var actual = Validator.IsBefore(input, date);
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
        [InlineData("340101319X", true)]
        [InlineData("9784873113685", true)]
        [InlineData("3423214121", false)]
        [InlineData("9783836221190", false)]
        [InlineData("Foo", false)]
        public void IsIsbn(string input, bool expected)
        {
            var actual = Validator.IsIsbn(input);
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
            var actual = Validator.IsIsbn(input, IsbnVersion.Ten);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Validator", "Valid", true)]
        [InlineData("Validator", "lid", true)]
        [InlineData("Validator", "", true)]
        [InlineData("", "", true)]
        [InlineData("", " ", false)]
        public void Contains(string input, string element, bool expected)
        {
            var actual = Validator.Contains(input, element);
            Assert.Equal(expected, actual);
        }

        [Theory(Skip = "abc@xyz.com will return true")]
        [InlineData("http://Microsoft.com", true)]
        [InlineData("https://api.trello.com/1/boards/4d5ea62fd76aa1136000000c", true)]
        [InlineData("ftp://ftp.funet.fi/pub/standards/RFC/rfc959.txt", true)]
        [InlineData("http://www.nerddinner.com/Services/OData.svc/", true)] // OData url
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("InvalidUrl", false)]
        [InlineData("01/01/01", false)]
        [InlineData("0123456789", false)]
        [InlineData("!@#$%^", false)]
        [InlineData("abc@xyz.com", false)] // source input.js would fail this too, as far as i know.
        public void IsUrl(string input, bool expected)
        {
            var actual = Validator.IsUrl(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("xyz://foobar.com", false)]
        [InlineData("valid.au", true)]
        [InlineData("foobar.com/", true)]
        [InlineData("foobar.com", true)]
        public void IsUrl2(string input, bool expected)
        {
            var actual = Validator.IsUrl(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Foo", "Foo", true)]
        [InlineData("Bar", "B.*", true)]
        [InlineData("Baz", "B.*", true)]
        [InlineData("bar", "B.*", false)]
        [InlineData("Foo", "B.*", false)]
        [InlineData("foo", "Foo", false)]
        public void Matches(string input, string pattern, bool expected)
        {
            var actual = Validator.Matches(input, pattern);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("Foo", "foo", RegexOptions.IgnoreCase, true)]
        [InlineData("\r\nFoo", "^Foo$", RegexOptions.Multiline, true)]
        public void MatchesWithOptions(string input, string pattern, 
            RegexOptions options, bool expected)
        {
            var actual = Validator.Matches(input, pattern, options);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("A987FBC9-4BED-3078-CF07-9141BA07C9F3", true)]
        [InlineData("A987FBC9-4BED-4078-8F07-9141BA07C9F3", true)]
        [InlineData("A987FBC9-4BED-5078-AF07-9141BA07C9F3", true)]
        [InlineData("", false)]
        [InlineData("xxxA987FBC9-4BED-3078-CF07-9141BA07C9F3", false)]
        [InlineData("A987FBC9-4BED-3078-CF07-9141BA07C9F3xxx", false)]
        [InlineData("A987FBC94BED3078CF079141BA07C9F3", false)]
        [InlineData("934859", false)]
        [InlineData("987FBC9-4BED-308-CF07A-9141BA07C9F3", false)]
        [InlineData("AAAAAAAA-1111-1111-AAAG-111111111111", false)]
        public void IsUuidWithAnyVersion(string input, bool expectedValid)
        {
            var actual = Validator.IsUuid(input);
            Assert.Equal(actual, expectedValid);
        }

        [Theory]
        [InlineData("507f1f77bcf86cd799439011", true)]
        [InlineData("507f1f77bcf86cd7994390", false)]
        [InlineData("507f1f77bcf86cd79943901z", false)]
        [InlineData("", false)]
        [InlineData("507f1f77bcf86cd799439011 ", false)]
        [InlineData("507s1f77bcf86cd799439011", false)]
        public void IsMongoId(string input, bool expected)
        {
            var actual = Validator.IsMongoId(input);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("Foo", 3, true)]
        [InlineData("Foo", 2, true)]
        [InlineData("Foo Bar", 3, true)]
        [InlineData("Foo", 5, false)]
        [InlineData("F", 2, false)]
        [InlineData("", 2, false)]
        public void IsByteLength(string input, int min, bool expected)
        {
            var actual = Validator.IsByteLength(input, min);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("Foo", 2, 3, true)]
        [InlineData("Foo", 3, 5, true)]
        [InlineData("Foo Bar", 5, 7, true)]
        [InlineData("Foo", 5, 10, false)]
        [InlineData("", 2, 3, false)]
        public void IsByteLengthWithMax(string input, int min, int max, bool expected)
        {
            var actual = Validator.IsByteLength(input, min, max);
            Assert.Equal(actual, expected); // assert equal is a method from the inbuilt library.
        }

        [Theory]
        [InlineData("domain.com", true)]
        [InlineData("dom.plato", true)]
        [InlineData("a.domain.co", true)]
        [InlineData("foo--bar.com", true)]
        [InlineData("xn--froschgrn-x9a.com", true)]
        [InlineData("rebecca.blackfriday", true)]
        [InlineData("abc", false)]
        [InlineData("256.0.0.0", false)]
        [InlineData("_.com", false)]
        [InlineData("*.some.com", false)]
        [InlineData("s!ome.com", false)]
        [InlineData("domain.com/", false)]
        [InlineData("/more.com", false)]
        public void IsFqdn(string input, bool expected)
        {
            var actual = Validator.IsFqdn(input);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("example")]
        [InlineData("input")]
        public void IsFqdnWithoutRequireTldOption(string input)
        {
            var actual = Validator.IsFqdn(input, requireTld: false);
            Assert.True(actual);
        }

        //copied from https://github.com/chriso/input.js/blob/master/test/validators.js
        [Theory]
        [InlineData("15323456787", "zh-CN", true)]
        [InlineData("13523333233", "zh-CN", true)]
        [InlineData("13898728332", "zh-CN", true)]
        [InlineData("+086-13238234822", "zh-CN", true)]
        [InlineData("08613487234567", "zh-CN", true)]
        [InlineData("8617823492338", "zh-CN", true)]
        [InlineData("86-17823492338", "zh-CN", true)]
        [InlineData("12345", "zh-CN", false)]
        [InlineData("", "zh-CN", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "zh-CN", false)]
        [InlineData("010-38238383", "zh-CN", false)]

        [InlineData("0987123456", "zh-TW", true)]
        [InlineData("+886987123456", "zh-TW", true)]
        [InlineData("886987123456", "zh-TW", true)]
        [InlineData("+886-987123456", "zh-TW", true)]
        [InlineData("886-987123456", "zh-TW", true)]
        [InlineData("12345", "zh-TW", false)]
        [InlineData("", "zh-TW", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "zh-TW", false)]
        [InlineData("0-987123456", "zh-TW", false)]

        [InlineData("15323456787", "en", false)]
        [InlineData("13523333233", "en", false)]
        [InlineData("13898728332", "en", false)]
        [InlineData("+086-13238234822", "en", false)]
        [InlineData("08613487234567", "en", false)]
        [InlineData("8617823492338", "en", false)]
        [InlineData("86-17823492338", "en", false)]

        [InlineData("0821231234", "en-ZA", true)]
        [InlineData("+27821231234", "en-ZA", true)]
        [InlineData("27821231234", "en-ZA", true)]
        [InlineData("082123", "en-ZA", false)]
        [InlineData("08212312345", "en-ZA", false)]
        [InlineData("21821231234", "en-ZA", false)]
        [InlineData("+21821231234", "en-ZA", false)]
        [InlineData("+0821231234", "en-ZA", false)]

        [InlineData("61404111222", "en-AU", true)]
        [InlineData("+61411222333", "en-AU", true)]
        [InlineData("0417123456", "en-AU", true)]
        [InlineData("082123", "en-AU", false)]
        [InlineData("08212312345", "en-AU", false)]
        [InlineData("21821231234", "en-AU", false)]
        [InlineData("+21821231234", "en-AU", false)]
        [InlineData("+0821231234", "en-AU", false)]

        [InlineData("0612457898", "fr-FR", true)]
        [InlineData("+33612457898", "fr-FR", true)]
        [InlineData("33612457898", "fr-FR", true)]
        [InlineData("0712457898", "fr-FR", true)]
        [InlineData("+33712457898", "fr-FR", true)]
        [InlineData("33712457898", "fr-FR", true)]

        [InlineData("061245789", "fr-FR", false)]
        [InlineData("06124578980", "fr-FR", false)]
        [InlineData("0112457898", "fr-FR", false)]
        [InlineData("0212457898", "fr-FR", false)]
        [InlineData("0312457898", "fr-FR", false)]
        [InlineData("0412457898", "fr-FR", false)]
        [InlineData("0512457898", "fr-FR", false)]
        [InlineData("0812457898", "fr-FR", false)]
        [InlineData("0912457898", "fr-FR", false)]
        [InlineData("+34612457898", "fr-FR", false)]
        [InlineData("+336124578980", "fr-FR", false)]
        [InlineData("+3361245789", "fr-FR", false)]

        [InlineData("2102323234", "el-GR", true)]
        [InlineData("+302646041461", "el-GR", true)]
        [InlineData("+306944848966", "el-GR", true)]
        [InlineData("6944848966", "el-GR", true)]

        [InlineData("120000000", "el-GR", false)]
        [InlineData("20000000000", "el-GR", false)]
        [InlineData("68129485729", "el-GR", false)]
        [InlineData("6589394827", "el-GR", false)]
        [InlineData("298RI89572", "el-GR", false)]

        [InlineData("91234567", "en-HK", true)]
        [InlineData("9123-4567", "en-HK", true)]
        [InlineData("61234567", "en-HK", true)]
        [InlineData("51234567", "en-HK", true)]
        [InlineData("+85291234567", "en-HK", true)]
        [InlineData("+852-91234567", "en-HK", true)]
        [InlineData("+852-9123-4567", "en-HK", true)]
        [InlineData("852-91234567", "en-HK", true)]

        [InlineData("999", "en-HK", false)]
        [InlineData("+852-912345678", "en-HK", false)]
        [InlineData("123456789", "en-HK", false)]
        [InlineData("+852-1234-56789", "en-HK", false)]

        // Lack of test data in original input.js.
        [InlineData("+351919706735", "pt-PT", true)]

        [InlineData("447789345856", "en-GB", true)]
        [InlineData("+447861235675", "en-GB", true)]
        [InlineData("07888814488", "en-GB", true)]
        [InlineData("67699567", "en-GB", false)]
        [InlineData("0773894868", "en-GB", false)]
        [InlineData("077389f8688", "en-GB", false)]
        [InlineData("+07888814488", "en-GB", false)]
        [InlineData("0152456999", "en-GB", false)]
        [InlineData("442073456754", "en-GB", false)]
        [InlineData("+443003434751", "en-GB", false)]
        [InlineData("05073456754", "en-GB", false)]
        [InlineData("08001123123", "en-GB", false)]

        [InlineData("19876543210", "en-US", true)]
        [InlineData("8005552222", "en-US", true)]
        [InlineData("+15673628910", "en-US", true)]
        [InlineData("564785", "en-US", false)]
        [InlineData("0123456789", "en-US", false)]
        [InlineData("1437439210", "en-US", false)]
        [InlineData("8009112340", "en-US", false)]
        [InlineData("+10345672645", "en-US", false)]
        [InlineData("11435213543", "en-US", false)]
        [InlineData("2436119753", "en-US", false)]
        [InlineData("16532116190", "en-US", false)]

        [InlineData("0956684590", "en-ZM", true)]
        [InlineData("0966684590", "en-ZM", true)]
        [InlineData("0976684590", "en-ZM", true)]
        [InlineData("+260956684590", "en-ZM", true)]
        [InlineData("+260966684590", "en-ZM", true)]
        [InlineData("+260976684590", "en-ZM", true)]
        [InlineData("12345", "en-ZM", false)]
        [InlineData("", "en-ZM", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "en-ZM", false)]
        [InlineData("010-38238383", "en-ZM", false)]
        [InlineData("966684590", "en-ZM", false)]
        [InlineData("260976684590", "en-ZM", false)]

        [InlineData("+79676338855", "ru-RU", true)]
        [InlineData("79676338855", "ru-RU", true)]
        [InlineData("89676338855", "ru-RU", true)]
        [InlineData("9676338855", "ru-RU", true)]
        [InlineData("12345", "ru-RU", false)]
        [InlineData("", "ru-RU", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "ru-RU", false)]
        [InlineData("010-38238383", "ru-RU", false)]
        [InlineData("+9676338855", "ru-RU", false)]
        [InlineData("19676338855", "ru-RU", false)]
        [InlineData("6676338855", "ru-RU", false)]
        [InlineData("+99676338855", "ru-RU", false)]

        [InlineData("+4796338855", "nb-NO", true)]
        [InlineData("+4746338855", "nb-NO", true)]
        [InlineData("4796338855", "nb-NO", true)]
        [InlineData("4746338855", "nb-NO", true)]
        [InlineData("46338855", "nb-NO", true)]
        [InlineData("96338855", "nb-NO", true)]
        [InlineData("12345", "nb-NO", false)]
        [InlineData("", "nb-NO", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "nb-NO", false)]
        [InlineData("+4676338855", "nb-NO", false)]
        [InlineData("19676338855", "nb-NO", false)]
        [InlineData("+4726338855", "nb-NO", false)]
        [InlineData("4736338855", "nb-NO", false)]
        [InlineData("66338855", "nb-NO", false)]

        [InlineData("+4796338855", "nn-NO", true)]
        [InlineData("+4746338855", "nn-NO", true)]
        [InlineData("4796338855", "nn-NO", true)]
        [InlineData("4746338855", "nn-NO", true)]
        [InlineData("46338855", "nn-NO", true)]
        [InlineData("96338855", "nn-NO", true)]
        [InlineData("12345", "nn-NO", false)]
        [InlineData("", "nn-NO", false)]
        [InlineData("Vml2YW11cyBmZXJtZtesting123", "nn-NO", false)]
        [InlineData("+4676338855", "nn-NO", false)]
        [InlineData("19676338855", "nn-NO", false)]
        [InlineData("+4726338855", "nn-NO", false)]
        [InlineData("4736338855", "nn-NO", false)]
        [InlineData("66338855", "nn-NO", false)]
        
        public void IsMobilePhone(string input, string locale, bool expected)
        {
            var actual = Validator.IsMobilePhone(input, locale);
            Assert.Equal(expected, actual);
        }
    }
}
