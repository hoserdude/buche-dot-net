using log4net;
using Microsoft.Practices.Unity;
using System;
using log4net.Appender;

namespace Buche
{
    /// <summary>
    /// Adapter Logging implementation for Log4Net Provider.  Converts Expceptions to one lines using LogUtil.
    /// </summary>
    public class Log4NetLoggerAdapter : ILogger
    {
        private const string MessagePrefix = "msg=";


// ReSharper disable UnusedMember.Local
        private Log4NetLoggerAdapter()
// ReSharper restore UnusedMember.Local
        {
            //prevent callers from instantiating this directly.
            //no DI, no love
        }

        public ILog Logger { get; set; }

        [InjectionConstructor]
        public Log4NetLoggerAdapter(System.Reflection.MethodBase callerMethod)
        {
            Logger = LogManager.GetLogger(callerMethod.DeclaringType);
        }

        public bool IsDebugEnabled
        {
            get { return Logger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return Logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return Logger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return Logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return Logger.IsWarnEnabled; }
        }

        public void Debug(object message)
        {
            Logger.Debug(MessagePrefix + message);
        }

        public void Debug(object message, Exception exception)
        {
            Logger.Debug(MessagePrefix + message, exception);
        }

        public void Error(object message)
        {
            Logger.Error(MessagePrefix + message);
        }

        public void Error(object message, Exception exception)
        {
            Logger.Error(MessagePrefix + message, exception);
        }

        public void Fatal(object message)
        {
            Logger.Fatal(MessagePrefix + message);
        }

        public void Fatal(object message, Exception exception)
        {
            Logger.Fatal(MessagePrefix + message, exception);
        }

        public void Info(object message)
        {
            Logger.Info(MessagePrefix + message);
        }

        public void InfoWithoutMsgPrefix(object message)
        {
            Logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            Logger.Info(MessagePrefix + message, exception);
        }

        public void Warn(object message)
        {
            Logger.Warn(MessagePrefix + message);
        }

        public void Warn(object message, Exception exception)
        {
            Logger.Warn(MessagePrefix + message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Logger.DebugFormat(MessagePrefix + format, args);
        }

        public void DebugFormat(Exception ex, string format, params object[] args)
        {
            Logger.Debug(string.Format(MessagePrefix + format, args), ex);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(MessagePrefix + format, args);
        }

        public void ErrorFormat(Exception ex, string format, params object[] args)
        {
            Logger.Error(string.Format(MessagePrefix + format, args), ex);
        }

        public void FatalFormat(string format, params object[] args)
        {
            Logger.FatalFormat(MessagePrefix + format, args);
        }

        public void FatalFormat(Exception ex, string format, params object[] args)
        {
            Logger.Fatal(string.Format(MessagePrefix + format, args), ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(MessagePrefix + format, args);
        }

        public void InfoFormat(Exception ex, string format, params object[] args)
        {
            Logger.Info(string.Format(MessagePrefix + format, args), ex);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Logger.WarnFormat(MessagePrefix + format, args);
        }

        public void WarnFormat(Exception ex, string format, params object[] args)
        {
            Logger.Warn(string.Format(MessagePrefix + format, args), ex);
        }


        /// <summary>
        /// Get the property as string for the given key.
        /// </summary>
        /// <param name="key">A non null string representing a key in the properties bag.</param>
        /// <returns>A string representation of the property object if it exists for a valid key, null otherwise.</returns>
        public string GetProperty(string key)
        {
            // Returns the object as string if it exists for a valid key (not null).
            if (!string.IsNullOrEmpty(key) &&
                ThreadContext.Properties != null &&
                ThreadContext.Properties[key] != null)
            {
                return ThreadContext.Properties[key].ToString();
            }

            return null;
        }

        /// <summary>
        /// Set a property in the Properties bag.
        /// </summary>
        /// <param name="key">A non null string representing a key in the properties bag.</param>
        /// <param name="value">A new value for the key. If value null, then the key is removed from the property bag.</param>
        public void SetProperty(string key, string value)
        {
            // If the key is not valid then returns.
            if (string.IsNullOrEmpty(key) || ThreadContext.Properties == null)
                return;

            if (value == null)
            {
                // If new value is null, then remove the property if it exists.
                if (ThreadContext.Properties[key] != null)
                {
                    // Removing it from the Properties maps does not work. It still keeps logging the old value.
                    // We will set the existing property to null instead of trying to remove it.
                    //ThreadContext.Properties.Remove(key);
                    ThreadContext.Properties[key] = AdaptivePropertyProviderFactory.Create(key, null as string);
                }
            }
            else
            {
                ThreadContext.Properties[key] = AdaptivePropertyProviderFactory.Create(key, value);
            }
        }

        public string GetLastLogMessage(out LogLevel logLevel)
        {
            // not supposed to be implemented in a real world logger, only used for testing purposes
            throw new NotImplementedException();
        }
    }
}
