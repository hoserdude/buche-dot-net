using System;
using System.Text;

namespace Buche
{
    public class RequestInfo
    {
        public static readonly string UninitializedSessionId = "SessionIdNull";
        public static readonly string UninitializedRequestId = "RequestIdNull";

        private const char Separator = '-';

        public RequestInfo(string sessionId, string requestId)
        {
            SessionId = sessionId;
            RequestId = requestId;
        }

        public string SessionId { get; private set; }
        public string RequestId { get; private set; }

        public static RequestInfo FromString(string requestInfoString)
        {
            var tokens = requestInfoString.Split(new[] { Separator }, StringSplitOptions.None);

            string sessionId = null;
            string requestId = null;

            if (tokens.Length == 2)
            {
                sessionId = tokens[0];
                requestId = tokens[1];
            }
            else if (tokens.Length == 1)
            {
                requestId = tokens[0];
            }

            return new RequestInfo(sessionId, requestId);
        }

        public override string ToString()
        {
            if ((SessionId != null) &&
                (!string.IsNullOrEmpty(SessionId) && !SessionId.Equals(UninitializedSessionId))) // sessionId is initialized to "SessionIdNull" 
            {
                var sb = new StringBuilder();
                sb.Append(SessionId);

                if (RequestId != null)
                {
                    sb.AppendFormat("{0}{1}", Separator, RequestId);
                }

                return sb.ToString();
            }

            //In session-less scenario like REST APIs we don't have a sessionID.
            if (RequestId != null)
            {
                return RequestId;
            }

            return string.Empty;
        }
    }
}
