using System;
using System.Collections.Generic;
using System.Linq;
//using Validator.ExtensionMethods;
using static System.Int32;

namespace Validator
{
    public partial class Validator
    {
        /// <summary>
        /// Simple class used to encapsulate options when checking a string for Url compatability.
        /// </summary>
        public class UrlOptions
        {
            /// <summary>
            /// Gets the collection of protocols to allow.
            /// </summary>
            public string[] Protocols { get; }

            /// <summary>
            /// Gets whether to require Tld or not.
            /// </summary>
            public bool RequireTld { get; }

            /// <summary>
            /// Gets whether to require a protocol or not.
            /// </summary>
            public bool RequireProtocol { get; }

            /// <summary>
            /// Gets whether to allow underscores or not.
            /// </summary>
            public bool AllowUnderscores { get; }

            /// <summary>
            /// Gets whether to allow a trailing dot or not.
            /// </summary>
            public bool AllowTrailingDot { get; }

            /// <summary>
            /// Gets the optional list of whitelist hosts.
            /// </summary>
            public string[] HostWhitelist { get; private set; }

            /// <summary>
            /// Gets the optional list of blacklist hosts.
            /// </summary>
            public string[] HostBlacklist { get; private set; }

            /// <summary>
            /// Instantiates a new UrlOptions object.
            /// </summary>
            /// <param name="protocols">The protocols to support. If none provided, default values will be used.</param>
            /// <param name="requireTld">Whether to require Tld or not.</param>
            /// <param name="requireProtocol">Whether to require a protocol or not.</param>
            /// <param name="allowUnderscores">Whether to allow underscores or not.</param>
            /// <param name="allowTrailingDot">Whether to allow a trailing dot or not.</param>
            public UrlOptions(string[] protocols = null, bool requireTld = true, bool requireProtocol = false,
                bool allowUnderscores = false, bool allowTrailingDot = false)
            {
                Protocols = protocols ?? new[] { "http", "https", "ftp" };
                RequireTld = requireTld;
                RequireProtocol = requireProtocol;
                AllowUnderscores = allowUnderscores;
                AllowTrailingDot = allowTrailingDot;

                HostWhitelist = null;
                HostBlacklist = null;
            }

            /// <summary>
            /// Sets the optional array of whitelist hosts.
            /// </summary>
            /// <param name="whitelist">Array of whitelist hosts to use.</param>
            public void SetWhitelist(string[] whitelist)
            {
                HostWhitelist = whitelist;
            }

            /// <summary>
            /// Sets the optional array of blacklist hosts.
            /// </summary>
            /// <param name="blacklist">Array of blacklist hosts to use.</param>
            public void SetBlacklist(string[] blacklist)
            {
                HostBlacklist = blacklist;
            }
        }

        /// <summary>
        /// Struct to hold the two pieces of data needed for each Url check sub-function.
        /// </summary>
        private struct CheckOutput
        {
            /// <summary>
            /// Whether or not the prospective Url met the criteria for the given function.
            /// </summary>
            public bool IsValid;
            /// <summary>
            /// The modified Url to use from this point forward.
            /// </summary>
            public string NewUrl;
        }

