using System;
using System.Linq;
using System.Text.RegularExpressions;

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
			public string[] Protocols { get; private set; }

			/// <summary>
			/// Gets whether to require Tld or not.
			/// </summary>
			public bool RequireTld { get; private set; }

			/// <summary>
			/// Gets whether to require a protocol or not.
			/// </summary>
			public bool RequireProtocol { get; private set; }

			/// <summary>
			/// Gets whether to allow underscores or not.
			/// </summary>
			public bool AllowUnderscores { get; private set; }

			/// <summary>
			/// Gets whether to allow a trailing dot or not.
			/// </summary>
			public bool AllowTrailingDot { get; private set; }

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

		public static bool IsUrl(string url, UrlOptions options = null)
		{
			options = options ?? new UrlOptions();

			if (string.IsNullOrWhiteSpace(url) || url.Length >= 2083 || url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			var newUrl = string.Empty;

			// parallelize!
			if (!CheckProtocol(url, options, out newUrl))
			{
				return false;
			}

			url = newUrl;

			//remove #...
			if (!CheckHash(url, options, out newUrl))
			{
				return false;
			}

			url = newUrl;

			//remove ?...
			if (!CheckQueryString(url, options, out newUrl))
			{
				return false;
			}

			url = newUrl;

			//remove /...
			if (!CheckPath(url, options, out newUrl))
			{
				return false;
			}

			url = newUrl;

			if (!CheckAuth(url, options, out newUrl))
			{
				return false;
			}

			url = newUrl;

			if (!CheckHost(url, options, out newUrl))
			{
				return false;
			}

			return true;
		}

		private static bool CheckProtocol(string url, UrlOptions options, out string modifiedUrl)
		{
			var protocolEndIndex = url.IndexOf("://", StringComparison.InvariantCultureIgnoreCase);
			if (protocolEndIndex > -1)
			{
				var protocol = url.Substring(0, protocolEndIndex);
				// this is not a one character indexof, so need to account for all three character we were looking for
				modifiedUrl = url.Substring(protocolEndIndex + 3);
				return options.Protocols.Contains(protocol);
			}
			else
			{
				modifiedUrl = url;
				return !options.RequireProtocol;
			}
		}

		private static bool CheckHash(string url, UrlOptions options, out string modifiedUrl)
		{
			var hashIndex = url.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
			if (hashIndex > -1)
			{
				var hashValue = url.Substring(hashIndex + 1);
				modifiedUrl = url.Substring(0, hashIndex);
				return !string.IsNullOrWhiteSpace(hashValue);
			}
			else
			{
				modifiedUrl = url;
				return true;
			}
		}

		private static bool CheckQueryString(string url, UrlOptions options, out string modifiedUrl)
		{
			var queryStringIndex = url.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
			if (queryStringIndex > -1)
			{
				var queryStringValue = url.Substring(queryStringIndex + 1);
				modifiedUrl = url.Substring(0, queryStringIndex);
				return !string.IsNullOrWhiteSpace(queryStringValue);
			}
			else
			{
				modifiedUrl = url;
				return true;
			}
		}

		private static bool CheckPath(string url, UrlOptions options, out string modifiedUrl)
		{
			var pathIndex = url.IndexOf("/", StringComparison.InvariantCultureIgnoreCase);
			if (pathIndex > -1)
			{
				var queryStringValue = url.Substring(pathIndex + 1);
				modifiedUrl = url.Substring(0, pathIndex);
				if (string.IsNullOrEmpty(queryStringValue))
				{
					return true;
				}
				else
				{
					return !queryStringValue.Contains(" ");
				}
				//return !string.IsNullOrWhiteSpace(queryStringValue);
			}
			else
			{
				modifiedUrl = url;
				return true;
			}
		}

		private static bool CheckAuth(string url, UrlOptions options, out string modifiedUrl)
		{
			var authIndex = url.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);
			modifiedUrl = url;
			if (authIndex > -1)
			{
				var authValue = url.Substring(0, authIndex);
				var colonIndex = authValue.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
				if (colonIndex > -1)
				{
					var user = authValue.Substring(0, colonIndex);
					var pass = authValue.Substring(colonIndex + 1);
					return !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass);
				}
				return true;
			}
			else
			{
				return true;
			}
		}

		private static bool CheckHost(string url, UrlOptions options, out string modifiedUrl)
		{
			var atIndex = url.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);
			//if atIndex is -1, then we'll just get the whole substring
			var hostName = url.Substring(atIndex+1);
			var colonIndex = hostName.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
			var host = string.Empty;

			// don't care about modifiedUrl here
			modifiedUrl = url;

			if (colonIndex == -1)
			{
				host = hostName;
			}
			else
			{
				host = hostName.Substring(0, colonIndex);
				var port = -1;
				Int32.TryParse(hostName.Substring(colonIndex + 1), out port);
				if (port <= 0 || port > 65535)
				{
					return false;
				}
			}

			var isIp = Validator.IsIp(host, IpVersion.Four) || Validator.IsIp(host, IpVersion.Six);
			var isFqdn = Validator.IsFqdn(host);

			if (!isIp && !isFqdn &&
			    !string.Equals(host, "localhost", StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			if (options.HostWhitelist != null && !options.HostWhitelist.Contains(host))
			{
				return false;
			}

			if (options.HostBlacklist != null && !options.HostBlacklist.Contains(host))
			{
				return false;
			}

			return true;
		}
	}
}
