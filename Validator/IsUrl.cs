using System;
using System.Collections.Generic;
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

		private struct CheckOutput
		{
			public bool IsValid;
			public string NewUrl;
		}

		public static bool IsUrl(string url, UrlOptions options = null)
		{
			options = options ?? new UrlOptions();

			if (string.IsNullOrWhiteSpace(url) || url.Length >= 2083 || url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			var output = new CheckOutput();
			var checkFunctions = new List<Func<string, UrlOptions, CheckOutput>>
			{
				CheckProtocol,
				CheckHash,
				CheckQueryString,
				CheckPath,
				CheckAuth,
				CheckHost
			};

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

		private static CheckOutput CheckHash(string url, UrlOptions options)
		{
			var output = new CheckOutput();
			var hashIndex = url.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
			if (hashIndex > -1)
			{
				var hashValue = url.Substring(hashIndex + 1);
				output.NewUrl = url.Substring(0, hashIndex);
				output.IsValid = !string.IsNullOrWhiteSpace(hashValue);
			}
			else
			{
				output.NewUrl = url;
				output.IsValid = true;
			}

			return output;
		}

		private static CheckOutput CheckQueryString(string url, UrlOptions options)
		{
			var output = new CheckOutput();
			var queryStringIndex = url.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
			if (queryStringIndex > -1)
			{
				var queryStringValue = url.Substring(queryStringIndex + 1);
				output.NewUrl = url.Substring(0, queryStringIndex);
				output.IsValid = !string.IsNullOrWhiteSpace(queryStringValue);
			}
			else
			{
				output.NewUrl = url;
				output.IsValid = true;
			}

			return output;
		}

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
					output.IsValid = !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass);
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

		private static CheckOutput CheckHost(string url, UrlOptions options)
		{
			var output = new CheckOutput();
			var atIndex = url.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);
			//if atIndex is -1, then we'll just get the whole substring
			var hostName = url.Substring(atIndex+1);
			var colonIndex = hostName.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
			var host = string.Empty;

			// don't care about modifiedUrl here
			output.NewUrl = url;

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
					output.IsValid = false;
					return output;
				}
			}

			var isIp = Validator.IsIp(host, IpVersion.Four) || Validator.IsIp(host, IpVersion.Six);
			var isFqdn = Validator.IsFqdn(host);

			if (!isIp && !isFqdn &&
			    !string.Equals(host, "localhost", StringComparison.InvariantCultureIgnoreCase))
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