        /// <summary>
        /// Determines whether the given string value, <paramref name="url"/>, qualifies as a Url.
        /// </summary>
        /// <param name="url">Value to check.</param>
        /// <param name="options">Options to consider.</param>
        /// <returns>True if a Url, false otherwise.</returns>
        public static bool IsUrl(string url, UrlOptions options = null)
        {
            options = options ?? new UrlOptions();

            if (string.IsNullOrEmpty(url) || url.Length >= 2083 || url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var output = new CheckOutput();
            // i purposely structured each of these "check" methods the same way so this list of Funcs works.
            var checkFunctions = new List<Func<string, UrlOptions, CheckOutput>>
			{
				CheckProtocol,
				CheckHash,
				CheckQueryString,
				CheckPath,
				CheckAuth,
				CheckHost
			};

            // Mimicking the source validator.js means we are monkeying with the url value in many of these functions
            // as we trim down the url string to verify. In other words, most of these methods remove the portion of the string that they validate
            // only leaving behind that which has yet to be validated.
            foreach (var f in checkFunctions)
            {
                output = f(url, options);
                if (!output.IsValid)
                {
                    break;
                }
                
                url = output.NewUrl;
            }

            return output.IsValid;
        }

        /// <summary>
        /// Checks the url string for protocol violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Specifically, this will conditionally check the RequireProtocol option.</param>
        /// <returns>True if it meets the protocol standards, false otherwise.</returns>
        private static CheckOutput CheckProtocol(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var protocolEndIndex = url.IndexOf("://", StringComparison.InvariantCultureIgnoreCase);
            if (protocolEndIndex > -1)
            {
                var protocol = url.Substring(0, protocolEndIndex);
                // this is not a one character indexof, so need to account for all three character we were looking for
                output.NewUrl = url.Substring(protocolEndIndex + 3);
                output.IsValid = options.Protocols.Contains(protocol);
            }
            else
            {
                output.NewUrl = url;
                output.IsValid = !options.RequireProtocol;
            }

            return output;
        }

        /// <summary>
        /// Checks the url string for hash violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Ignored.</param>
        /// <returns>True if it meets the hash standards, false otherwise.</returns>
        private static CheckOutput CheckHash(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var hashIndex = url.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
            if (hashIndex > -1)
            {
                var hashValue = url.Substring(hashIndex + 1);
                output.NewUrl = url.Substring(0, hashIndex);
                output.IsValid = !string.IsNullOrEmpty(hashValue);
            }
            else
            {
                output.NewUrl = url;
                output.IsValid = true;
            }

            return output;
        }

        /// <summary>
        /// Checks the url string for query string violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Ignored.</param>
        /// <returns>True if it meets the query string standards, false otherwise.</returns>
        private static CheckOutput CheckQueryString(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var queryStringIndex = url.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
            if (queryStringIndex > -1)
            {
                var queryStringValue = url.Substring(queryStringIndex + 1);
                output.NewUrl = url.Substring(0, queryStringIndex);
                output.IsValid = !string.IsNullOrEmpty(queryStringValue);
            }
            else
            {
                output.NewUrl = url;
                output.IsValid = true;
            }

            return output;
        }

        /// <summary>
        /// Checks the url string for path violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Ignored.</param>
        /// <returns>True if it meets the path standards, false otherwise.</returns>
        private static CheckOutput CheckPath(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var pathIndex = url.IndexOf("/", StringComparison.InvariantCultureIgnoreCase);
            if (pathIndex > -1)
            {
                var queryStringValue = url.Substring(pathIndex + 1);
                output.NewUrl = url.Substring(0, pathIndex);
                if (string.IsNullOrEmpty(queryStringValue))
                {
                    output.IsValid = true;
                }
                else
                {
                    output.IsValid = !queryStringValue.Contains(" ");
                }
            }
            else
            {
                output.NewUrl = url;
                output.IsValid = true;
            }

            return output;
        }

        /// <summary>
        /// Checks the url string for authentication violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Ignored.</param>
        /// <returns>True if it meets the authorization standards, false otherwise.</returns>
        private static CheckOutput CheckAuth(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var authIndex = url.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);
            output.NewUrl = url;
            if (authIndex > -1)
            {
                var authValue = url.Substring(0, authIndex);
                var colonIndex = authValue.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (colonIndex > -1)
                {
                    var user = authValue.Substring(0, colonIndex);
                    var pass = authValue.Substring(colonIndex + 1);
                    output.IsValid = !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass);
                }
                else
                {
                    output.IsValid = true;
                }
            }
            else
            {
                output.IsValid = true;
            }

            return output;
        }

        /// <summary>
        /// Checks the url string for host violations.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <param name="options">The options to consider. Specifically, it will check white and blaclist entries, if available.</param>
        /// <returns>True if it meets the host standards, false otherwise.</returns>
        private static CheckOutput CheckHost(string url, UrlOptions options)
        {
            var output = new CheckOutput();
            var atIndex = url.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);
            //if atIndex is -1, then we'll just get the whole substring
            var hostName = url.Substring(atIndex + 1);
            var colonIndex = hostName.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
            string host;

            // don't care about modifiedUrl here since this is the last method and there's nothing left to check.
            output.NewUrl = string.Empty;

            if (colonIndex == -1)
                host = hostName;
            else
            {
                host = hostName.Substring(0, colonIndex);
                int port;
                TryParse(hostName.Substring(colonIndex + 1), out port);
                if (port <= 0 || port > 65535)
                {
                    output.IsValid = false;
                    return output;
                }
            }

            var isIp = Validator.IsIp(host, IpVersion.Four) || Validator.IsIp(host, IpVersion.Six);
            var isFqdn = Validator.IsFqdn(host);

            if (!isIp && !isFqdn && !string.Equals(host, "localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                output.IsValid = false;
                return output;
            }

            if (options.HostWhitelist != null && !options.HostWhitelist.Contains(host))
            {
                output.IsValid = false;
                return output;
            }

            if (options.HostBlacklist != null && !options.HostBlacklist.Contains(host))
            {
                output.IsValid = false;
                return output;
            }

            output.IsValid = true;
            return output;
        }
    }
}
