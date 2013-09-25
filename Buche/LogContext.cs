using System.Collections.Generic;
using System.Reflection;

namespace Buche
{
    /// <summary>
    /// This class is used to capture and restore log context when switching threads
    /// within the context of System.Threading.Tasks.Task.
    /// </summary>
    public class LogContext
    {
        /// <summary>
        /// Valid keys for the Log4Net ThreadContext.Properties.
        /// </summary>
        public static class PropertyKey
        {
            private static readonly FieldInfo[] FieldInfos = typeof(PropertyKey).GetFields();
            public static FieldInfo[] Fields { get { return FieldInfos; } }

            public const string RequestId = "requestId";
            public const string SessionId = "sessionId";
            public const string IpAddress = "ipAddress";
            public const string UserLogin = "userLogin";
            public const string UserAgent = "userAgent";
            public const string Operation = "operation";
        }

        private static readonly LogContext DummyLogContext = new LogContext();

        public Dictionary<string, string> Properties { get; private set; }

        public LogContext()
            : this(new Dictionary<string, string>()) { }

        public LogContext(Dictionary<string, string> properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// Capture all log context values from logger and return a new LogContext object.
        /// </summary>
        internal static LogContext Get(ILogger logger)
        {
            var properties = new Dictionary<string, string>();

            foreach (var propertyKeyField in PropertyKey.Fields)
            {
                var propertyKey = propertyKeyField.GetValue(DummyLogContext) as string;
                var propertyValue = logger.GetProperty(propertyKey);
                if (propertyKey != null && properties.ContainsKey(propertyKey)) properties[propertyKey] = propertyValue;
            }

            return new LogContext(properties);
        }

        /// <summary>
        /// Set all log context values from this LogContext object to logger.
        /// </summary>
        internal void Set(ILogger logger)
        {
            if (Properties == null)
            {
                return;
            }

            foreach (var propertyKeyField in PropertyKey.Fields)
            {
                var propertyKey = propertyKeyField.GetValue(DummyLogContext) as string;

                if (propertyKey != null && Properties.ContainsKey(propertyKey))
                {
                    var propertyValue = Properties[propertyKey];
                    logger.SetProperty(propertyKey, propertyValue);
                }
            }
        }

        /// <summary>
        /// Clear log context.
        /// </summary>
        internal static void Clear(ILogger logger)
        {
            foreach (var field in PropertyKey.Fields)
            {
                var key = field.GetValue(DummyLogContext) as string;
                logger.SetProperty(key, null);
            }
        }
    }
}
