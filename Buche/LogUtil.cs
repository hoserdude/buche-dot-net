using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Buche
{

	/// <summary>
	/// LogUtil - Log utility for our value add over-and-above the logging providers.
	/// </summary>
	public static class LogUtil
	{
        private const string IpAddressRegexString = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        
        private static readonly Regex IpAddressRegex = new Regex(IpAddressRegexString);
        public static bool IsPrimitiveExtended(this Type t)
        {
            return t.IsPrimitive || t == typeof(string) || t == typeof(Decimal) || t == typeof(DateTime) ||
                   t == typeof(TimeSpan);
        }

        /// <summary>
        /// Answers true if this String is neither null or empty.
        /// </summary>
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s) && !string.IsNullOrEmpty(s.Trim());
        }

    	/// <summary>
        /// Iterates through a SqlParameterCollection to output the names and values in one line.
        /// </summary>
        /// <param name="sqlParameterCollection"></param>
        /// <returns></returns>
        public static string DumpSqlParameters(SqlParameterCollection sqlParameterCollection)
        {
            var buffer = new StringBuilder();
            foreach (SqlParameter p in sqlParameterCollection)
            {
                buffer.Append('{').Append("param_name=").Append(p.ParameterName).Append(',');
                buffer.Append("param_value=").Append(p.Value).Append('}');
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Get the filename and line number.
        /// </summary>
        /// <param name="skipFrames">Number of stack frames to skip. It skips one frame by default, i.e., 
        /// gets the filename and line number of the immediate caller of this function.</param>
        /// <returns>String with the filename and line number.</returns>
        public static string GetFileInfo(int skipFrames = 1)
        {
            var stackFrame = new StackFrame(skipFrames, true);
            return string.Format("{0}({1})", stackFrame.GetFileName(), stackFrame.GetFileLineNumber());
        }

		/// <summary>
		/// Determines if the given key is sensitive or not.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool IsSensitiveKey(string key)
		{
			var lower = key.ToLower();
			return (lower.Contains("password") 
                || lower.Contains("passwd") 
                || lower.Contains("securityanswer")
                || lower.Contains("challengequestion")
                || lower.Contains("challengeanswer")
                || lower.Contains("secret") 
                || lower.Contains("passcode"));
		}

        public static string GetOriginationIp(this HttpRequestBase httpRequest)
        {
            //IIS Server var
            string originIp = httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (originIp.HasValue() && IpAddressRegex.IsMatch(originIp))
            {
                originIp = originIp.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            }

            //Http Header
            if (!originIp.HasValue())
            {
                originIp = httpRequest.Headers["X-Forwarded-For"];
                if (originIp.HasValue() && IpAddressRegex.IsMatch(originIp))
                {
                    originIp = originIp.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                }
            }

            //Non-proxied IP
            if (!originIp.HasValue())
            {
                originIp = httpRequest.UserHostAddress;
            }

            return originIp;
        }
    }
}
