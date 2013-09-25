
namespace Buche
{
    public static class LoggerExtensions
    {
        public static void SetRequestInfo(this ILogger logger, RequestInfo requestInfo)
        {
            logger.SetRequestId(requestInfo.SessionId, requestInfo.RequestId);
        }

        public static RequestInfo GetRequestInfo(this ILogger logger)
        {
            var sessionId = logger.GetProperty(LogContext.PropertyKey.SessionId);
            var requestId = logger.GetProperty(LogContext.PropertyKey.RequestId);
            
            return new RequestInfo(sessionId, requestId);
        }

        /// <summary>
        /// Sets the requestId variable in the log4net ThreadContext to trace sessions.
        /// </summary>
        public static void SetRequestId(this ILogger logger, string sessionId, string requestId)
        {
            logger.SetProperty(LogContext.PropertyKey.SessionId, sessionId);
            logger.SetProperty(LogContext.PropertyKey.RequestId, requestId);
        }

        /// <summary>
        /// Gets the requestId variable in the log4net ThreadContext to trace sessions.
        /// </summary>
        public static string GetRequestId(this ILogger logger)
        {
            var requestInfo = logger.GetRequestInfo();

            return (requestInfo != null) ? requestInfo.ToString() : null;
        }

        public static LogContext GetContext(this ILogger logger)
        {
            return LogContext.Get(logger);
        }

        public static void SetContext(this ILogger logger, LogContext context)
        {
            context.Set(logger);
        }

        public static void ClearContext(this ILogger logger)
        {
            LogContext.Clear(logger);
        }

        public static LogOperation CreateOperation(this ILogger logger, string value)
        {
            return new LogOperation(logger, value);
        }
    }
}
